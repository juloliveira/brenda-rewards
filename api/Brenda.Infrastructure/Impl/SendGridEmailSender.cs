using Brenda.Infrastructure.Email;
using Brenda.Utils;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Brenda.Infrastructure.Impl
{
    public class ElasticMailSender : IEmailSender
    {
        private readonly EmailSenderOptions _emailSenderOptions;

        public ElasticMailSender(IOptions<EmailSenderOptions> emailSenderOptions)
        {
            _emailSenderOptions = emailSenderOptions.Value;
        }

        public async Task SendEmail(string fullName, string emailAddress, RenderedEmail renderedEmail)
        {
            try
            {
                ElasticEmailClient.Api.ApiKey = "A2B75631C0D628DE6577E81F19F934444567B8FA50D9005F486BCC94E35C9DCC72083B4BC0A90CFA14CBDC9ECE55431A";
                var response = await ElasticEmailClient.Api.Email.SendAsync(
                    subject: renderedEmail.Subject,
                    bodyHtml: renderedEmail.HtmlEmail,
                    from: "brenda@brendarewards.com",
                    fromName: "Brenda Rewards",
                    msgTo: new[] { emailAddress });

                if (response == null)
                    throw new Exception("Erro ao enviar email.");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }

    public class SendGridEmailSender : IEmailSender
    {
        private readonly EmailSenderOptions _emailSenderOptions;

        public SendGridEmailSender(IOptions<EmailSenderOptions> emailSenderOptions)
        {
            _emailSenderOptions = emailSenderOptions.Value;
        }

        public async Task SendEmail(string fullName, string emailAddress, RenderedEmail renderedEmail)
        {
            var client = new SendGrid.SendGridClient(_emailSenderOptions.ApiKey);
            var from = new EmailAddress("brenda@brendarewards.com", "Brenda Rewards");
            var to = new EmailAddress(emailAddress, fullName ?? emailAddress);

            var msg = MailHelper.CreateSingleEmail(from, to, renderedEmail.Subject, null, renderedEmail.HtmlEmail);
            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
                throw new Exception(await response.Body.ReadAsStringAsync());
        }
    }
}
