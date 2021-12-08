using Carol.Core.Services;
using Carol.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Carol.Api.Installers
{
    public class ServicesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<IRewardService, RewardService>();
        }
    }
}
