using NormativeCalculator.Common.Enums;

namespace NormativeCalculator.Core.Models.DTOs
{
    public class GetRecipesIngredientsDto
    {
        public int RecipeId { get; set; }

        public int IngredientId { get; set; }

        public decimal UnitQuantity { get; set; }

        public string IngredinentName { get; set; }

        public MeasureUnit MeasureUnits { get; set; }

        public decimal IngredientPrice { get; set; }
    }
}
