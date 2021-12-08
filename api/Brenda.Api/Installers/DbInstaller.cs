using Brenda.Core;
using Brenda.Core.Interfaces;
using Brenda.Data;
using Brenda.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Brenda.Api.Installers
{

    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //var connection = new MySql.Data.MySqlClient.MySqlConnection(
            //        configuration.GetConnectionString("DefaultConnection")
            //    );

            services.AddDbContext<BrendaContext>(x => 
                x.UseMySql(configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Transient);
            //services.AddTransient<Func<BrendaContext>>(options => () => options.GetService<BrendaContext>());


            services.AddIdentity<BrendaUser, BrendaRole>()
                .AddEntityFrameworkStores<BrendaContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<ICustomers, Customers>();
            services.AddScoped<ICampaigns, Campaigns>();
            services.AddScoped<IUsers, Users>();
            services.AddScoped<IDashboard, Dashboard>();

            services.AddScoped<IGenderIdentities, GenderIdentities>();
            services.AddScoped<IIncomes, Incomes>();

        }
    }
}
