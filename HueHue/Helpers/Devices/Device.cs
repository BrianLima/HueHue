using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;

namespace HueHue.Helpers.Devices
{
    /// <summary>
    /// Generic class representing a device supported by HueHue
    /// </summary>
    [JsonConverter(typeof(DeviceJsonConverter))]
    public abstract class Device
    {
        /// <summary>
        /// Stores the SubType(Keyboard, Mouse, etc...) of a SDK supported device
        /// </summary>
        public enum SubType 
        {
            Keyboard,
            Mouse,
            Headset,
            Mousepad,
            Keypad,
            All
            //IF you are going to add a new device type, say a RGB refrigerator, 
            //please, add it on the last position to avoid conflicts with devices added prior
            //because when serialized, it is stored as a index on Devices.json.
        }

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

        private SubType _subType;

        public SubType subType
        {
            get { return _subType; }
            set { _subType = value; }
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
    public class DeviceJsonConverter : JsonConverter
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
                    return JsonConvert.DeserializeObject<RazerChroma>(jo.ToString(), SpecifiedSubclassConversion);
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
