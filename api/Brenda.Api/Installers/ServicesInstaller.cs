using Brenda.Core.Services;
using Brenda.Infrastructure;
using Brenda.Infrastructure.Impl;
using Brenda.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Brenda.Api.Installers
{

    public class ServicesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICampaignServices, CampaignServices>();
            services.AddScoped<IUserRegisterService, UserRegisterService>();

            
        }
    }
}
