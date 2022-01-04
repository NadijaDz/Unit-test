namespace NormativeCalculator.Core.Models.Response
{
    public class GetRecipesWithAtLeast10IngredientsResponse
    {
        public string Name { get; set; }

        public int NumberOfIngredients { get; set; }

        public decimal TotalCost { get; set; }
    }
}
