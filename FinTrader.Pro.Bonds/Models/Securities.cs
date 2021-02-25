using FinTrader.Pro.DB.Models;
using Newtonsoft.Json;

namespace FinTrader.Pro.Bonds.Models
{
    public class Securities
    {
        [JsonProperty(PropertyName = "columns")]
        public string[] Columns { get; set; }

        [JsonProperty(PropertyName = "data")]
        public Bond[] Data { get; set; }
    }
}
