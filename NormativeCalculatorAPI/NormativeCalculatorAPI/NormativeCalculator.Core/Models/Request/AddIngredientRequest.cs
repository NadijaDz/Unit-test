using NormativeCalculator.Common.Enums;

namespace NormativeCalculator.Core.Models.Request
{
    public class AddIngredientRequest
    {
        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal UnitQuantity { get; set; }

        public MeasureUnit MeasureUnit { get; set; }

        public decimal CostIngredient { get; set; }
    }
}
