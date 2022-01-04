using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NormativeCalculator.Core.Models.Request
{
    public class TableRequest
    {
        [JsonPropertyName("draw")]
        public int Draw { get; set; }

        [JsonPropertyName("columns")]
        public List<Column> Columns { get; set; }

        [JsonPropertyName("order")]
        public List<Order> Order { get; set; }

        [JsonPropertyName("start")]
        public int Start { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonPropertyName("filter")]
        public List<Filter> Filter { get; set; }
    }
}
