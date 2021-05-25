using FinTrader.Pro.Iss.Converters;
using Newtonsoft.Json;

namespace FinTrader.Pro.Iss.Requests
{
    public class BondsDurationsResponse
    {
        [JsonProperty("durations")]
        public IssResponsePayload Coupons { get; set; }
    }
}