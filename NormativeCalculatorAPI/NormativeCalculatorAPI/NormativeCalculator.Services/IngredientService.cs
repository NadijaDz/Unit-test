using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NormativeCalculator.Common.Enums;
using NormativeCalculator.Core.Entities;
using NormativeCalculator.Core.Models.Request;
using NormativeCalculator.Core.Models.Response;
using NormativeCalculator.Core.Validators;
using NormativeCalculator.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace NormativeCalculator.Services
{
    public interface IIngredientService
    {
        Task<List<GetIngredientsResponse>> GetIngredientsAsync(CancellationToken cancellationToken = default);
        Task<GetIngredientsResponse> InsertIngredientAsync(AddIngredientRequest request, CancellationToken cancellationToken = default);
        Task<DataTableResponse<GetIngredientsResponse>> GetIngredientsForTableAsync(TableRequest request, CancellationToken cancellationToken = default);
        Task<GetIngredientsResponse> UpdateRecipeCategoryAsync(int id, AddIngredientRequest request, CancellationToken cancellationToken);
        Task<GetIngredientsResponse> DeleteIngredientAsync(int id, CancellationToken cancellationToken);
    }

    public class IngredientService : IIngredientService
    {
        private readonly NormativeCalculatorDBContext _context;
        protected readonly IMapper _mapper;

        public IngredientService(NormativeCalculatorDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetIngredientsResponse>> GetIngredientsAsync(CancellationToken cancellationToken)
        {
            var list = await _context.Ingredients.ToListAsync(cancellationToken);
            return _mapper.Map<List<GetIngredientsResponse>>(list);
        }

        public async Task<GetIngredientsResponse> InsertIngredientAsync(AddIngredientRequest request, CancellationToken cancellationToken = default)
        {
            var validator = new IngredientValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new ArgumentException("Ingredient validator is incorrect");

            }
            var entity = _mapper.Map<Ingredient>(request);
            entity.CreatedAt = DateTime.Now;
            entity.IsDeleted = false;
            _context.Ingredients.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<GetIngredientsResponse>(entity);
        }

        public async Task<DataTableResponse<GetIngredientsResponse>> GetIngredientsForTableAsync(TableRequest request, CancellationToken cancellationToken)
        {
            var list = _context.Ingredients.AsQueryable();

            if (request.Order != null && request.Order.Count > 0)
            {
                if (request.Columns != null && request.Columns.Count > 0)
                {
                    var sortColumn = request.Columns[request.Order[0].Column].Data;
                    var orderBy = request.Order[0].Dir;
                    list = list.OrderBy(sortColumn + " " + orderBy);
                }
            }

            foreach (var f in request.Filter)
            {
                if (!string.IsNullOrWhiteSpace(f.Property))
                {
                    if (f.Property.Equals("Name"))
                    {
                        list = list.AsQueryable().Where(x => x.Name.Contains(f.Value));
                    }

                    if (f.Property.Equals("MeasureUnit"))
                    {
                        list = list.AsQueryable().Where(x => x.MeasureUnit.Equals((MeasureUnit)Enum.Parse(typeof(MeasureUnit), f.Value)));
                    }

                    if (f.Property.Equals("UnitQuantityMin"))
                    {
                        list = list.AsQueryable().Where(x => x.UnitQuantity >= Convert.ToDecimal(f.Value));
                    }

                    if (f.Property.Equals("UnitQuantityMax"))
                    {
                        list = list.AsQueryable().Where(x => x.UnitQuantity <= Convert.ToDecimal(f.Value));
                    }
                }
            }

            var ingredients = _mapper.Map<List<GetIngredientsResponse>>(list);

            DataTableResponse<GetIngredientsResponse> getIngredientsResponse = new DataTableResponse<GetIngredientsResponse>();
            getIngredientsResponse.Data = ingredients.Skip(request.Start).Take(request.Length).ToList();
            getIngredientsResponse.recordsTotal = ingredients.Count;
            getIngredientsResponse.recordsFiltered = ingredients.Count;

            return getIngredientsResponse;
        }

        public async Task<GetIngredientsResponse> UpdateRecipeCategoryAsync(int id, AddIngredientRequest request, CancellationToken cancellationToken)
        {
            var validator = new IngredientValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new ArgumentException("Ingredient validator is incorrect");

            }
            var entity = await _context.Ingredients.FindAsync(new object[] { id }, cancellationToken);
            _mapper.Map(request, entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<GetIngredientsResponse>(entity);
        }

        public async Task<GetIngredientsResponse> DeleteIngredientAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _context.Ingredients.FindAsync(new object[] { id }, cancellationToken);
            entity.IsDeleted = true;
            _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<GetIngredientsResponse>(entity);
        }
    }
}
