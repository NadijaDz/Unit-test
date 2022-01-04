using System;
using System.Collections.Generic;

namespace NormativeCalculator.Core.Models.DTOs
{
    public class GetRecipesDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal TotalCost { get; set; }

        public DateTime Created { get; set; }

        public int RecipeCategory_Id { get; set; }

        public IEnumerable<GetRecipesIngredientsDto> RecipeIngredient { get; set; }

        public decimal RecommendedPrice { get; set; }
    }
}
