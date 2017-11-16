using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.DirectInput;

namespace HueHue.Helpers
{
    public class JoystickHelper
    {
        DirectInput directInput = new DirectInput();

        public List<Guid> GetJoysticks()
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

        public Joystick HookJoystick(Guid guid)
        {
            Joystick result = new Joystick(directInput, guid);

            result.Properties.BufferSize = 128;

            result.Acquire();

            return result;
        }
    }
}
