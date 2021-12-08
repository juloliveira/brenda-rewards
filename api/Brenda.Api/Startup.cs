using AutoMapper;
using Brenda.Api.Installers;
using Brenda.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.Text;
using FluentValidation.AspNetCore;
using Brenda.Api.Validators;
using Julia.Client;

namespace Brenda.Api
{
    public class Startup
    {
        protected IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServicesInAssembly(Configuration);
            services.AddAutoMapper(typeof(Startup));

            services.AddCors();

            services.AddControllers()
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<UserRegisterValidator>())
                .AddNewtonsoftJson(options => 
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                });
            
            
            services.AddHttpContextAccessor();

            var emailSenderSection = Configuration.GetSection("EmailSender");
            services.Configure<EmailSenderOptions>(emailSenderSection);

            var endpointSection = Configuration.GetSection("Endpoints");
            services.Configure<EndpointOptions>(endpointSection);

            var teste = endpointSection.Get<EndpointOptions>();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<BrendaSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<BrendaSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);


            var juliaSettings = Configuration.GetSection("JuliaSettings");
            services.Configure<JuliaSettings>(juliaSettings);

            services.AddSingleton<Julia.Client.Campaigns.CampaignsClient>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Root", policy => policy.RequireClaim("", ""));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            FirebaseAdmin.FirebaseApp.Create();
            //services.AddTransient<GlobalExceptionHandlerMiddleware>();

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler("/error-local-development");
            
            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            


            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            
        }
    }

    
}
