using Microsoft.AspNetCore.Mvc;
using NormativeCalculator.Common.Enums;
using NormativeCalculator.Services;
using System.Threading;
using System.Threading.Tasks;

namespace NormativeCalculator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("GetMostUsedIngredients")]
        public async Task<IActionResult> GetMostUsedIngredients(MeasureUnit measureType, decimal minQuantity,
            decimal maxQuantity, CancellationToken cancellationToken)
        {
            return Ok(await _reportService.GetMostUsedIngredients(measureType, minQuantity, maxQuantity, cancellationToken));
        }

        [HttpGet("GetRecipesOrderByCostRecipeGroupByCategory")]
        public async Task<IActionResult> GetRecipesOrderByCostRecipeGroupByCategory(CancellationToken cancellationToken)
        {
            return Ok(await _reportService.GetRecipesOrderByCostRecipeGroupByCategory(cancellationToken));
        }

        [HttpGet("GetRecipesWithAtLeast10Ingredients")]
        public async Task<IActionResult> GetRecipesWithAtLeast10Ingredients(CancellationToken cancellationToken)
        {
            return Ok(await _reportService.GetRecipesWithAtLeast10Ingredients(cancellationToken));
        }
    }
}
