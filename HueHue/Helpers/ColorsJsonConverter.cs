using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace HueHue.Helpers
{
    public class ColorsJsonConverter : JsonConverter
    {
        private readonly Type[] _types;

        public ColorsJsonConverter(params Type[] types)
        {
            _types = types;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(LEDBulb));
        }

        /// <summary>
        /// Deserializes a JSON object back to a LEDBulb object
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            Byte.TryParse(jo["A"].Value<string>(), out byte a);
            Byte.TryParse(jo["R"].Value<string>(), out byte r);
            Byte.TryParse(jo["G"].Value<string>(), out byte g);
            Byte.TryParse(jo["B"].Value<string>(), out byte b);

            return new LEDBulb(r, g, b);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); //Isn't needed
        }
    }
}
