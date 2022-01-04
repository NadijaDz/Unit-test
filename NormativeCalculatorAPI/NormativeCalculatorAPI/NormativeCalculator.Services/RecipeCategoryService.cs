using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NormativeCalculator.Core.Entities;
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
    public interface IRecipeCategoryService
    {
        Task<PaginationResponse<List<GetRecipeCategoriesResponse>>> GetRecipeCategoriesAsync(int skip, CancellationToken cancellationToken);
        Task<GetRecipeCategoriesResponse> InsertRecipeCategoryAsync(AddRecipeCategoryRequest request, CancellationToken cancellationToken);
        Task<GetRecipeCategoriesResponse> UpdateRecipeCategoryAsync(int id, AddRecipeCategoryRequest request, CancellationToken cancellationToken);
        Task<GetRecipeCategoriesResponse> DeleteRecipeCategoryAsync(int id, CancellationToken cancellationToken);
        Task<GetRecipeCategoriesResponse> GetRecipeCategoryByIdAsync(int id, CancellationToken cancellationToken);
    }

    public class RecipeCategoryService : IRecipeCategoryService
    {
        private readonly NormativeCalculatorDBContext _context;
        protected readonly IMapper _mapper;

        public RecipeCategoryService(NormativeCalculatorDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<List<GetRecipeCategoriesResponse>>> GetRecipeCategoriesAsync(int skip, CancellationToken cancellationToken)
        {
            var list = await _context.RecipeCategories
                .Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Take(10).ToListAsync(cancellationToken);

            var countAllRecipeCategories = _context.RecipeCategories.Count();

            var data = _mapper.Map<List<GetRecipeCategoriesResponse>>(list);

            return new PaginationResponse<List<GetRecipeCategoriesResponse>>(data, countAllRecipeCategories);
        }
        public async Task<GetRecipeCategoriesResponse> GetRecipeCategoryByIdAsync(int id, CancellationToken cancellationToken)
        {

            var entity = await _context.RecipeCategories.FindAsync(new object[] { id }, cancellationToken);

            return _mapper.Map<GetRecipeCategoriesResponse>(entity);
        }

        public async Task<GetRecipeCategoriesResponse> InsertRecipeCategoryAsync(AddRecipeCategoryRequest request, CancellationToken cancellationToken = default)
        {
            var validator = new RecipeCategoryValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new ArgumentException("Recipe category validator is incorrect");

            }

            var entity = _mapper.Map<RecipeCategory>(request);
            entity.CreatedAt = DateTime.Now;
            entity.IsDeleted = false;
            _context.RecipeCategories.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<GetRecipeCategoriesResponse>(entity);
        }

        public async Task<GetRecipeCategoriesResponse> UpdateRecipeCategoryAsync(int id, AddRecipeCategoryRequest request, CancellationToken cancellationToken)
        {
            var validator = new RecipeCategoryValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new ArgumentException("Recipe category validator is incorrect");

            }
            var entity = await _context.RecipeCategories.FindAsync(new object[] { id }, cancellationToken);
            _mapper.Map(request, entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<GetRecipeCategoriesResponse>(entity);
        }

        public async Task<GetRecipeCategoriesResponse> DeleteRecipeCategoryAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _context.RecipeCategories.FindAsync(new object[] { id }, cancellationToken);
            entity.IsDeleted = true;
            _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<GetRecipeCategoriesResponse>(entity);
        }
    }
}
