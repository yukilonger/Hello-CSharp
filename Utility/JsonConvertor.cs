using System;
using Newtonsoft.Json;

namespace Utility
{
    public class EnumJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return existingValue.ToString();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string type = value.GetType().ToString();

            switch (type)
            {
                case "System.DateTime":
                    writer.WriteValue(Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss"));
                    break;
                default:
                    writer.WriteValue(value.ToString());
                    break;
            }
        }
    }
}

