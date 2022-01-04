using System.Text.Json.Serialization;

namespace NormativeCalculator.Core.Models.Request
{
    public class Order
    {
        [JsonPropertyName("column")]
        public int Column { get; set; }

        [JsonPropertyName("dir")]
        public string Dir { get; set; }
    }
}
