namespace NormativeCalculator.Core.Models.DTOs
{
    public class CalculatedDto
    {
        public decimal UnitPrice { get; set; }

        public decimal UnitQuantity { get; set; }

        public decimal CostIngredient { get; set; }

        public decimal CostPerIngredient { get; set; }
    }
}
