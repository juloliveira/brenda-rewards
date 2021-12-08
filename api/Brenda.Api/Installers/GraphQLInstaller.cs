using Brenda.Api.GraphQL;
using Brenda.Api.GraphQL.Types;
using GraphQL.Types;
using GraphQL.Types.Relay;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Brenda.Api.Installers
{
    public class GraphQLInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(ConnectionType<>));
            services.AddTransient(typeof(EdgeType<>));
            services.AddTransient<PageInfoType>();

            services.AddSingleton<GuidGraphType>();
            services.AddSingleton<CampaignsType>();
            services.AddSingleton<BrendaQuery>();
            
            services.AddScoped<BrendaQuery>();
            services.AddScoped<ISchema, BrendaSchema>();
        }
    }
}
