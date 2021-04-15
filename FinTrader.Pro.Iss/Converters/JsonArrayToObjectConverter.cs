using Newtonsoft.Json;
using System;

namespace FinTrader.Pro.Iss.Converters
{
    public class JsonArrayToObjectConverter<T> : JsonConverter where T : PayloadDataBase, new()
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var jArray = Newtonsoft.Json.Linq.JArray.Load(reader);
            var r = new T();
            r.ReadFromJArray(jArray);

            return r;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
