using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NormativeCalculator.Database;
using System;

namespace NormativeCalculator.API.Extensions
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection RegisterIdentityConfiguration(this IServiceCollection services, IConfiguration config)
        {

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<NormativeCalculatorDBContext>()
                    .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(o =>
               o.TokenLifespan = TimeSpan.FromHours(3)
           );

            return services;
        }
    }
}
