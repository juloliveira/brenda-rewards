using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sara.Core.Factories;
using Sara.Services.Factories;

namespace Sara.Api.Installers
{
    public class ServicesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserFactory, UserFactory>();
        }
    }
}
