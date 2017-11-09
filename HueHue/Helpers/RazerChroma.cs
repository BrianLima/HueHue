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
                            await Task.Run(() => Chroma.Instance.Keyboard.SetAll(new Color(Effects.Colors[0].R, Effects.Colors[0].G, Effects.Colors[0].B)));
                            break;
                        case SubType.Mouse:
                            await Task.Run(() => Chroma.Instance.Mouse.SetAll(new Color(Effects.Colors[0].R, Effects.Colors[0].G, Effects.Colors[0].B)));
                            break;
                        case SubType.Headset:
                            await Task.Run(() => Chroma.Instance.Headset.SetAll(new Color(Effects.Colors[0].R, Effects.Colors[0].G, Effects.Colors[0].B)));
                            break;
                        case SubType.Mousepad:
                            await Task.Run(() => Chroma.Instance.Mousepad.SetAll(new Color(Effects.Colors[0].R, Effects.Colors[0].G, Effects.Colors[0].B)));
                            break;
                        case SubType.Keypad:
                            await Task.Run(() => Chroma.Instance.Keypad.SetAll(new Color(Effects.Colors[0].R, Effects.Colors[0].G, Effects.Colors[0].B)));
                            break;
                        case SubType.All:
                            await Task.Run(() => Chroma.Instance.SetAll(new Color(Effects.Colors[0].R, Effects.Colors[0].G, Effects.Colors[0].B)));
                            break;
                        default:
                            break;
                    }
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