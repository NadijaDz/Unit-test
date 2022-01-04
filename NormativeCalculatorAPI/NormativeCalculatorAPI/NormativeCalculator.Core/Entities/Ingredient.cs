using NormativeCalculator.Common.Enums;
using NormativeCalculator.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NormativeCalculator.Core.Entities
{
    public class Ingredient : BaseEntity
    {
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal UnitQuantity { get; set; }

        public MeasureUnit MeasureUnit { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal CostIngredient { get; set; }

        public IEnumerable<RecipeIngredient> RecipesIngredients { get; set; }
    }
}
