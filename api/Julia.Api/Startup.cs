using Julia.Api.Graph.Types;
using Julia.Api.Installers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using Julia.Api.Data;
using Julia.Api.Graph;
using HotChocolate.AspNetCore.Playground;

namespace Julia.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServicesInAssembly(Configuration);
            services.AddControllers();

            services.AddGraphQL(
                SchemaBuilder.New()
                    .AddQueryType<QueryType>()
                    .AddType<RequestsType>()
                    .AddType<SexType>());

            services.AddScoped<JuliaContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHsts();
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseGraphQL(new Microsoft.AspNetCore.Http.PathString("/stats"));

            app.UsePlayground(new PlaygroundOptions
            {
                Path = "/playground",
                QueryPath = "/stats"
            });
            app.UseVoyager(new VoyagerOptions
            {
                Path = "/voyager",
                QueryPath = "/stats"
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });
        }
    }
}
