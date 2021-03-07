using FinTrader.Pro.Iss.Converters;
using Newtonsoft.Json;

namespace FinTrader.Pro.Iss.Requests
{
    public class BondCouponsResponse
    {
        [JsonProperty("coupons")]
        public IssResponsePayload Coupons { get; set; }
    }
}
