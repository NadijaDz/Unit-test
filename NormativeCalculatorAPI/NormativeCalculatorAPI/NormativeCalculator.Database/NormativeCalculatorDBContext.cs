using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NormativeCalculator.Core.Entities;

namespace NormativeCalculator.Database
{
    public class NormativeCalculatorDBContext : IdentityDbContext
    {
        public NormativeCalculatorDBContext(DbContextOptions<NormativeCalculatorDBContext> options) : base(options)
        {
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeCategory> RecipeCategories { get; set; }
        public DbSet<RecipeIngredient> RecipesIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
            modelBuilder.Entity<RecipeIngredient>().HasKey(sc => new { sc.IngredientId, sc.RecipeId });
            modelBuilder.Entity<Recipe>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        }
    }
}
