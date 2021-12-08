namespace Brenda.Web.Installers
{
    using System;
    using Brenda.Core;
    using Brenda.Core.Interfaces;
    using Brenda.Data;
    using Brenda.Data.Repository;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BrendaContext>(x =>
                x.UseMySql(configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Transient);
            
            services.AddTransient<Func<BrendaContext>>(options => () => options.GetService<BrendaContext>());

            services.AddIdentity<BrendaUser, BrendaRole>()
                .AddEntityFrameworkStores<BrendaContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IErrorMessages, ErrorMessages>();
        }
    }
}
