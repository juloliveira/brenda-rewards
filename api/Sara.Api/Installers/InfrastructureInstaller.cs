using Brenda.Infrastructure;
using Brenda.Infrastructure.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sara.Api.Infrasctructure;
using Sara.Api.Infrasctructure.Impl;
using Sara.Infrastructure.Models;

namespace Sara.Api.Installers
{
    public class InfrastructureInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<TenantInfo>();

            services.AddTransient<IPushNotifications, FCSPushNotifications>();

            services.AddTransient<IEmailSender, ElasticMailSender>();
            services.AddScoped<IEmailRenderer, RazorViewToStringRenderer>();
            
            services.AddScoped<IPushMessage, PushMessage>();
        }
    }
}
