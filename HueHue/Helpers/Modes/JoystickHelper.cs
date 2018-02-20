using System;
using System.Collections.Generic;
using SharpDX.DirectInput;
using Newtonsoft.Json;
using System.IO;
using RGB.NET.Core;
using Newtonsoft.Json.Linq;
using static HueHue.Helpers.Modes.JoystickButtonToColor;
using HueHue.Helpers.Devices;

namespace HueHue.Helpers.Modes
{
    public class JoystickHelper
    {
        private DirectInput directInput = new DirectInput();

        /// <summary>
        /// Querys all available joysticks on the computer
        /// </summary>
        /// <returns>List<Guid>Joysticks</returns>
        public List<Guid> GetGuids()
        {
            List<Guid> results = new List<Guid>();

            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
            {
                results.Add(deviceInstance.InstanceGuid);
            }

            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
            {
                results.Add(deviceInstance.InstanceGuid);
            }

            foreach (var deviceInstance in directInput.GetDevices(DeviceType.ControlDevice, DeviceEnumerationFlags.AllDevices))
            {
                results.Add(deviceInstance.InstanceGuid);
            }

            return results;
        }

        /// <summary>
        /// Finds the human relatable name for each joystick
        /// </summary>
        /// <param name="guids"></param>
        /// <param name="directInput"></param>
        /// <returns></returns>
        public List<String> GetJoystickNames(List<Guid> guids)
        {
            List<String> result = new List<string>(); ;

            foreach (var item in guids)
            {
                Joystick j = new Joystick(directInput, item);
                result.Add(j.Information.InstanceName);
            }

            return result;
        }

        /// <summary>
        /// Hooks to a selected joystick
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public Joystick HookJoystick(Guid guid)
        {
            Joystick result = new Joystick(directInput, guid);

            result.Properties.BufferSize = 128;

            result.Acquire();

            return result;
        }

        /// <summary>
        /// Saves the list of buttons of the currently configured joystick
        /// </summary>
        /// <param name="buttons"></param>
        /// <param name="guid"></param>
        public void SaveJoystickButtons(List<JoystickButtonToColor> buttons, Guid guid)
        {
            var json = JsonConvert.SerializeObject(buttons);
            File.WriteAllText(App.settings.AppData + guid.ToString() + ".json", json);
        }

        /// <summary>
        /// Checks if a file with a List<JoystickButtonToCollor> for a said joystick exists 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>A list of buttons for the said joystick</returns>
        public List<JoystickButtonToColor> LoadJoystickButtons(Guid guid)
        {
            List<JoystickButtonToColor> result = new List<JoystickButtonToColor>();

            if (File.Exists(App.settings.AppData + guid.ToString() + ".json"))
            {
                result = JsonConvert.DeserializeObject<List<JoystickButtonToColor>>(File.ReadAllText(App.settings.AppData + guid.ToString() + ".json"));
            }

            return result;
        }
    }

    [JsonConverter(typeof(JoystickButtonToColorJsonConverter))]
    public class JoystickButtonToColor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public JoystickButtonToColor()
        {

        }

        public JoystickButtonToColor(Color color, JoystickOffset button, ButtonTypeEnum buttonType, byte pressedBrightness, byte releasedBrightness)
        {
            this._color = color;
            this._button = button;
            this._button_type = buttonType;
            this._pressed_brightness = pressedBrightness;
            this._released_brightness = releasedBrightness;
        }

        public enum ButtonTypeEnum
        {
            Color,
            Brightness
        }

        private Color _color;
        /// <summary>
        /// Gets or sets the color of the desired button
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private JoystickOffset _button;

        public JoystickOffset Button
        {
            get { return _button; }
            set { _button = value; }
        }

        private ButtonTypeEnum _button_type;

        public ButtonTypeEnum ButtonType
        {
            get { return _button_type; }
            set { _button_type = value; }
        }

        private byte _pressed_brightness;

        public byte PressedBrightness
        {
            get { return _pressed_brightness; }
            set { _pressed_brightness = value; }
        }

        private byte _released_brightness;

        public byte ReleasedBrightness
        {
            get { return _released_brightness; }
            set { _released_brightness = value; }
        }
    }

    public class JoystickButtonToColorJsonConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(JoystickButtonToColor));
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

            Byte.TryParse(jo["Color"]["A"].Value<string>(), out byte a);
            Byte.TryParse(jo["Color"]["R"].Value<string>(), out byte r);
            Byte.TryParse(jo["Color"]["G"].Value<string>(), out byte g);
            Byte.TryParse(jo["Color"]["B"].Value<string>(), out byte b);

            //Enums are stored as int64 on the JSON, to read it properly converting the index is needed
            JoystickOffset button = (JoystickOffset)jo["Button"].Value<Int64>();
            ButtonTypeEnum buttonType = (ButtonTypeEnum)jo["ButtonType"].Value<Int64>();

            return new JoystickButtonToColor(new Color(a, r, g, b), button, buttonType, jo["PressedBrightness"].Value<byte>(), jo["ReleasedBrightness"].Value<byte>());
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