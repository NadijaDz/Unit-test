namespace NormativeCalculator.Core.Models.Response
{
    public class PaginationResponse<T> where T : class
    {
        public PaginationResponse(T data, int total)
        {
            Data = data;
            TotalCount = total;
        }

        public T Data { get; set; }
        public int TotalCount { get; set; }
    }
}
