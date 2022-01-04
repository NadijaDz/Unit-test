using Microsoft.AspNetCore.Identity;
using NormativeCalculator.Core.Models.DTOs;
using System;
using System.Collections.Generic;

namespace NormativeCalculator.Core.Models.Request
{
    public class AddRecipeRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public int RecipeCategoryId { get; set; }

        public IdentityUser User { get; set; }

        public string UserId { get; set; }

        public List<AddRecipeIngredientsDto> Ingredients { get; set; }

        public decimal RecommendedPrice { get; set; }
    }
}
