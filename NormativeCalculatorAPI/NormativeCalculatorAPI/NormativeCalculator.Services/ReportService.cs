using Dapper;
using Microsoft.EntityFrameworkCore;
using NormativeCalculator.Common.Enums;
using NormativeCalculator.Core.Models.Response;
using NormativeCalculator.Database;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace NormativeCalculator.Services
{
    public interface IReportService
    {
        Task<IEnumerable<GetMostUsedIngredientsResponse>> GetMostUsedIngredients(MeasureUnit measureType, decimal minQuantity,
             decimal maxQuantity, CancellationToken cancellationToken);
        Task<IEnumerable<GetRecipesOrderByCostRecipeGroupByCategoryResponse>> GetRecipesOrderByCostRecipeGroupByCategory(CancellationToken cancellationToken);
        Task<IEnumerable<GetRecipesWithAtLeast10IngredientsResponse>> GetRecipesWithAtLeast10Ingredients(CancellationToken cancellationToken);
    }

    public class ReportService : IReportService
    {
        private readonly NormativeCalculatorDBContext _context;

        private readonly DbConnection _dbConnection;

        public ReportService(NormativeCalculatorDBContext context)
        {
            _context = context;
            _dbConnection = _context.Database.GetDbConnection();

        }

        public async Task<IEnumerable<GetMostUsedIngredientsResponse>> GetMostUsedIngredients(MeasureUnit measureType, decimal minQuantity,
            decimal maxQuantity, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@measureType", measureType);
            parameters.Add("@minQuantity", minQuantity);
            parameters.Add("@maxQuantity", maxQuantity);

            return await _dbConnection.QueryAsync<GetMostUsedIngredientsResponse>("dbo.GetMostUsedIngredients", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<GetRecipesOrderByCostRecipeGroupByCategoryResponse>> GetRecipesOrderByCostRecipeGroupByCategory(CancellationToken cancellationToken)
        {
            return await _dbConnection.QueryAsync<GetRecipesOrderByCostRecipeGroupByCategoryResponse>("dbo.GetRecipesOrderByCostRecipeGroupByCategory", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<GetRecipesWithAtLeast10IngredientsResponse>> GetRecipesWithAtLeast10Ingredients(CancellationToken cancellationToken)
        {
            return await _dbConnection.QueryAsync<GetRecipesWithAtLeast10IngredientsResponse>("dbo.GetRecipesWithAtLeast10Ingredients", commandType: CommandType.StoredProcedure);
        }
    }
}
