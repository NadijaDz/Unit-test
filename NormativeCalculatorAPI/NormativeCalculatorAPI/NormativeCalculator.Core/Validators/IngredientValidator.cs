using FluentValidation;
using NormativeCalculator.Core.Models.Request;

namespace NormativeCalculator.Core.Validators
{
    public class IngredientValidator : AbstractValidator<AddIngredientRequest>
    {
        public IngredientValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.UnitPrice).NotEmpty().NotNull()
                .GreaterThan(0).WithMessage("UnitPrice must be grather than 0");
            RuleFor(x => x.UnitQuantity).NotEmpty().NotNull()
                .GreaterThan(0).WithMessage("UnitQuantity must be grather than 0");
            RuleFor(x => x.MeasureUnit).NotEmpty().NotNull();
            RuleFor(x => x.CostIngredient).NotEmpty().NotNull()
                 .GreaterThan(0).WithMessage("CostIngredient must be grather than 0");
        }
    }
}
