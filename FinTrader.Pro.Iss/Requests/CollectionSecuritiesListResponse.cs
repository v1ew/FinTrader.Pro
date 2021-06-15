using FinTrader.Pro.Iss.Converters;
using Newtonsoft.Json;

namespace FinTrader.Pro.Iss.Requests
{
    public class CollectionSecuritiesListResponse
    {
        [JsonProperty("securities")]
        public IssResponsePayload Securities { get; set; }
    }
}