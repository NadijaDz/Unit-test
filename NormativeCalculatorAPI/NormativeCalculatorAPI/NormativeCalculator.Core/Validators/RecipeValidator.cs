using FluentValidation;
using NormativeCalculator.Core.Models.Request;

namespace NormativeCalculator.Core.Validators
{
    public class RecipeValidator : AbstractValidator<AddRecipeRequest>
    {
        public RecipeValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.RecipeCategoryId).NotEmpty().NotNull();
            RuleFor(x => x.RecommendedPrice).NotEmpty().NotNull()
                .GreaterThan(0).WithMessage("Recommended price must be grather than 0");
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotEmpty().NotNull();
        }
    }
}
