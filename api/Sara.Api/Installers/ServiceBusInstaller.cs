using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sara.Api.Consumers;

namespace Sara.Api.Installers
{
    public class ServiceBusInstaller : IInstaller
    {
        //sw7N#@wp@WA9GHi@npy!XJNTm3ygPZS&
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var host = configuration.GetValue<string>("ServiceBus:Host");
            var virtualHost = configuration.GetValue<string>("ServiceBus:VirtualHost");
            var username = configuration.GetValue<string>("ServiceBus:Username");
            var password = configuration.GetValue<string>("ServiceBus:Password");
            services.AddMassTransit(x =>
            {
                x.SetSnakeCaseEndpointNameFormatter();
                
                x.AddConsumer<CustomerInfoConsumer>();
                x.AddConsumer<ChallengeOnGoingConsumer>();
                x.AddConsumer<CampaignOnGoingConsumer>();
                x.AddConsumer<TransferReceivedPushMessageConsumer>();
                x.AddConsumer<AwardPushMessageConsumer>();
                x.AddConsumer<BalanceChangePushMessageConsumer>();

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(host, virtualHost, cfg =>
                    {
                        cfg.Username(username);
                        cfg.Password(password);
                    });

                    config.ReceiveEndpoint("Sara:CustomerInfoConsumer", x => x.ConfigureConsumer<CustomerInfoConsumer>(context));
                    config.ReceiveEndpoint("Sara:ChallengeOnGoingConsumer", x => x.ConfigureConsumer<ChallengeOnGoingConsumer>(context));
                    config.ReceiveEndpoint("Sara:CampaignOnGoingConsumer", x => x.ConfigureConsumer<CampaignOnGoingConsumer>(context));
                    config.ReceiveEndpoint("Sara:TransferReceivedPushMessageConsumer", x => x.ConfigureConsumer<TransferReceivedPushMessageConsumer>(context));
                    config.ReceiveEndpoint("Sara:AwardPushMessageConsumer", x => x.ConfigureConsumer<AwardPushMessageConsumer>(context));
                    config.ReceiveEndpoint("Sara:BalanceChangePushMessageConsumer", x => x.ConfigureConsumer<BalanceChangePushMessageConsumer>(context));

                });
            });

            services.AddMassTransitHostedService();
        }

    }
}
