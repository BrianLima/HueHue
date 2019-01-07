using CSCore;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.SoundOut;
using CSCore.Streams;
using CSCore.Streams.Effects;
using HueHue.Helpers;
using HueHue.Helpers.CSCore;
using HueHue.Helpers.Modes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for MusicMode.xaml
    /// For this mode we are going to sample the audio currently playing on the device
    /// then measure the frequency range and convert it to colors, brightness etc
    /// for reference see: https://i.imgur.com/8BHYI15.jpg
    /// 
    /// Heavily inspired by the samples provided by CSCore
    /// </summary>
    public partial class MusicMode : UserControl
    {
        private WasapiCapture _soundIn;
        private ISoundOut _soundOut;
        private IWaveSource _source;
        private PitchShifter _pitchShifter;
        DispatcherTimer timer;

        private List<LEDBulb> original_colors;

        //Bass frequencies
        private VoicePrint3DSpectrum _DeepBassSpectrum;
        private VoicePrint3DSpectrum _MidBassSpectrum;
        private VoicePrint3DSpectrum _UpperBassSpectrum;

        private VoicePrint3DSpectrum _DeepMidSpectrum;
        private VoicePrint3DSpectrum _MidMidSpectrum;
        private VoicePrint3DSpectrum _UpperMidSpectrum;

        private VoicePrint3DSpectrum _HighSpectrum;
        private VoicePrint3DSpectrum _UltraHighSpectrum;

        public MusicMode()
        {
            InitializeComponent();

            while (Mode.Colors.Count < 8)
            {
                Mode.Colors.Add(new LEDBulb());
            }

            original_colors = new List<LEDBulb>(Mode.Colors);

            //original_colors.Freeze();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 16);
            timer.Tick += Timer1_Tick;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            var size = FftSize.Fft256;
            var fftBuffer = new float[(int)size];

            _DeepBassSpectrum.UpdateFrequencyMapping();
            _DeepBassSpectrum.SpectrumProvider.GetFftData(fftBuffer, _DeepBassSpectrum);
            var db = _DeepBassSpectrum.CalculateSpectrumPoints(1, fftBuffer);
            Mode.Colors[0] = LEDBulb.Subtract(db[0].Value, LEDBulb.PropertyType.Saturation, original_colors[0]);

            _MidBassSpectrum.UpdateFrequencyMapping();
            _MidBassSpectrum.SpectrumProvider.GetFftData(fftBuffer, _MidBassSpectrum);
            var mb = _MidBassSpectrum.CalculateSpectrumPoints(1, fftBuffer);
            Mode.Colors[1] = LEDBulb.Subtract(mb[0].Value, LEDBulb.PropertyType.Saturation, original_colors[1]);

            _UpperBassSpectrum.UpdateFrequencyMapping();
            _UpperBassSpectrum.SpectrumProvider.GetFftData(fftBuffer, _UpperBassSpectrum);
            var ub = _MidBassSpectrum.CalculateSpectrumPoints(1, fftBuffer);
            Mode.Colors[2] = LEDBulb.Subtract(ub[0].Value, LEDBulb.PropertyType.Saturation, original_colors[2]);

            _DeepMidSpectrum.UpdateFrequencyMapping();
            _DeepMidSpectrum.SpectrumProvider.GetFftData(fftBuffer, _DeepMidSpectrum);
            var dm = _DeepMidSpectrum.CalculateSpectrumPoints(1, fftBuffer);
            Mode.Colors[3] = LEDBulb.Subtract(dm[0].Value, LEDBulb.PropertyType.Saturation, original_colors[3]);

            _MidMidSpectrum.UpdateFrequencyMapping();
            _MidMidSpectrum.SpectrumProvider.GetFftData(fftBuffer, _MidMidSpectrum);
            var mm = _MidMidSpectrum.CalculateSpectrumPoints(1, fftBuffer);
            Mode.Colors[4] = LEDBulb.Subtract(mm[0].Value, LEDBulb.PropertyType.Saturation, original_colors[4]);

            _UpperMidSpectrum.UpdateFrequencyMapping();
            _UpperMidSpectrum.SpectrumProvider.GetFftData(fftBuffer, _UpperMidSpectrum);
            var um = _UpperMidSpectrum.CalculateSpectrumPoints(1, fftBuffer);
            Mode.Colors[5] = LEDBulb.Subtract(um[0].Value, LEDBulb.PropertyType.Saturation, original_colors[5]);

            _HighSpectrum.UpdateFrequencyMapping();
            _HighSpectrum.SpectrumProvider.GetFftData(fftBuffer, _HighSpectrum);
            var h = _HighSpectrum.CalculateSpectrumPoints(1, fftBuffer);
            Mode.Colors[6] = LEDBulb.Subtract(h[0].Value, LEDBulb.PropertyType.Saturation, original_colors[6]);

            _UltraHighSpectrum.UpdateFrequencyMapping();
            _UltraHighSpectrum.SpectrumProvider.GetFftData(fftBuffer, _UltraHighSpectrum);
            var uh = _UltraHighSpectrum.CalculateSpectrumPoints(1, fftBuffer);
            Mode.Colors[7] = LEDBulb.Subtract(h[0].Value, LEDBulb.PropertyType.Saturation, original_colors[7]);

            Console.WriteLine("db {0}, mb{1}, hb{2}", db[0].Value, mb[0].Value, ub[0].Value);

            Mode.Music();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Stop();

            //open the default device
            _soundIn = new WasapiLoopbackCapture(20, new WaveFormat(48000, 24, 2));
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

            //Listen
            _soundIn.Start();
            timer.Start();
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
            var spectrumProvider = new BasicSpectrumProvider(aSampleSource.WaveFormat.Channels, aSampleSource.WaveFormat.SampleRate, fftSize);

            //linespectrum and voiceprint3dspectrum used for rendering some fft data
            //in oder to get some fft data, set the previously created spectrumprovider 
            _DeepBassSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = false,
                PointCount = 1,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Decibel,
                MaximumFrequency = 40,
                MinimumFrequency = 20,
            };

            _MidBassSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = false,
                PointCount = 1,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Decibel,
                MaximumFrequency = 160,
                MinimumFrequency = 40,
            };

            _UpperBassSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = false,
                PointCount = 1,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Decibel,
                MaximumFrequency = 300,
                MinimumFrequency = 160,
            };

            _DeepMidSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = false,
                PointCount = 1,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Decibel,
                MaximumFrequency = 800,
                MinimumFrequency = 300,
            };

            _MidMidSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = false,
                PointCount = 1,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Decibel,
                MaximumFrequency = 2500,
                MinimumFrequency = 800,
            };

            _UpperMidSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = false,
                PointCount = 1,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Decibel,
                MaximumFrequency = 5000,
                MinimumFrequency = 2500,
            };

            _HighSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = false,
                PointCount = 1,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Decibel,
                MaximumFrequency = 10000,
                MinimumFrequency = 5000,
            };

            _UltraHighSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = false,
                PointCount = 1,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Decibel,
                MaximumFrequency = 20000,
                MinimumFrequency = 10000,
            };

            // The SingleBlockNotificationStream is used to intercept the played samples
            var notificationSource = new SingleBlockNotificationStream(aSampleSource);
            // Pass the intercepted samples as input data to the spectrumprovider (which will calculate a fft based on them)
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
