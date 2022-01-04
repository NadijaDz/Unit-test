using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace NormativeCalculator.API.Extensions
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection RegisterAuthenticationConfiguration(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddGoogle(options =>
                 {
                     IConfigurationSection googleAuthNSection =
                        config.GetSection("Authentication:Google");

                     options.ClientId = googleAuthNSection["ClientId"];
                     options.ClientSecret = googleAuthNSection["ClientSecret"];
                     options.Events = new OAuthEvents
                     {
                         OnRemoteFailure = (RemoteFailureContext context) =>
                         {
                             context.Response.Redirect("/home/denied");
                             context.HandleResponse();
                             return Task.CompletedTask;
                         }
                     };
                 });

            return services;
        }
    }
}
