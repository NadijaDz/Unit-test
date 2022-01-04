using FluentValidation;
using NormativeCalculator.Core.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NormativeCalculator.Core.Validators
{
    public class RecipeCategoryValidator : AbstractValidator<AddRecipeCategoryRequest>
    {
        public RecipeCategoryValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().NotNull();
        }
    }
}
