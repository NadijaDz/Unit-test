using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NormativeCalculator.Core.Models.Request;
using NormativeCalculator.Services;
using System.Threading;
using System.Threading.Tasks;

namespace NormativeCalculatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipesService;
        private readonly UserManager<IdentityUser> _userManager;

        public RecipesController(IRecipeService recipes, UserManager<IdentityUser> userManager)
        {
            _recipesService = recipes;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(int id, CancellationToken cancellationToken)
        {
            return Ok(await _recipesService.GetRecipeByIdAsync(id, cancellationToken));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetRecipes([FromQuery] RecipeSearchRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _recipesService.GetRecipesAsync(request, cancellationToken));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> InsertRecipe(AddRecipeRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _recipesService.InsertRecipeAsync(request, cancellationToken));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, AddRecipeRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _recipesService.UpdateRecipeAsync(id, request, cancellationToken));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id, CancellationToken cancellationToken)
        {
            return Ok(await _recipesService.DeleteRecipeAsync(id, cancellationToken));
        }
    }
}
