using FinTrader.Pro.Iss.Converters;
using Newtonsoft.Json;

namespace FinTrader.Pro.Iss.Requests
{
    public class DatesRangeResponse
    {
        [JsonProperty("dates")]
        public IssResponsePayload Dates { get; set; }
    }
}