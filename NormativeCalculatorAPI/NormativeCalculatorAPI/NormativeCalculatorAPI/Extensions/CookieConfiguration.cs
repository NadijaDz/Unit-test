using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace NormativeCalculator.API.Extensions
{
    public static class CookieConfiguration
    {
        public static IServiceCollection RegisterCookieConfiguration(this IServiceCollection services, IConfiguration config)
        {

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "auth_cookie";
                options.Cookie.SameSite = SameSiteMode.None;
                options.LoginPath = new PathString("/api/Login/signin-google");
                options.AccessDeniedPath = new PathString("/api/contests");
                options.Cookie.HttpOnly = false;

                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });

            return services;
        }
    }
}
