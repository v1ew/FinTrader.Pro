using Newtonsoft.Json;
using System.Collections.Generic;

namespace FinTrader.Pro.Iss.Converters
{
    /// <summary>
    /// Часть ответа, данные определённого типа с описанием колонок и метаданными
    /// </summary>
    [JsonConverter(typeof(JsonIssResponseToObjectConverter))]
    public class IssResponsePayload
    {
        /// <summary>
        /// Метаданные
        /// </summary>
        //[JsonProperty("metadata")]
        //public string Metadata { get; set; }

        /// <summary>
        /// Описание колонок
        /// </summary>
        [JsonProperty("columns")]
        public List<string> Columns { get; set; }

        /// <summary>
        /// Данные
        /// </summary>
        [JsonProperty("data")]
        public List<Dictionary<string, string>> Data { get; set; }
    }
}
