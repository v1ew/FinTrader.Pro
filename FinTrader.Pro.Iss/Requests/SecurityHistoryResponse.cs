using FinTrader.Pro.Iss.Converters;
using Newtonsoft.Json;

namespace FinTrader.Pro.Iss.Requests
{
    public class SecurityHistoryResponse
    {
        [JsonProperty("history")]
        public IssResponsePayload History { get; set; }
    }
}