using ColorTools;
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
using System.Windows.Input;
using System.Windows.Threading;
using Media = System.Windows.Media;

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

        FftSize fftSize = FftSize.Fft64;
        ScalingStrategy strategy = ScalingStrategy.Decibel;

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

            for (int i = 0; i < 10; i++)
            {
                ComboPoints.Items.Add(new ComboBoxItem() { Content = i + 1 });
            }

            original_colors = new List<LEDBulb>(Mode.Colors);
            this.ModeSettings.DataContext = App.settings;
            BindColors();
            SelectData();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 16);
            timer.Tick += Timer1_Tick;
        }

        void BindColors()
        {
            for (int i = 0; i < Mode.Colors.Count; i++)
            {
                ColorControlPanel panel = new ColorControlPanel();
                panel.Margin = new Thickness(10);

                //I couldn't in ANY way make it bind the color properly, at least this works
                //Binding each value from the RGB is broken
                //Binding the color it self conflicts because the controler uses System.Windows.Media.Color instead of System.Drawing.Color
                //I give up, this is it, MVVM for a later day.

                panel.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Mode.Colors[i].R, Mode.Colors[i].G, Mode.Colors[i].B));
                panel.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Mode.Colors[i].R, Mode.Colors[i].G, Mode.Colors[i].B));
                panel.DockAlphaVisibility = Visibility.Hidden;
                panel.Style = (Style)FindResource("StyleColorControlPanel");
                panel.ColorChanged += colorPicker_ColorChanged;
                panel.LostMouseCapture += Panel_LostMouseCapture;
                panel.LostKeyboardFocus += Panel_LostKeyboardFocus;
                panel.ToolTip = "Frequency";

                //No option to "remove" colors from music mdoe for now
                //ContextMenu context = new ContextMenu();
                //MenuItem item = new MenuItem();
                //item.Header = "Remove";
                //item.Click += Item_Click;
                //context.Items.Add(item);
                //panel.ContextMenu = context

                StackColors.Children.Add(panel);
            }
        }

        private void Panel_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            App.settings.Colors = Mode.Colors;
        }

        private void Panel_LostMouseCapture(object sender, MouseEventArgs e)
        {
            App.settings.Colors = Mode.Colors;
        }

        private void colorPicker_ColorChanged(object sender, ColorControlPanel.ColorChangedEventArgs e)
        {
            int index = StackColors.Children.IndexOf(sender as UIElement);
            Mode.Colors[index] = new LEDBulb(e.CurrentColor.R, e.CurrentColor.G, e.CurrentColor.B);
            original_colors[index] = new LEDBulb(e.CurrentColor.R, e.CurrentColor.G, e.CurrentColor.B);
        }

        void SelectData()
        {
            switch (ComboStrategy.SelectedIndex)
            {
                case 1:
                    strategy = ScalingStrategy.Linear;
                    break;
                case 2:
                    strategy = ScalingStrategy.Sqrt;
                    break;
                default:
                    strategy = ScalingStrategy.Decibel;
                    break;
            }

            switch (ComboFFT.SelectedIndex)
            {
                case 1:
                    fftSize = FftSize.Fft128;
                    break;
                case 2:
                    fftSize = FftSize.Fft256;
                    break;
                case 3:
                    fftSize = FftSize.Fft512;
                    break;
                case 4:
                    fftSize = FftSize.Fft1024;
                    break;
                case 5:
                    fftSize = FftSize.Fft2048;
                    break;
                case 6:
                    fftSize = FftSize.Fft4096;
                    break;
                case 7:
                    fftSize = FftSize.Fft8192;
                    break;
                case 8:
                    fftSize = FftSize.Fft16384;
                    break;
                default:
                    fftSize = FftSize.Fft64;
                    break;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            var fftBuffer = new float[(int)fftSize];
            int[] values = new int[8];
            double max_value = .5;

            _DeepBassSpectrum.UpdateFrequencyMapping();
            _DeepBassSpectrum.SpectrumProvider.GetFftData(fftBuffer, _DeepBassSpectrum);
            var db = _DeepBassSpectrum.CalculateSpectrumPoints(max_value, fftBuffer);
            Mode.Colors[0] = LEDBulb.Subtract(db[0].Value, LEDBulb.ColorPropertyType.Saturation, original_colors[0]);
            values[0] = (int)Math.Floor(db[0].Value);

            _MidBassSpectrum.UpdateFrequencyMapping();
            _MidBassSpectrum.SpectrumProvider.GetFftData(fftBuffer, _MidBassSpectrum);
            var mb = _MidBassSpectrum.CalculateSpectrumPoints(max_value, fftBuffer);
            Mode.Colors[1] = LEDBulb.Subtract(mb[1].Value, LEDBulb.ColorPropertyType.Saturation, original_colors[1]);
            values[1] = (int)Math.Floor(mb[1].Value);

            _UpperBassSpectrum.UpdateFrequencyMapping();
            _UpperBassSpectrum.SpectrumProvider.GetFftData(fftBuffer, _UpperBassSpectrum);
            var ub = _MidBassSpectrum.CalculateSpectrumPoints(max_value, fftBuffer);
            Mode.Colors[2] = LEDBulb.Subtract(ub[1].Value, LEDBulb.ColorPropertyType.Saturation, original_colors[2]);
            values[2] = (int)Math.Floor(ub[1].Value);

            _DeepMidSpectrum.UpdateFrequencyMapping();
            _DeepMidSpectrum.SpectrumProvider.GetFftData(fftBuffer, _DeepMidSpectrum);
            var dm = _DeepMidSpectrum.CalculateSpectrumPoints(max_value, fftBuffer);
            Mode.Colors[3] = LEDBulb.Subtract(dm[1].Value, LEDBulb.ColorPropertyType.Saturation, original_colors[3]);
            values[3] = (int)Math.Floor(dm[1].Value);

            _MidMidSpectrum.UpdateFrequencyMapping();
            _MidMidSpectrum.SpectrumProvider.GetFftData(fftBuffer, _MidMidSpectrum);
            var mm = _MidMidSpectrum.CalculateSpectrumPoints(max_value, fftBuffer);
            Mode.Colors[4] = LEDBulb.Subtract(mm[1].Value, LEDBulb.ColorPropertyType.Saturation, original_colors[4]);
            values[4] = (int)Math.Floor(mm[1].Value);

            _UpperMidSpectrum.UpdateFrequencyMapping();
            _UpperMidSpectrum.SpectrumProvider.GetFftData(fftBuffer, _UpperMidSpectrum);
            var um = _UpperMidSpectrum.CalculateSpectrumPoints(max_value, fftBuffer);
            Mode.Colors[5] = LEDBulb.Subtract(um[1].Value, LEDBulb.ColorPropertyType.Saturation, original_colors[5]);
            values[5] = (int)Math.Floor(um[1].Value);

            _HighSpectrum.UpdateFrequencyMapping();
            _HighSpectrum.SpectrumProvider.GetFftData(fftBuffer, _HighSpectrum);
            var h = _HighSpectrum.CalculateSpectrumPoints(max_value, fftBuffer);
            Mode.Colors[6] = LEDBulb.Subtract(h[1].Value, LEDBulb.ColorPropertyType.Saturation, original_colors[6]);
            values[6] = (int)Math.Floor(h[1].Value);

            _UltraHighSpectrum.UpdateFrequencyMapping();
            _UltraHighSpectrum.SpectrumProvider.GetFftData(fftBuffer, _UltraHighSpectrum);
            var uh = _UltraHighSpectrum.CalculateSpectrumPoints(max_value, fftBuffer);
            Mode.Colors[7] = LEDBulb.Subtract(uh[2].Value, LEDBulb.ColorPropertyType.Saturation, original_colors[7]);
            values[7] = (int)Math.Floor(uh[1].Value);

            Console.WriteLine("db {0}, mb{1}, hb{2}", db[0].Value, mb[1].Value, ub[1].Value );
            Console.WriteLine("dm {0}, mm {1}, um{2}", dm[1].Value, mm[1].Value, um[1].Value);
            Console.WriteLine("h {0}, uh {1}", h[1].Value, uh[2].Value);

            Mode.Music(values);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SelectData();
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
            //create a spectrum provider which provides fft data based on some input
            var spectrumProvider = new BasicSpectrumProvider(aSampleSource.WaveFormat.Channels, aSampleSource.WaveFormat.SampleRate, fftSize);

            //linespectrum and voiceprint3dspectrum used for rendering some fft data
            //in oder to get some fft data, set the previously created spectrumprovider 
            _DeepBassSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = App.settings.MusicUseAverages,
                PointCount = App.settings.MusicPointCount + 1,
                IsXLogScale = App.settings.MusicUseLog,
                ScalingStrategy = strategy,
                MaximumFrequency = 40,
                MinimumFrequency = 20,
            };

            _MidBassSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = App.settings.MusicUseAverages,
                PointCount = App.settings.MusicPointCount + 1,
                IsXLogScale = App.settings.MusicUseLog,
                ScalingStrategy = strategy,
                MaximumFrequency = 160,
                MinimumFrequency = 40,
            };

            _UpperBassSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = App.settings.MusicUseAverages,
                PointCount = App.settings.MusicPointCount + 1,
                IsXLogScale = App.settings.MusicUseLog,
                ScalingStrategy = strategy,
                MaximumFrequency = 300,
                MinimumFrequency = 160,
            };

            _DeepMidSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = App.settings.MusicUseAverages,
                PointCount = App.settings.MusicPointCount + 1,
                IsXLogScale = App.settings.MusicUseLog,
                ScalingStrategy = strategy,
                MaximumFrequency = 800,
                MinimumFrequency = 300,
            };

            _MidMidSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = App.settings.MusicUseAverages,
                PointCount = App.settings.MusicPointCount + 1,
                IsXLogScale = App.settings.MusicUseLog,
                ScalingStrategy = strategy,
                MaximumFrequency = 2500,
                MinimumFrequency = 800,
            };

            _UpperMidSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = App.settings.MusicUseAverages,
                PointCount = App.settings.MusicPointCount + 1,
                IsXLogScale = App.settings.MusicUseLog,
                ScalingStrategy = strategy,
                MaximumFrequency = 5000,
                MinimumFrequency = 2500,
            };

            _HighSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = App.settings.MusicUseAverages,
                PointCount = App.settings.MusicPointCount + 1,
                IsXLogScale = App.settings.MusicUseLog,
                ScalingStrategy = strategy,
                MaximumFrequency = 10000,
                MinimumFrequency = 5000,
            };

            _UltraHighSpectrum = new VoicePrint3DSpectrum(fftSize)
            {
                SpectrumProvider = spectrumProvider,
                UseAverage = App.settings.MusicUseAverages,
                PointCount = App.settings.MusicPointCount + 1,
                IsXLogScale = App.settings.MusicUseLog,
                ScalingStrategy = strategy,
                MaximumFrequency = 20000,
                MinimumFrequency = 10000,
            };

            // The SingleBlockNotificationStream is used to intercept the played samples
            var notificationSource = new SingleBlockNotificationStream(aSampleSource);
            // Pass the intercepted samples as input data to the spectrumprovider (which will calculate a fft based on them)
            notificationSource.SingleBlockRead += (s, a) => spectrumProvider.Add(a.Left, a.Right);
            _source = notificationSource.ToWaveSource(24);
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
