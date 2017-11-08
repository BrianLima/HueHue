using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corale.Colore.Core;
using System.Threading;
using System.Diagnostics;

namespace HueHue.Helpers
{
    class RazerChroma : Device, IDisposable
    {
        private Thread _workerThread;
        private CancellationTokenSource _cancellationTokenSource;
        private bool running = false;

        public RazerChroma(string _type, string _Name)
        {
            this.Type = _type;
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
        }

        private async void BackgroundWorker_DoWork(object tokenObject)
        {
            var cancellationToken = (CancellationToken)tokenObject;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    switch (Type)
                    {
                        case "Chroma Keyboard":
                            await Task.Run(() => Chroma.Instance.Keyboard.SetAll(new Color(Effects.Colors[0].R, Effects.Colors[0].G, Effects.Colors[0].B)));
                            break;
                        //case RazerType.Headset:
                        //    break;
                        //case RazerType.Keypad:
                        //    break;
                        //case RazerType.Mouse:
                        //    break;
                        //case RazerType.Mousepad:
                        //    break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {

                    throw;
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