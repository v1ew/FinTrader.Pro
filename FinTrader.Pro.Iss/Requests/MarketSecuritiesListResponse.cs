using FinTrader.Pro.Iss.Converters;
using Newtonsoft.Json;

namespace FinTrader.Pro.Iss.Requests
{
    /// <summary>
    /// Ответ на запрос списка инструментов рынка
    /// </summary>
    public class MarketSecuritiesListResponse
    {
        [JsonProperty("securities")]
        public IssResponsePayload Securities { get; set; }
    }
}
