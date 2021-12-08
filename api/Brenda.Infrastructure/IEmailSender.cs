using Brenda.Infrastructure.Email;
using System.Threading.Tasks;

namespace Brenda.Infrastructure
{
    public interface IEmailSender
    {
        Task SendEmail(string fullName, string emailAddress, RenderedEmail renderedEmail);
    }
}
