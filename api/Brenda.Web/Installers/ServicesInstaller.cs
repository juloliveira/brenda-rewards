using Brenda.Core.Services;
using Brenda.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Brenda.Web.Installers
{

    public class ServicesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICampaignValidator, CampaignValidation>();
            services.AddScoped<IUserRegisterService, UserRegisterService>();
            
            services.AddScoped<ICampaignPublisher, CampaignPublisher>();
        }
    }
}
