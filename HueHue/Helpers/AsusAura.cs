using RGB.NET.Devices.Asus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueHue.Helpers
{
    public class AsusAura : Device, IDisposable
    {
        AsusDeviceProvider provider;

        public override void Start()
        {
            provider = new AsusDeviceProvider();
            if (!provider.IsInitialized)
            {
                provider.Initialize(RGB.NET.Core.RGBDeviceType.All, false, false);
            }
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
