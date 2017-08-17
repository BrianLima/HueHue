using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using HueHue.Properties;

namespace HueHue.Helpers
{
    public class SerialStream : IDisposable
    {
        public SerialStream()
        {
        }

        private ILogger _log = LogManager.GetCurrentClassLogger();
        private bool running = false;

        private readonly byte[] _messagePreamble = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09 };

        private Thread _workerThread;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public static string[] GetPorts()
        {
            return SerialPort.GetPortNames();
        }

        public void Start()
        {
            _log.Debug("Start called.");
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

        public void Stop()
        {
            _log.Debug("Stop called.");
            if (_workerThread == null) return;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
            _workerThread.Join();
            _workerThread = null;
            running = false;
        }

        private byte[] GetOutputStream()
        {
            byte[] outputStream;

            int counter = _messagePreamble.Length;
            const int colorsPerLed = 3;
            outputStream = new byte[_messagePreamble.Length + (Settings.Default.TotalLeds * colorsPerLed) + 2]; //3 colors per led, +1 for the brightness +1 to tell a specific effect
            Buffer.BlockCopy(_messagePreamble, 0, outputStream, 0, _messagePreamble.Length);

            outputStream[counter++] = Properties.Settings.Default.Breath ? (byte)1 : (byte)0; //If the user wants to use a breath effect this will be 1 and the arduino will handle the brightness, else it's 0 andit will use the next byte as brightness
            outputStream[counter++] = Properties.Settings.Default.Brightness; //Set the brightness as the first byte after the preamble

            foreach (LEDBulb bulb in Effects.LEDs)
            {
                outputStream[counter++] = bulb.B; // blue
                outputStream[counter++] = bulb.G; // green
                outputStream[counter++] = bulb.R; // red
            }

            return outputStream;
        }

        private void BackgroundWorker_DoWork(object tokenObject)
        {
            var cancellationToken = (CancellationToken)tokenObject;
            SerialPort serialPort = null;

            if (String.IsNullOrEmpty(Settings.Default.COM_PORT)) return;

            //retry after exceptions
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    const int baudRate = 1000000; // 115200;
                    serialPort = new SerialPort(Settings.Default.COM_PORT, baudRate);
                    serialPort.Open();

                    //send frame data
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        _stopwatch.Start();

                        byte[] outputStream = GetOutputStream();
                        serialPort.Write(outputStream, 0, outputStream.Length);

                        //ws2812b LEDs need 30 µs = 0.030 ms for each led to set its color so there is a lower minimum to the allowed refresh rate
                        //receiving over serial takes it time as well and the arduino does both tasks in sequence
                        //+1 ms extra safe zone
                        var fastLedTime = (outputStream.Length - _messagePreamble.Length) / 3.0 * 0.030d;
                        var serialTransferTime = outputStream.Length * 10.0 * 1000.0 / baudRate;
                        var minTimespan = (int)(fastLedTime + serialTransferTime) + 1;

                        var delayInMs = Math.Max(minTimespan, 0 - (int)_stopwatch.ElapsedMilliseconds);
                        if (delayInMs > 0)
                        {
                            Task.Delay(delayInMs, cancellationToken).Wait(cancellationToken);
                        }

                        _stopwatch.Reset();
                    }
                }
                catch (OperationCanceledException)
                {
                    _log.Debug("OperationCanceledException catched. returning.");
                    running = false;
                    return;
                }
                catch (Exception ex)
                {
                    _log.Debug(ex, "Exception catched.");
                    //to be safe, we reset the serial port
                    if (serialPort != null && serialPort.IsOpen)
                    {
                        serialPort.Close();
                    }
                    serialPort?.Dispose();
                    serialPort = null;
                    running = false;
                }
                finally
                {
                    if (serialPort != null && serialPort.IsOpen)
                    {
                        serialPort.Close();
                        serialPort.Dispose();
                    }
                }
            }
        }

        public  bool IsRunning()
        {
            return running;
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