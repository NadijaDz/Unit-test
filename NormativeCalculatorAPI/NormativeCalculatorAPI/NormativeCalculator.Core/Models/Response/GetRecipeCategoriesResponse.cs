using System;

namespace NormativeCalculator.Core.Models.Response
{
    public class GetRecipeCategoriesResponse
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public DateTime Created { get; set; }
    }
}
