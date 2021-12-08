using Carol.Api.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Carol.Api.Installers
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

                x.AddConsumer<ChallengeOnGoingConsumer>();
                x.AddConsumer<CampaignOnGoingConsumer>();

                x.AddConsumer<RewardUserConsumer>();
                x.AddConsumer<CreateUserConsumer>();
                x.AddConsumer<FirebaseUserTokenUpdateConsumer>();

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(host, virtualHost, cfg =>
                    {
                        cfg.Username(username);
                        cfg.Password(password);
                    });

                    config.ReceiveEndpoint("Carol:ChallengeOnGoingConsumer", x => x.ConfigureConsumer<ChallengeOnGoingConsumer>(context));
                    config.ReceiveEndpoint("Carol:RewardUserConsumer", x => x.ConfigureConsumer<RewardUserConsumer>(context));
                    config.ReceiveEndpoint("Carol:CreateUserConsumer", x => x.ConfigureConsumer<CreateUserConsumer>(context));
                    config.ReceiveEndpoint("Carol:CampaignOnGoingConsumer", x => x.ConfigureConsumer<CampaignOnGoingConsumer>(context));
                    config.ReceiveEndpoint("Carol:FirebaseUserTokenUpdateConsumer", x => x.ConfigureConsumer<FirebaseUserTokenUpdateConsumer>(context));
                });
            });

            services.AddMassTransitHostedService();
        }

    }
}
