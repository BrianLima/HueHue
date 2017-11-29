using System;
using System.Collections.Generic;
using SharpDX.DirectInput;

namespace HueHue.Helpers
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
    }

    

    public class JoystickButtonToColor
    {
        public enum ButtonTypeEnum
        {
            Color,
            Brightness
        }

        private LEDBulb _color;

        public LEDBulb Color
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
}
