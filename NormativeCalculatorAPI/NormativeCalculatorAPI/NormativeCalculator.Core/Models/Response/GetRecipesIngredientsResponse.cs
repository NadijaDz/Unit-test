namespace NormativeCalculator.Core.Models.Response
{
    public class GetRecipesIngredientsResponse
    {
        public int RecipeId { get; set; }

        public int IngredientId { get; set; }

        public decimal UnitQuantity { get; set; }

        public string IngredinentName { get; set; }

        public string MeasureUnitName { get; set; }

        public decimal CostOfIngredient { get; set; }
    }
}
