using HueHue.Helpers.Devices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.IO;
using static HueHue.Helpers.Modes.JoystickButtonToColor;

namespace HueHue.Helpers.Modes
{
    public class JoystickHelper
    {
        private DirectInput directInput = new DirectInput();

        /// <summary>
        /// Queries all available joysticks on the computer
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

        public JoystickButtonToColor(LEDBulb color, JoystickOffset button, ButtonTypeEnum buttonType, byte pressedBrightness, byte releasedBrightness, int[] minCenterMaxValue)
        {
            this._color = color;
            this._button = button;
            this._button_type = buttonType;
            this._pressed_brightness = pressedBrightness;
            this._Centered_brightness = releasedBrightness;
            this._min_max_value = minCenterMaxValue;
        }

        public JoystickButtonToColor()
        {
        }

        public enum ButtonTypeEnum
        {
            Color,
            Brightness
        }

        private LEDBulb _color;
        /// <summary>
        /// Gets or sets the color of the desired button
        /// </summary>
        public LEDBulb Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private JoystickOffset _button;
        /// <summary>
        /// Gets or sets the desired button
        /// </summary>
        public JoystickOffset Button
        {
            get { return _button; }
            set { _button = value; }
        }

        private ButtonTypeEnum _button_type;
        /// <summary>
        /// Gets or sets if this button is controlling color or brightness
        /// </summary>
        public ButtonTypeEnum ButtonType
        {
            get { return _button_type; }
            set { _button_type = value; }
        }

        private byte _pressed_brightness;
        /// <summary>
        /// Gets or sets the brightness to set when this button is pressed
        /// </summary>
        public byte PressedBrightness
        {
            get { return _pressed_brightness; }
            set { _pressed_brightness = value; }
        }

        private byte _Centered_brightness;
        /// <summary>
        /// Gets or sets the brightness to set when this button is released
        /// </summary>
        public byte CenteredBrightness
        {
            get { return _Centered_brightness; }
            set { _Centered_brightness = value; }
        }

        private int[] _min_max_value;
        /// <summary>
        /// Gets or sets the maximum known positions for this axis when ControlType is brightness
        /// </summary>
        public int[] MinMaxValue
        {
            get { return _min_max_value; }
            private set { _min_max_value = value; }
        }

        /// <summary>
        /// Sets the min and max values for this axis
        /// </summary>
        public void SetMinMaxValues(int value)
        {
            if (this._min_max_value == null)
            {
                _min_max_value = new int[2] { value, value };
            }

            if (_min_max_value[0] > value)
            {
                _min_max_value[0] = value;
            }
            else if (value > _min_max_value[1])
            {
                _min_max_value[1] = value;
            }
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

            //Byte.TryParse(jo["Color"]["A"].Value<string>(), out byte a);
            Byte.TryParse(jo["Color"]["R"].Value<string>(), out byte r);
            Byte.TryParse(jo["Color"]["G"].Value<string>(), out byte g);
            Byte.TryParse(jo["Color"]["B"].Value<string>(), out byte b);

            int[] minmax = new int[2];
            for (int i = 0; i < 2; i++)
            {
                //TODO: Improve this parsing
                minmax[i] = Int32.Parse(jo["MinMaxValue"][i].ToString());
            }

            //Enums are stored as int64 on the JSON, to read it properly we need to use a index
            JoystickOffset button = (JoystickOffset)jo["Button"].Value<Int64>();
            ButtonTypeEnum buttonType = (ButtonTypeEnum)jo["ButtonType"].Value<Int64>();
            return new JoystickButtonToColor(new LEDBulb(r, g, b), 
                                             button, 
                                             buttonType, 
                                             jo["PressedBrightness"].Value<byte>(), 
                                             jo["CenteredBrightness"].Value<byte>(), 
                                             minmax);
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