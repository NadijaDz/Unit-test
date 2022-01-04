using NormativeCalculator.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace NormativeCalculator.Core.Models.DTOs
{
    public class AddRecipeIngredientsDto
    {
        public int RecipeId { get; set; }

        public int IngredientId { get; set; }

        public MeasureUnit MeasureUnit { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal UnitQuantity { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal IngredientPrice { get; set; }

    }
}
