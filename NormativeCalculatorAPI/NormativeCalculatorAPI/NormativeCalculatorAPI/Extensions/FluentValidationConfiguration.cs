using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NormativeCalculator.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NormativeCalculator.API.Extensions
{
    public static class FluentValidationConfiguration
    {
        public static IServiceCollection RegisterFluentValidationConfiguration(this IServiceCollection services, IConfiguration config)
        {

            services.AddControllersWithViews().AddFluentValidation(options =>
            {
                options.DisableDataAnnotationsValidation = true;
                options.RegisterValidatorsFromAssemblyContaining<RecipeValidator>();
                options.RegisterValidatorsFromAssemblyContaining<IngredientValidator>();
                options.RegisterValidatorsFromAssemblyContaining<RecipeCategoryValidator>();
            });
           
            return services;
        }
    }
}
