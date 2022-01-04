using NormativeCalculator.Common.Enums;
using NormativeCalculator.Core.Entities;
using NormativeCalculator.Core.Models.DTOs;
using System;
using System.Linq;

namespace NormativeCalculator.Core.Helper
{
    public static class CalculatedPrice
    {
        public static decimal CalculatedQuantityOnMinUnit(decimal quantity, MeasureUnit measureUnit)
        {
            if (measureUnit == MeasureUnit.kg || measureUnit == MeasureUnit.L)
            {
                return quantity * 1000;
            }
            else
            {
                return quantity;
            }
        }

        public static decimal CalculatedTotalPrice(Recipe request)
        {

            decimal totalCost = 0;

            var costPerIngredient = request.RecipesIngredients
                .Select(r => new CalculatedDto
                {
                    CostPerIngredient = CalculatedIngredientPrice(r.UnitQuantity, r.MeasureUnit, r.Ingredient.UnitPrice)
                }); ;

            totalCost = costPerIngredient.Sum(x => x.CostPerIngredient);

            return Math.Round(totalCost, 2);

        }

        public static decimal CalculatedIngredientPrice(decimal quantity, MeasureUnit unit, decimal unitPrice)
        {
            return Math.Round(unitPrice * (CalculatedQuantityOnMinUnit(quantity, unit)), 2);
        }
    }
}
