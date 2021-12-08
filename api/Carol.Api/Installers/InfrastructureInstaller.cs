using Carol.Api.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Carol.Api.Installers
{
    public class InfrastructureInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<TenantInfo>();
        }
    }
}
