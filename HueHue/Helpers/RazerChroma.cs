using System;
using System.Threading.Tasks;
using Corale.Colore.Core;
using System.Threading;

namespace HueHue.Helpers
{
    class RazerChroma : Device, IDisposable
    {
        private Thread _workerThread;
        private CancellationTokenSource _cancellationTokenSource;
        private bool running = false;

        public RazerChroma(string _type, SubType _subType, string _Name)
        {
            this.Type = _type;
            this.subType = _subType;
            this.Name = _Name;
            this.Icon = "/HueHue;component/Icons/Devices/RazerChroma.png";
        }

        public override void Start()
        {
            if (_workerThread != null) return;

            _cancellationTokenSource = new CancellationTokenSource();
            _workerThread = new Thread(BackgroundWorker_DoWork)
            {
                Name = "Serial sending",
                IsBackground = true
            };
            _workerThread.Start(_cancellationTokenSource.Token);
            running = true;

            if (!Chroma.Instance.Initialized)
            {
                Chroma.Instance.Initialize();
            }
        }

        private async void BackgroundWorker_DoWork(object tokenObject)
        {

            //TODO: Implement device/effect specific color handling
            var cancellationToken = (CancellationToken)tokenObject;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    switch (subType)
                    {
                        case SubType.Keyboard:
                            var keyboardGrid = Corale.Colore.Razer.Keyboard.Effects.Custom.Create();
                            //Loop through all Rows
                            for (var r = 0; r < Corale.Colore.Razer.Keyboard.Constants.MaxRows; r++)
                            {
                                //Loop through all Columns
                                for (var c = 0; c < Corale.Colore.Razer.Keyboard.Constants.MaxColumns; c++)
                                {
                                    // Set the current row and column to the random color
                                    keyboardGrid[r, c] = (new Color(Effects.LEDs[c].R, Effects.LEDs[c].G, Effects.LEDs[c].B));
                                }
                            }

                            await Task.Run(() => Chroma.Instance.Keyboard.SetCustom(keyboardGrid));
                            break;
                        case SubType.Mouse:
                            var mouseCustom = Corale.Colore.Razer.Mouse.Effects.Custom.Create();
                            for (int i = 0; i < Corale.Colore.Razer.Mouse.Constants.MaxLeds; i++)
                            {
                                mouseCustom[i] = (new Color(Effects.LEDs[i].R, Effects.LEDs[i].G, Effects.LEDs[i].B));
                            }
                            await Task.Run(() => Chroma.Instance.Mouse.SetCustom(mouseCustom));
                            break;
                        case SubType.Headset: //LEDs on a headset for some reason doesn't seem to be adressable
                                              //var headCustom = Corale.Colore.Razer.Headset.Effects.Static;

                            await Task.Run(() => Chroma.Instance.Headset.SetAll(new Color(Effects.LEDs[0].R, Effects.LEDs[0].G, Effects.LEDs[0].B)));
                            break;
                        case SubType.Mousepad:
                            var padCustom = Corale.Colore.Razer.Mousepad.Effects.Custom.Create();
                            for (int i = 0; i < Corale.Colore.Razer.Mousepad.Constants.MaxLeds; i++)
                            {
                                padCustom[i] = (new Color(Effects.LEDs[i].R, Effects.LEDs[i].G, Effects.LEDs[i].B));
                            }
                            await Task.Run(() => Chroma.Instance.Mousepad.SetCustom(padCustom));
                            break;
                        case SubType.Keypad:
                            var keypadGrid = Corale.Colore.Razer.Keypad.Effects.Custom.Create();
                            // Loop through all Rows
                            for (var r = 0; r < Corale.Colore.Razer.Keypad.Constants.MaxRows; r++)
                            {
                                //Loop through all Columns
                                for (var c = 0; c < Corale.Colore.Razer.Keypad.Constants.MaxColumns; c++)
                                {
                                    // Set the current row and column to the random color
                                    keypadGrid[r, c] = (new Color(Effects.LEDs[c].R, Effects.LEDs[c].G, Effects.LEDs[c].B));
                                }
                            }
                            await Task.Run(() => Chroma.Instance.Keypad.SetCustom(keypadGrid)); break;
                        case SubType.All:
                            await Task.Run(() => Chroma.Instance.SetAll(new Color(Effects.LEDs[0].R, Effects.LEDs[0].G, Effects.LEDs[0].B)));
                            break;
                        default:
                            break;
                    }

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
            if (_workerThread == null) return;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
            _workerThread.Join();
            _workerThread = null;
            running = false;
            Chroma.Instance.Uninitialize();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
            }
        }
    }
}