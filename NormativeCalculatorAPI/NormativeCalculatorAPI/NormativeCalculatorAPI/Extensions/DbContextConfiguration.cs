using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NormativeCalculator.Database;

namespace NormativeCalculator.API.Extensions
{
    public static class DbContextConfiguration
    {
        public static IServiceCollection RegisterDbContextConfiguration(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<NormativeCalculatorDBContext>(options =>
                    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddDbContext<NormativeCalculatorLoggerDBContext>(options =>
                   options.UseSqlServer(config.GetConnectionString("LoggerConnection")));

            return services;
        }
    }
}
