using Microsoft.AspNetCore.Identity;
using NormativeCalculator.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NormativeCalculator.Core.Entities
{
    public class Recipe : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [ForeignKey("RecipeCategoryId")]
        public RecipeCategory RecipeCategory { get; set; }
        public int RecipeCategoryId { get; set; }

        public string UserId { get; set; }

        public IdentityUser? User { get; set; }

        public IEnumerable<RecipeIngredient> RecipesIngredients { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal RecommendedPrice { get; set; }
    }
}
