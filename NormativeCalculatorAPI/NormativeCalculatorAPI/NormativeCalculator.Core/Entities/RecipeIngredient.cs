using NormativeCalculator.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace NormativeCalculator.Core.Entities
{
    public class RecipeIngredient
    {
        [ForeignKey("RecipeId")]
        public Recipe Recipe { get; set; }
        public int RecipeId { get; set; }

        [ForeignKey("IngredientId")]
        public Ingredient Ingredient { get; set; }
        public int IngredientId { get; set; }

        public MeasureUnit MeasureUnit { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal UnitQuantity { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal IngredientPrice { get; set; }

    }
}
