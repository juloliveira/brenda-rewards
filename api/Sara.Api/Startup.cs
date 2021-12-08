using Brenda.Utils;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Sara.Api.Installers;
using Sara.Api.Validators;
using Sara.Core;
using Sara.Data;
using Sara.Infrastructure.Middleware;
using System.Text;

namespace Sara.Api
{
    public class SaraSettings
    {
        public string Secret { get; set; }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServicesInAssembly(Configuration);

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration =
                    Configuration.GetConnectionString("Redis");
                options.InstanceName = "sara";
            });


            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<SaraSettings>(appSettingsSection);

            var emailSenderSection = Configuration.GetSection("EmailSender");
            services.Configure<EmailSenderOptions>(emailSenderSection);

            var appSettings = appSettingsSection.Get<SaraSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddCors();

            services.AddControllers(options =>
            {
                //options.Filters.Add(new HttpResponseExceptionFilter());
            })
            .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<UserRegisterValidator>())
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
            });

            services.AddHttpContextAccessor();
            services.AddRazorPages();

            var firebaseapp = FirebaseAdmin.FirebaseApp.Create();
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            SaraContext context,
            RoleManager<SaraRole> roleManager)
        {
            app.UseExceptionHandler("/error");
            
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<TenantInfoMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });

            //Data.Seeds.SecurityDataInitializer.SeedRoles(roleManager);
            //Data.Seeds.IdentityDataInitializer.SeedData(context);
        }
    }
}
