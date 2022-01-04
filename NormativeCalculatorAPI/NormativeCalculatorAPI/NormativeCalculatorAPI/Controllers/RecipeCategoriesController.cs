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
    public class RecipeCategoriesController : ControllerBase
    {
        private readonly IRecipeCategoryService _recipeCategoriesService;
        public RecipeCategoriesController(IRecipeCategoryService recipeCategories)
        {
            _recipeCategoriesService = recipeCategories;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetRecipeCategories([FromQuery] int skip, CancellationToken cancellationToken)
        {
            return Ok(await _recipeCategoriesService.GetRecipeCategoriesAsync(skip, cancellationToken));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeCategoryById(int id, CancellationToken cancellationToken)
        {
            return Ok(await _recipeCategoriesService.GetRecipeCategoryByIdAsync(id, cancellationToken));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> InsertRecipeCategory(AddRecipeCategoryRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _recipeCategoriesService.InsertRecipeCategoryAsync(request, cancellationToken));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipeCategory(int id, AddRecipeCategoryRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _recipeCategoriesService.UpdateRecipeCategoryAsync(id, request, cancellationToken));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipeCategory(int id, CancellationToken cancellationToken)
        {
            return Ok(await _recipeCategoriesService.DeleteRecipeCategoryAsync(id, cancellationToken));
        }

    }
}
