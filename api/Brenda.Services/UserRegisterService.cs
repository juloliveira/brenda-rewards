using Brenda.Contracts.Emails;
using Brenda.Contracts.V1.Requests;
using Brenda.Core;
using Brenda.Core.Interfaces;
using Brenda.Core.Services;
using Brenda.Infrastructure;
using Brenda.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Brenda.Services
{
    public class UserRegisterService : IUserRegisterService
    {
        private readonly UserManager<BrendaUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IEmailRenderer _razorRenderer;
        private readonly EndpointOptions _endpoints;

        public UserRegisterService(
            UserManager<BrendaUser> userManager, 
            IEmailSender emailSender,
            IEmailRenderer razorRenderer,
            IOptions<EndpointOptions> endpoints)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _razorRenderer = razorRenderer;

            if (endpoints == null) throw new ArgumentNullException(nameof(endpoints));
            _endpoints = endpoints.Value ?? null;
        }

        public async Task Register(
            string companyName,
            string companyDocument,
            string name,
            string email,
            string password,
            string role)
        {
            var customer = new Customer(companyName, companyDocument.ClearDocumentChars());
            var user = new BrendaUser(name, email);
            customer.AddUser(user);

            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, role);
            
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = _endpoints.Build(x => x.ConfirmEmail, token.Base64ForUrlEncode(), user.Email);

            var welcome = new WelcomeUser(user.Name, user.Email, callbackUrl);
            var emailRendered = await _razorRenderer.RenderHtmlAsync(welcome, folder: "Emails/");

            await _emailSender.SendEmail(user.Name, user.Email, emailRendered);
        }

        public async Task Register(Customer customer, string name, string email, string role)
        {
            var user = new BrendaUser(name, email, customer);
            
            await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, role);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = _endpoints.Build(x => x.NewUserConfirmation, token.Base64ForUrlEncode(), user.Email);

            var welcome = new NewUserConfirmation(user.Name, user.Email, callbackUrl);
            var emailRendered = await _razorRenderer.RenderHtmlAsync(welcome, folder: "Emails/");

            await _emailSender.SendEmail(user.Name, user.Email, emailRendered);
        }

        public async Task ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            if (user == null) throw new Exception();

            await _userManager.ConfirmEmailAsync(user, token.Base64ForUrlDecode()).ConfigureAwait(false);
        }

        
    }
}
