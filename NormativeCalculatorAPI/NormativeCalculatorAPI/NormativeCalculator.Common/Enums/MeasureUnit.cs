using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NormativeCalculator.Common.Enums
{
   [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MeasureUnit
    {
        kg=1,
        gr=2,
        L=3,
        ml=4,
        kom=5
    }
}
