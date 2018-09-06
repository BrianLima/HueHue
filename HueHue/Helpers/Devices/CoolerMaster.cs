using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RGB.NET.Core;
using RGB.NET.Devices.CoolerMaster;
using RGB.NET.Groups;

namespace HueHue.Helpers.Devices
{
    class CoolerMaster : Device, IDisposable
    {
        private Thread _workerThread;
        private CancellationTokenSource _cancellationTokenSource;
        private bool running = false;
        dynamic device;
        ILedGroup deviceleds;
        IDeviceUpdateTrigger t;
       
        //<CoolerMasterMouseRGBDevice> d;

        CoolerMasterDeviceProvider provider;

        public CoolerMaster(string _type, SubType _subType, string _Name)
        {
            this.Type = _type;
            this.subType = _subType;
            this.Name = _Name;
            this.Icon = "/HueHue;component/Icons/Devices/CoolerMaster.png";

            if (!CoolerMasterDeviceProvider.Instance.IsInitialized)
            {
                CoolerMasterDeviceProvider.Instance.Initialize();
            }

            App.surface.LoadDevices(CoolerMasterDeviceProvider.Instance, RGBDeviceType.Mouse, true);

            //deviceleds = new ListLedGroup(RGBSurface.Instance.Leds).e;
            //deviceleds
        }

        public static List<String> GetCoolerMasterDevices()
        {
            if (!CoolerMasterDeviceProvider.Instance.IsInitialized)
            {
                CoolerMasterDeviceProvider.Instance.Initialize();
            }

            List<String> result = new List<string>();

            foreach (var item in CoolerMasterDeviceProvider.Instance.Devices)
            {
                result.Add(item.DeviceInfo.Model);
            }

            return result;
        }

        public override void Start()
        {
            if (!CoolerMasterDeviceProvider.Instance.IsInitialized)
            {
                CoolerMasterDeviceProvider.Instance.Initialize();
            }

            if (_workerThread != null) return;

            _cancellationTokenSource = new CancellationTokenSource();
            _workerThread = new Thread(BackgroundWorker_DoWork)
            {
                Name = "HueHue Device " + subType,
                IsBackground = true
            };
            _workerThread.Start(_cancellationTokenSource.Token);
            running = true;
        }

        private void BackgroundWorker_DoWork(object tokenObject)
        {
            var cancellationToken = (CancellationToken)tokenObject;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var x = CoolerMasterDeviceProvider.Instance.Devices;
                    //foreach (< item> in x)
                    //{
                        
                    //    App.surface.Leds[0].se
                    //    if (true)
                    //    {

                    //    }

                    //}
                    //switch (subType)
                    //{
                    //    case SubType.Keyboard:
                    //        RazerRGBDevice<RazerKeyboardRGBDeviceInfo>()
                    //        var keyboardGrid = Corale.Colore.Razer.Keyboard.Mode.Custom.Create();
                    //        //Loop through all Rows
                    //        for (var r = 0; r < Corale.Colore.Razer.Keyboard.Constants.MaxRows; r++)
                    //        {
                    //            //Loop through all Columns
                    //            for (var c = 0; c < Corale.Colore.Razer.Keyboard.Constants.MaxColumns; c++)
                    //            {
                    //                // Set the current row and column to the random color
                    //                keyboardGrid[r, c] = (new Color(Mode.LEDs[c].R, Mode.LEDs[c].G, Mode.LEDs[c].B));
                    //            }
                    //        }

                    //        await Task.Run(() => Chroma.Instance.Keyboard.SetCustom(keyboardGrid));
                    //        break;
                    //    case SubType.Mouse:
                    //        var mouseCustom = Corale.Colore.Razer.Mouse.Mode.Custom.Create();
                    //        for (int i = 0; i < Corale.Colore.Razer.Mouse.Constants.MaxLeds; i++)
                    //        {
                    //            mouseCustom[i] = (new Color(Mode.LEDs[i].R, Mode.LEDs[i].G, Mode.LEDs[i].B));
                    //        }
                    //        await Task.Run(() => Chroma.Instance.Mouse.SetCustom(mouseCustom));
                    //        break;
                    //    case SubType.Headset: //LEDs on a headset for some reason doesn't seem to be adressable
                    //                          //var headCustom = Corale.Colore.Razer.Headset.Mode.Static;

                    //        await Task.Run(() => Chroma.Instance.Headset.SetAll(new Color(Mode.LEDs[0].R, Mode.LEDs[0].G, Mode.LEDs[0].B)));
                    //        break;
                    //    case SubType.Mousepad:
                    //        var padCustom = Corale.Colore.Razer.Mousepad.Mode.Custom.Create();
                    //        for (int i = 0; i < Corale.Colore.Razer.Mousepad.Constants.MaxLeds; i++)
                    //        {
                    //            padCustom[i] = (new Color(Mode.LEDs[i].R, Mode.LEDs[i].G, Mode.LEDs[i].B));
                    //        }
                    //        await Task.Run(() => Chroma.Instance.Mousepad.SetCustom(padCustom));
                    //        break;
                    //    case SubType.Keypad:
                    //        var keypadGrid = Corale.Colore.Razer.Keypad.Mode.Custom.Create();
                    //        // Loop through all Rows
                    //        for (var r = 0; r < Corale.Colore.Razer.Keypad.Constants.MaxRows; r++)
                    //        {
                    //            //Loop through all Columns
                    //            for (var c = 0; c < Corale.Colore.Razer.Keypad.Constants.MaxColumns; c++)
                    //            {
                    //                // Set the current row and column to the random color
                    //                keypadGrid[r, c] = (new Color(Mode.LEDs[c].R, Mode.LEDs[c].G, Mode.LEDs[c].B));
                    //            }
                    //        }
                    //        await Task.Run(() => Chroma.Instance.Keypad.SetCustom(keypadGrid)); break;
                    //    case SubType.All:
                    //        await Task.Run(() => Chroma.Instance.SetAll(new Color(Mode.LEDs[0].R, Mode.LEDs[0].G, Mode.LEDs[0].B)));
                    //        break;
                    //    default:
                    //        break;
                    //}

                    //15 so we aproach 60~70 fps
                    Task.Delay(15, cancellationToken).Wait(cancellationToken);
                }
                catch (Exception ex)
                {

                }
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
