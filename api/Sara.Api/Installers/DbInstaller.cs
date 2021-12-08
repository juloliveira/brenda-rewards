using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sara.Core;
using Sara.Core.Interfaces;
using Sara.Data;
using Sara.Data.Repositories;
using System;

namespace Sara.Api.Installers
{

    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SaraContext>(x =>
                x.UseMySql(configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Transient);

            services.AddTransient<Func<SaraContext>>(options => () => options.GetService<SaraContext>());

            services.AddIdentity<SaraUser, SaraRole>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<SaraContext>()
            .AddDefaultTokenProviders();


            services.AddScoped<IIncomes, Incomes>();
            services.AddScoped<ISexualities, Sexualities>();
            services.AddScoped<IEducationLevels, EducationLevels>();
            services.AddScoped<IGenders, Genders>();
        }
    }
}
