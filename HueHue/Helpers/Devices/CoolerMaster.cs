using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RGB.NET.Core;
using RGB.NET.Devices.CoolerMaster;

namespace HueHue.Helpers.Devices
{
    class CoolerMaster : Device, IDisposable
    {
        private Thread _workerThread;
        private CancellationTokenSource _cancellationTokenSource;
        private bool running = false;

        CoolerMasterDeviceProvider provider;

        public CoolerMaster(string _type, SubType _subType, string _Name)
        {
            this.Type = _type;
            this.subType = _subType;
            this.Name = _Name;
            this.Icon = "/HueHue;component/Icons/Devices/CoolerMaster.png";
        }

        public static void GetCoolerMasterDevices()
        {
            RGBSurface s = RGBSurface.Instance;

            if (!CoolerMasterDeviceProvider.Instance.IsInitialized)
            {
                CoolerMasterDeviceProvider.Instance.Initialize();
            }


            //var a=  App.surface.Devices.OfType<RGBDeviceType.GraphicsCard>();
            s.LoadDevices(CoolerMasterDeviceProvider.Instance);
            foreach (var item in s.Devices)
            {

            }
        }

        public override void Start()
        {
            provider = new CoolerMasterDeviceProvider();
            if (!provider.IsInitialized)
            {
                provider.Initialize();
            }

            if (_workerThread != null) return;

            _cancellationTokenSource = new CancellationTokenSource();
            _workerThread = new Thread(BackgroundWorker_DoWork)
            {
                Name = "Serial sending",
                IsBackground = true
            };
            _workerThread.Start(_cancellationTokenSource.Token);
            running = true;

            if (!CoolerMasterDeviceProvider.Instance.IsInitialized)
            {
                CoolerMasterDeviceProvider.Instance.Initialize();
            }


        }

        private void BackgroundWorker_DoWork()
        {
            throw new NotImplementedException();
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
