using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NormativeCalculator.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace NormativeCalculator.Database
{
    public static class DataSeed
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            List<RecipeCategory> recipeCategories = new List<RecipeCategory>()
            {
                new RecipeCategory{ Id=1, CategoryName="Breakfast", CreatedAt=new DateTime(new Random().Next(2021, 2021), new Random().Next(1, 12), new Random().Next(1, 28),0,0,0), IsDeleted=false },
                new RecipeCategory{ Id=2, CategoryName="Lunch", CreatedAt=new DateTime(new Random().Next(2021, 2021), new Random().Next(1, 12), new Random().Next(1, 28),0,0,0), IsDeleted=false },
                new RecipeCategory{ Id=3, CategoryName="Dinner", CreatedAt=new DateTime(new Random().Next(2021, 2021), new Random().Next(1, 12), new Random().Next(1, 28),0,0,0), IsDeleted=false },
                new RecipeCategory{ Id=4, CategoryName="Snacks", CreatedAt=new DateTime(new Random().Next(2021, 2021), new Random().Next(1, 12), new Random().Next(1, 28),0,0,0), IsDeleted=false },
                new RecipeCategory{ Id=5, CategoryName="Salads", CreatedAt=new DateTime(new Random().Next(2021, 2021), new Random().Next(1, 12), new Random().Next(1, 28),0,0,0), IsDeleted=false },
                new RecipeCategory{ Id=6, CategoryName="Rice", CreatedAt=new DateTime(new Random().Next(2021, 2021), new Random().Next(1, 12), new Random().Next(1, 28),0,0,0), IsDeleted=false },
                new RecipeCategory{ Id=7, CategoryName="Pasta", CreatedAt=new DateTime(new Random().Next(2021, 2021), new Random().Next(1, 12), new Random().Next(1, 28),0,0,0), IsDeleted=false },
                new RecipeCategory{ Id=8, CategoryName="Chicken", CreatedAt=new DateTime(new Random().Next(2021, 2021), new Random().Next(1, 12), new Random().Next(1, 28),0,0,0), IsDeleted=false },
                new RecipeCategory{ Id=9, CategoryName="Vegetarian", CreatedAt=new DateTime(new Random().Next(2021, 2021), new Random().Next(1, 12), new Random().Next(1, 28),0,0,0), IsDeleted=false },
                new RecipeCategory{ Id=10, CategoryName="Meat", CreatedAt=new DateTime(new Random().Next(2021, 2021), new Random().Next(1, 12), new Random().Next(1, 28),0,0,0), IsDeleted=false },
                new RecipeCategory{ Id=11, CategoryName="Seafood", CreatedAt=new DateTime(new Random().Next(2021, 2021), new Random().Next(1, 12), new Random().Next(1, 28),0,0,0), IsDeleted=false },
                new RecipeCategory{ Id=12, CategoryName="Desserts", CreatedAt=new DateTime(new Random().Next(2021, 2021), new Random().Next(1, 12), new Random().Next(1, 28),0,0,0), IsDeleted=false },
                new RecipeCategory{ Id=13, CategoryName="Drinks", CreatedAt=new DateTime(new Random().Next(2021, 2021), new Random().Next(1, 12), new Random().Next(1, 28),0,0,0), IsDeleted=false },
                new RecipeCategory{ Id=14, CategoryName="Burgers", CreatedAt=new DateTime(new Random().Next(2021, 2021), new Random().Next(1, 12), new Random().Next(1, 28),0,0,0), IsDeleted=false },
            };
            modelBuilder.Entity<RecipeCategory>().HasData(recipeCategories);

            var jsonIngredients = Path.Combine(Directory.GetCurrentDirectory(), "dataSeedIngredients.json");
            StreamReader r = new StreamReader(jsonIngredients);
            string jsonStringIngredients = r.ReadToEnd();
            var ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(jsonStringIngredients);

            foreach (var m in ingredients)
            {

                Ingredient ingredient = new Ingredient
                {
                    Id = m.Id,
                    Name = m.Name,
                    UnitPrice = m.UnitPrice,
                    UnitQuantity = m.UnitQuantity,
                    MeasureUnit = m.MeasureUnit,
                    CostIngredient = m.CostIngredient,
                    CreatedAt = m.CreatedAt,
                    IsDeleted = m.IsDeleted
                };
                modelBuilder.Entity<Ingredient>().HasData(ingredient);

            }

            var jsonRecipes = Path.Combine(Directory.GetCurrentDirectory(), "dataSeedRecipes.json");
            StreamReader r2 = new StreamReader(jsonRecipes);
            string jsonStringRecipes = r2.ReadToEnd();
            var recipes = JsonConvert.DeserializeObject<List<Recipe>>(jsonStringRecipes);


            foreach (var m in recipes)
            {
                Recipe recipe = new Recipe
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    RecipeCategoryId = m.RecipeCategoryId,
                    CreatedAt = m.CreatedAt,
                    IsDeleted = m.IsDeleted,
                };
                modelBuilder.Entity<Recipe>().HasData(recipe);
            }

            var jsonRecipesIngredients = Path.Combine(Directory.GetCurrentDirectory(), "dataSeedRecipesIngredients.json");
            StreamReader r3 = new StreamReader(jsonRecipesIngredients);
            string jsonStringRecipesIngredients = r3.ReadToEnd();
            var recipesIngredients = JsonConvert.DeserializeObject<List<RecipeIngredient>>(jsonStringRecipesIngredients);

            for (int i = 0; i < recipesIngredients.Count; i++)
            {
                RecipeIngredient recipesIngredient = new RecipeIngredient()
                {
                    RecipeId = recipesIngredients[i].RecipeId,
                    IngredientId = recipesIngredients[i].IngredientId,
                    MeasureUnit = recipesIngredients[i].MeasureUnit,
                    UnitQuantity = recipesIngredients[i].UnitQuantity,

                };
                modelBuilder.Entity<RecipeIngredient>().HasData(recipesIngredient);
            }
        }
    }
}
