using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;

namespace HueHue.Helpers
{
    /// <summary>
    /// Generic class representing a device supported by HueHue
    /// </summary>
    [JsonConverter(typeof(DeviceConverter))]
    public abstract class Device
    {
        private string _type;
        /// <summary>
        /// Type of the device, ie: Arduino, Aura Device, RazerChroma Device, CUE device
        /// </summary>
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _name;
        /// <summary>
        /// Name the user has given to the device
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _icon;
        /// <summary>
        /// Path of the icon that represents the Device
        /// </summary>
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        /// <summary>
        /// The Device MUST implement a Start procedure which begins communication
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// The Device MUST implement a Stop procedure which stops communication 
        /// </summary>
        public abstract void Stop();
    }

    public class BaseSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(Device).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }

    /// <summary>
    /// Converts json objects to types derived from <Device>
    /// </summary>
    public class DeviceConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Device));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //TODO: Implement Json converter for the various devices suported
            JObject jo = JObject.Load(reader);
            switch (jo["Type"].Value<string>())
            {
                case "Arduino":
                    return JsonConvert.DeserializeObject<Arduino>(jo.ToString(), SpecifiedSubclassConversion);
                case "Aura":
                    return JsonConvert.DeserializeObject<Arduino>(jo.ToString(), SpecifiedSubclassConversion);
                case "Chroma":
                    return JsonConvert.DeserializeObject<Arduino>(jo.ToString(), SpecifiedSubclassConversion);

                default:
                    throw new Exception();
            }
            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
}
