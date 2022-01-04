using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NormativeCalculator.Core.Models.Request;
using NormativeCalculator.Services;
using System.Threading;
using System.Threading.Tasks;

namespace NormativeCalculatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientService _ingredientsService;
        public IngredientsController(IIngredientService ingredients)
        {
            _ingredientsService = ingredients;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetIngredients(CancellationToken cancellationToken)
        {
            return Ok(await _ingredientsService.GetIngredientsAsync(cancellationToken));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> InsertIngredient(AddIngredientRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _ingredientsService.InsertIngredientAsync(request, cancellationToken));
        }

        [Authorize]
        [HttpPost("GetIngredientsForTable")]
        public async Task<IActionResult> GetIngredientsForTable(TableRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _ingredientsService.GetIngredientsForTableAsync(request, cancellationToken));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, AddIngredientRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _ingredientsService.UpdateRecipeCategoryAsync(id, request, cancellationToken));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteingredient(int id, CancellationToken cancellationToken)
        {
            return Ok(await _ingredientsService.DeleteIngredientAsync(id, cancellationToken));
        }

    }
}
