using Carol.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Carol.Api.Installers
{

    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CarolContext>(x =>
                x.UseMySql(configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Transient);

            services.AddTransient<Func<CarolContext>>(options => () => options.GetService<CarolContext>());
        }
    }
}
