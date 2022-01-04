using System;

namespace NormativeCalculator.Core.Models.Response
{
    public class GetRecipesResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal TotalCost { get; set; }

        public DateTime Created { get; set; }

        public int RecipeCategoryId { get; set; }
        public decimal RecommendedPrice { get; set; }
    }
}
