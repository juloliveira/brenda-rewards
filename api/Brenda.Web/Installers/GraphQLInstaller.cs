using Brenda.Web.GraphQL;
using Brenda.Web.GraphQL.Types;
using GraphQL.Types;
using GraphQL.Types.Relay;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Brenda.Web.Installers
{
    public class GraphQLInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(ConnectionType<>));
            services.AddTransient(typeof(EdgeType<>));
            services.AddTransient<PageInfoType>();

            services.AddSingleton<GuidGraphType>();
            services.AddSingleton<CampaignType>();
            services.AddSingleton<ActionType>();
            
            //services.AddSingleton<BrendaQuery>();
            //services.AddSingleton<ISchema, BrendaSchema>();
            services.AddScoped<BrendaQuery>();
            services.AddScoped<ISchema, BrendaSchema>();
        }
    }
}
