using Brenda.Infrastructure;
using Brenda.Infrastructure.Impl;
using Brenda.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Brenda.Web.Installers
{
    public class InfrastructureInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<TenantInfo>();

            services.AddTransient<IPushNotifications, FCSPushNotifications>();

            services.AddTransient<IEmailSender, ElasticMailSender>();
            services.AddScoped<IEmailRenderer, RazorViewToStringRenderer>();
        }
    }
}
