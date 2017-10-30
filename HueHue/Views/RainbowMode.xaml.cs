using HueHue.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for RainbowMode.xaml
    /// </summary>
    public partial class RainbowMode : UserControl
    {
        static DispatcherTimer timer;

        public RainbowMode()
        {
            InitializeComponent();

            timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(App.settings.Speed)
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            GridRainbow.DataContext = App.settings;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Effects.ShiftRight();

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow != null)
            {
                if (Application.Current.MainWindow.WindowState != WindowState.Minimized)
                {
                    timer.Stop();
                }
            }
            else
            {
                timer.Stop();
            }
        }

        private void sliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Interval = TimeSpan.FromMilliseconds(e.NewValue);
                timer.Start();
            }
            Effects.CalcRainbow(App.settings.Center, App.settings.Width,
                App.settings.FrequencyR / 10, App.settings.FrequencyG / 10, App.settings.FrequencyB / 10,
                App.settings.PhaseR, App.settings.PhaseG, App.settings.PhaseB);

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ComboBox)sender).SelectedIndex)
            {
                case 1: //Sets Rainbow effect to normal rainbow
                    App.settings.FrequencyR = 0.3M;
                    App.settings.FrequencyG = 0.3M;
                    App.settings.FrequencyB = 0.3M;
                    App.settings.PhaseR = 0;
                    App.settings.PhaseG = 2;
                    App.settings.PhaseB = 4;
                    App.settings.Center = 127;
                    App.settings.Width = 128;
                    break;
                case 2: //Sets Rainbow effect to Pastel colors
                    App.settings.FrequencyR = 0.3M;
                    App.settings.FrequencyG = 0.3M;
                    App.settings.FrequencyB = 0.3M;
                    App.settings.PhaseR = 0;
                    App.settings.PhaseG = 2;
                    App.settings.PhaseB = 4;
                    App.settings.Center = 230;
                    App.settings.Width = 25;
                    break;
                default: //Do nothing
                    break;
            }
        }

        private void sliderSpeed_Copy7_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Effects.CalcRainbow(App.settings.Center, App.settings.Width,
                                App.settings.FrequencyR / 10, App.settings.FrequencyG / 10, App.settings.FrequencyB / 10,
                                App.settings.PhaseR, App.settings.PhaseG, App.settings.PhaseB);

        }
    }
}
