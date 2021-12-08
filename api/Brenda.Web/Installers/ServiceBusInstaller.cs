using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Brenda.Web.Installers
{
    public class ServiceBusInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var host = configuration.GetValue<string>("ServiceBus:Host");
            var virtualHost = configuration.GetValue<string>("ServiceBus:VirtualHost");
            var username = configuration.GetValue<string>("ServiceBus:Username");
            var password = configuration.GetValue<string>("ServiceBus:Password");
            services.AddMassTransit(x =>
            {
                x.SetSnakeCaseEndpointNameFormatter();
                //x.AddConsumer<UserRewardedConsumer>();

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(host, virtualHost, cfg =>
                    {
                        cfg.Username(username);
                        cfg.Password(password);
                    });
                });
            });

            services.AddMassTransitHostedService();
        }

    }
}
