using Brenda.Infrastructure;
using Brenda.Infrastructure.Impl;
using Brenda.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Brenda.Api.Installers
{
    public class InfrastructureInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<TenantInfo>();

            services.AddTransient<IPushNotifications, FCSPushNotifications>();

            services.AddTransient<IEmailSender, SendGridEmailSender>();
            services.AddScoped<IEmailRenderer, RazorViewToStringRenderer>();
        }
    }
}
