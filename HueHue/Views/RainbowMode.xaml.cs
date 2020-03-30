using HueHue.Helpers.Modes;
using System;
using System.Threading.Tasks;
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
            GridRainbow.DataContext = App.settings;

            timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(App.settings.Speed)

            };

            Mode.CalcRainbow(App.settings.Saturation / 10, App.settings.Lightness / 10);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (timer != null && this.IsLoaded)
            {
                await Task.Run(() => Mode.ShiftLeft());
            }
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
            if (timer != null && this.IsLoaded)
            {
                timer.Stop();
                timer.Interval = TimeSpan.FromMilliseconds(e.NewValue);
                timer.Start();

                Mode.CalcRainbow(App.settings.Saturation / 10, App.settings.Lightness /10);
            }
        }

        private void sliders_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (timer != null && this.IsLoaded)
            {
                Mode.CalcRainbow(App.settings.Saturation / 10, App.settings.Lightness / 10);
            }
        }
    }
}
