using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NormativeCalculator.Core.Entities;
using NormativeCalculator.Core.Helper;
using NormativeCalculator.Core.Models.DTOs;
using NormativeCalculator.Core.Models.Request;
using NormativeCalculator.Core.Models.Response;
using NormativeCalculator.Core.Validators;
using NormativeCalculator.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NormativeCalculator.Services
{
    public interface IRecipeService
    {
        Task<GetRecipesDto> GetRecipeByIdAsync(int id, CancellationToken cancellationToken);
        Task<PaginationResponse<List<GetRecipesResponse>>> GetRecipesAsync(RecipeSearchRequest request, CancellationToken cancellationToken = default);
        Task<GetRecipesResponse> InsertRecipeAsync(AddRecipeRequest request, CancellationToken cancellationToken = default);
        Task<GetRecipesResponse> UpdateRecipeAsync(int id, AddRecipeRequest request, CancellationToken cancellationToken);
        Task<GetRecipesResponse> DeleteRecipeAsync(int id, CancellationToken cancellationToken);
    }

    public class RecipeService : IRecipeService
    {
        private readonly NormativeCalculatorDBContext _context;
        protected readonly IMapper _mapper;
        private readonly IUserService _userService;

        public RecipeService(NormativeCalculatorDBContext context, IMapper mapper, IUserService userService
            )
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<GetRecipesDto> GetRecipeByIdAsync(int id, CancellationToken cancellationToken)
        {
            var recipe = await _context.Recipes.Where(x => x.Id == id && x.IsDeleted == false).Include(i => i.RecipesIngredients)
                .Select(s => new GetRecipesDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    RecommendedPrice = s.RecommendedPrice,
                    RecipeIngredient = s.RecipesIngredients.Select(c =>
                          new GetRecipesIngredientsDto
                          {
                              IngredientId = c.IngredientId,
                              RecipeId = c.RecipeId,
                              UnitQuantity = c.UnitQuantity,
                              IngredinentName = c.Ingredient.Name,
                              MeasureUnits = c.MeasureUnit,
                              IngredientPrice = c.IngredientPrice
                          }
                        )

                }).FirstOrDefaultAsync();

            recipe.TotalCost = recipe.RecipeIngredient.Sum(x => x.IngredientPrice);

            return recipe;
        }

        public async Task<PaginationResponse<List<GetRecipesResponse>>> GetRecipesAsync(RecipeSearchRequest request, CancellationToken cancellationToken = default)
        {
            var list = await _context.Recipes.Include(r => r.RecipesIngredients).ThenInclude(i => i.Ingredient)
               .Where(x => x.RecipeCategoryId == request.CategoryId && x.IsDeleted == false)
               .Where(s => (string.IsNullOrWhiteSpace(request.SearchQuery)) ||
                   s.Name.ToLower().Trim().StartsWith(request.SearchQuery.ToLower().Trim()) ||
                   s.Description.ToLower().Trim().StartsWith(request.SearchQuery.ToLower().Trim()) ||
                   s.RecipesIngredients.Any(y => y.Ingredient.Name.ToLower().Contains(request.SearchQuery)))
               .Select(s => new GetRecipesDto
               {
                   Id = s.Id,
                   Name = s.Name,
                   Description = s.Description,
                   TotalCost = CalculatedPrice.CalculatedTotalPrice(s)
               }).ToListAsync(cancellationToken);

            list = list.OrderBy(x => x.TotalCost).ThenByDescending(c => c.Created).Skip(request.Skip)
               .Take(10)
               .ToList();

            var countAllRecipes = _context.Recipes.Count();
            var data = _mapper.Map<List<GetRecipesResponse>>(list);

            return new PaginationResponse<List<GetRecipesResponse>>(data, countAllRecipes);
        }

        public async Task<GetRecipesResponse> InsertRecipeAsync(AddRecipeRequest request, CancellationToken cancellationToken = default)
        {
            var validator = new RecipeValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new ArgumentException("Recipe validator is incorrect");
            }

            if (request.RecipeCategoryId == 0)
            {
                return null;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                if (request.User == null)
                {
                    var user = await _userService.GetLoggedInUser();
                    request.User = user;
                    request.UserId = user.Id;
                }

                request.Ingredients = request.Ingredients.GroupBy(x => x.IngredientId)
                    .Select(x => x.First()).ToList();

                var ingredientId = request.Ingredients.Select(x => x.IngredientId).ToArray();
                var ingredientFromRequest = await _context.Ingredients.Where(x => ingredientId.Contains(x.Id)).ToListAsync();

                var entity = _mapper.Map<Recipe>(request);
                entity.CreatedAt = DateTime.Now;
                entity.IsDeleted = false;
                await _context.Recipes.AddAsync(entity);
                await _context.SaveChangesAsync(cancellationToken);

                if (request.Ingredients != null)
                {
                    foreach (var ingredient in request.Ingredients)
                    {
                        var currentIngredient = ingredientFromRequest.FirstOrDefault(x => x.Id == ingredient.IngredientId);
                        await _context.RecipesIngredients.AddAsync(
                             new RecipeIngredient()
                             {
                                 RecipeId = entity.Id,
                                 IngredientId = ingredient.IngredientId,
                                 MeasureUnit = ingredient.MeasureUnit,
                                 UnitQuantity = ingredient.UnitQuantity,
                                 IngredientPrice = CalculatedPrice.CalculatedIngredientPrice(ingredient.UnitQuantity, ingredient.MeasureUnit, currentIngredient.UnitPrice)
                             }
                         );
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();
                return _mapper.Map<GetRecipesResponse>(entity);
            }

            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<GetRecipesResponse> UpdateRecipeAsync(int id, AddRecipeRequest request, CancellationToken cancellationToken = default)
        {
            if (id == 0)
            {
                return null;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await _context.Recipes.FindAsync(new object[] { id }, cancellationToken);
                _mapper.Map(request, entity);

                var recipeIngredientsRemove = await _context.RecipesIngredients.Where(x => x.RecipeId == id).ToListAsync();

                foreach (var recipeIngredient in recipeIngredientsRemove)
                {
                    _context.RecipesIngredients.Remove(recipeIngredient);
                }
                await _context.SaveChangesAsync(cancellationToken);

                request.Ingredients = request.Ingredients.GroupBy(x => x.IngredientId)
                 .Select(x => x.First()).ToList();

                var ingredientId = request.Ingredients.Select(x => x.IngredientId).ToArray();
                var ingredientFromRequest = await _context.Ingredients.Where(x => ingredientId.Contains(x.Id)).ToListAsync();

                if (request.Ingredients != null)
                {
                    foreach (var ingredient in request.Ingredients)
                    {
                        var currentIngredient = ingredientFromRequest.FirstOrDefault(x => x.Id == ingredient.IngredientId);
                        await _context.RecipesIngredients.AddAsync(
                             new RecipeIngredient()
                             {
                                 RecipeId = entity.Id,
                                 IngredientId = ingredient.IngredientId,
                                 MeasureUnit = ingredient.MeasureUnit,
                                 UnitQuantity = ingredient.UnitQuantity,
                                 IngredientPrice = CalculatedPrice.CalculatedIngredientPrice(ingredient.UnitQuantity, ingredient.MeasureUnit, currentIngredient.UnitPrice)
                             }
                         );
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();
                return _mapper.Map<GetRecipesResponse>(entity);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<GetRecipesResponse> DeleteRecipeAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _context.Recipes.FindAsync(new object[] { id }, cancellationToken);
            entity.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<GetRecipesResponse>(entity);
        }
    }
}
