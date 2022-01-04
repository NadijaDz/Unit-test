using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NormativeCalculator.Services;

namespace NormativeCalculator.API.Extensions
{
    public static class DIConfiguration
    {
        public static IServiceCollection RegisterDIConfiguration(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IRecipeCategoryService, RecipeCategoryService>();
            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
