using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NormativeCalculator.Mapper.Mapping;

namespace NormativeCalculator.API.Extensions
{
    public static class AutoMapperConfiguration
    {
        public static IServiceCollection RegisterAutoMapperConfiguration(this IServiceCollection services, IConfiguration config)
        {

            services.AddAutoMapper(typeof(NormativeCalculatorProfile).Assembly);

            return services;
        }
    }
}
