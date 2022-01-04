using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace NormativeCalculator.API.Extensions
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection RegisterSwaggerConfiguration(this IServiceCollection services, IConfiguration config)
        {

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NormativeCalculatorAPI", Version = "v1" });
                c.AddSecurityDefinition("Auth with google", new OpenApiSecurityScheme
                {
                    Description = @"Please click this link for authorize  <a href='https://localhost:5001/api/Login/signin-google?provider=Google&returnUrl=https%3A%2F%2Flocalhost%3A5001%2Fswagger%2Findex.html'>Login with Google</a>",
                });

            });

            return services;
        }
    }
}
