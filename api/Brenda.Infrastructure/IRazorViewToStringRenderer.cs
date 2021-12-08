using Brenda.Infrastructure.Email;
using System.Threading.Tasks;

namespace Brenda.Infrastructure
{
    public interface IEmailRenderer
    {
        Task<RenderedEmail> RenderHtmlAsync<TModel>(TModel model, string folder = null);
    }
}
