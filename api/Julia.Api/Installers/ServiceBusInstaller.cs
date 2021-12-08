using Julia.Api.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Julia.Api.Installers
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

                x.AddConsumer<CampaignOutOfDateConsumer>();
                x.AddConsumer<CampaignAlreadyRewardedConsumer>();
                x.AddConsumer<CampaignRequestConsumer>();
                x.AddConsumer<RewardUserConsumer>();
                x.AddConsumer<CreateUserConsumer>();

#if DEBUG
                x.AddConsumer<DescriptorConsumer>();
#endif

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(host, virtualHost, cfg =>
                    {
                        cfg.Username(username);
                        cfg.Password(password);
                    });

                    config.ReceiveEndpoint("Julia:CampaignOutOfDateConsumer", x => x.ConfigureConsumer<CampaignOutOfDateConsumer>(context));
                    config.ReceiveEndpoint("Julia:CampaignRequestConsumer", x => x.ConfigureConsumer<CampaignRequestConsumer>(context));
                    config.ReceiveEndpoint("Julia:RewardUserConsumer", x => x.ConfigureConsumer<RewardUserConsumer>(context));
                    config.ReceiveEndpoint("Julia:CreateUserConsumer", x => x.ConfigureConsumer<CreateUserConsumer>(context));

                    config.ReceiveEndpoint("Julia:CampaignAlreadyRewardedConsumer", x => x.ConfigureConsumer<CampaignAlreadyRewardedConsumer>(context));

#if DEBUG
                    config.ReceiveEndpoint("Julia:DescriptorConsumer", x => x.ConfigureConsumer<DescriptorConsumer>(context));
#endif

                });
            });

            services.AddMassTransitHostedService();
        }

    }
}
