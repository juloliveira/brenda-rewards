using AutoMapper;
using Brenda.Core;
using Brenda.Data;
using Brenda.Infrastructure.Middleware;
using Brenda.Utils;
using Brenda.Web.Extensions;
using Brenda.Web.Installers;
using Brenda.Web.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Security.Claims;
using System.Text;


namespace Brenda.Web
{
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
            services.AddAutoMapper(typeof(Startup));

            var emailSenderSection = Configuration.GetSection("EmailSender");
            services.Configure<EmailSenderOptions>(emailSenderSection);

            var endpointSection = Configuration.GetSection("Endpoints");
            services.Configure<EndpointOptions>(endpointSection);

            var teste = endpointSection.Get<EndpointOptions>();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<BrendaSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<BrendaSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(x =>
            {
                x.AccessDeniedPath = "/security/access-denied";
                x.LoginPath = "/security/sign-in";
                x.ReturnUrlParameter = "returnUrl";
            });

            services.AddAuthorization(options => 
            {
                options.AddPolicy(Roles.ROOT, authBuilder => { authBuilder.RequireClaim(ClaimTypes.Role, Roles.ROOT); });
                options.AddPolicy(Roles.Administrator, authBuilder => { authBuilder.RequireClaim(ClaimTypes.Role, Roles.Administrator); });
                options.AddPolicy(Roles.Collaborator, authBuilder => { authBuilder.RequireClaim(ClaimTypes.Role, Roles.Collaborator); });
            });

            services.AddControllersWithViews(options =>
            {
                options.ModelBinderProviders.Insert(0, new DateTimeBinderProvider());
            })
            .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<SignUpValidator>())
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };

                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
                options.SerializerSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
                options.SerializerSettings.MetadataPropertyHandling = Newtonsoft.Json.MetadataPropertyHandling.Ignore;
                options.SerializerSettings.Converters.Add(new Extensions.GuidConverter());
            })
            .AddRazorRuntimeCompilation();
            
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env,
            Microsoft.AspNetCore.Identity.UserManager<BrendaUser> userManager,
            Microsoft.AspNetCore.Identity.RoleManager<BrendaRole> roleManager,
            BrendaContext context)
        {
            if (env.IsDevelopment()) { }

            app.UseDeveloperExceptionPage();
            //app.UseExceptionHandler("/error");
            app.UseHsts();

            var cultureInfo = new CultureInfo("pt-BR");
            cultureInfo.NumberFormat.CurrencySymbol = "BRE$";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseBrowserLink();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<TenantInfoMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Backoffice",
                    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //Data.Seed.IdentityDataInitializer.SeedData(userManager, roleManager, context);

        }
    }
}
