using FinTrader.Pro.Iss.Converters;
using Newtonsoft.Json;

namespace FinTrader.Pro.Iss.Requests
{
    public class SecurityDefinitionResponse
    {
        [JsonProperty("description")]
        public IssResponsePayload Description { get; set; }

        [JsonProperty("boards")]
        public IssResponsePayload Boards { get; set; }    }
}