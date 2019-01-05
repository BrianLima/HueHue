﻿using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.SoundOut;
using CSCore.Streams;
using CSCore.Streams.Effects;
using HueHue.Helpers.CSCore;
using HueHue.Helpers.Modes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for MusicMode.xaml
    /// </summary>
    public partial class MusicMode : UserControl
    {
        private WasapiCapture _soundIn;
        private ISoundOut _soundOut;
        private IWaveSource _source;
        private PitchShifter _pitchShifter;
//        private LineSpectrum _lineSpectrum; //TODO: create my handlers and converters from the audio spectrum to the RGB spectrum
//        private VoicePrint3DSpectrum _voicePrint3DSpectrum;
        DispatcherTimer timer;
        private VoicePrint3DSpectrum _BassSpectrum;

        public MusicMode()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            //timer1.
            timer.Interval = new TimeSpan(0, 0, 0, 0, 16);
            timer.Tick += Timer1_Tick;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            var size = FftSize.Fft256;
            var fftBuffer = new float[(int)size];
            //render the spectrum
            //GenerateLineSpectrum();   
            _BassSpectrum.UpdateFrequencyMapping();
            _BassSpectrum.SpectrumProvider.GetFftData(fftBuffer, _BassSpectrum);
            var r  = _BassSpectrum.CalculateSpectrumPoints(255, fftBuffer);
            if (true)
            {
                App.settings.Brightness = (byte)Math.Ceiling(r[0].Value);
            }
        }

        private void GenerateLineSpectrum()
        {
            //Image image = pictureBoxTop.Image;
            //var newImage = _lineSpectrum.CreateSpectrumLine(pictureBoxTop.Size, Color.Green, Color.Red, Color.Black, true);
            //if (newImage != null)
            //{
            //    pictureBoxTop.Image = newImage;
            //    if (image != null)
            //        image.Dispose();
            //}
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Stop();

            //open the default device
            _soundIn = new WasapiLoopbackCapture(100, new WaveFormat(48000, 24, 2));
            //Our loopback capture opens the default render device by default so the following is not needed
            //_soundIn.Device = MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Render, Role.Console);
            _soundIn.Initialize();

            var soundInSource = new SoundInSource(_soundIn);
            ISampleSource source = soundInSource.ToSampleSource().AppendSource(x => new PitchShifter(x), out _pitchShifter);

            SetupSampleSource(source);

            // We need to read from our source otherwise SingleBlockRead is never called and our spectrum provider is not populated
            byte[] buffer = new byte[_source.WaveFormat.BytesPerSecond / 2];
            soundInSource.DataAvailable += (s, aEvent) =>
            {
                int read;
                while ((read = _source.Read(buffer, 0, buffer.Length)) > 0) ;
            };


            //play the audio
            _soundIn.Start();

            timer.Start();

            //propertyGridTop.SelectedObject = _lineSpectrum;
            //propertyGridBottom.SelectedObject = _voicePrint3DSpectrum;
        }

        private void Stop()
        {
            timer.Stop();

            if (_soundOut != null)
            {
                _soundOut.Stop();
                _soundOut.Dispose();
                _soundOut = null;
            }
            if (_soundIn != null)
            {
                _soundIn.Stop();
                _soundIn.Dispose();
                _soundIn = null;
            }
            if (_source != null)
            {
                _source.Dispose();
                _source = null;
            }
        }

        private void SetupSampleSource(ISampleSource aSampleSource)
        {
            const FftSize fftSize = FftSize.Fft64;
            //create a spectrum provider which provides fft data based on some input
            var spectrumProvider = new BasicSpectrumProvider(aSampleSource.WaveFormat.Channels,
                aSampleSource.WaveFormat.SampleRate, fftSize);
            //
            ////linespectrum and voiceprint3dspectrum used for rendering some fft data
            ////in oder to get some fft data, set the previously created spectrumprovider 
            _BassSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = false,
                PointCount = 1,
                IsXLogScale = false,
                ScalingStrategy = ScalingStrategy.Sqrt,
                MaximumFrequency = 250,
                MinimumFrequency = 20,
            };

            //the SingleBlockNotificationStream is used to intercept the played samples
            var notificationSource = new SingleBlockNotificationStream(aSampleSource);
            //pass the intercepted samples as input data to the spectrumprovider (which will calculate a fft based on them)
            notificationSource.SingleBlockRead += (s, a) => spectrumProvider.Add(a.Left, a.Right);

            _source = notificationSource.ToWaveSource(16);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow != null)
            {
                if (Application.Current.MainWindow.WindowState != WindowState.Minimized)
                {
                    Stop();
                    timer.Stop();
                }
            }
            else
            {
                Stop();
                timer.Stop();
            }
        }
    }
}
