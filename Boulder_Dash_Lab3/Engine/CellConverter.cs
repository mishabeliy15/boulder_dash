using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Boulder_Dash_Lab3
{
    public class CellConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Cell));
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            switch ((Symbol)jo["Icon"]?.Value<char>())
            {
                case Symbol.Empty:
                    return jo.ToObject<Empty>(serializer);
                case Symbol.Rock:
                    return jo.ToObject<Rock>(serializer);
                case Symbol.Sand:
                    return jo.ToObject<Sand>(serializer);
                case Symbol.Player:
                    return jo.ToObject<Player>(serializer);
                case Symbol.Diamond:
                    return jo.ToObject<Diamond>(serializer);
            }
            return null;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}