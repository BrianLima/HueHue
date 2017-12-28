using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RGB.NET.Core;
using System;

namespace HueHue.Helpers
{
    public class ColorsJsonConverter: JsonConverter
    {
        private readonly Type[] _types;

        public ColorsJsonConverter(params Type[] types)
        {
            _types = types;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Color));
        }

        /// <summary>
        /// Deserializes a JSON object back to a RGB.NET.Core.Color
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            return new Color(jo["A"].Value<byte>(), jo["R"].Value<byte>(), jo["G"].Value<byte>(), jo["B"].Value<byte>());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
}
