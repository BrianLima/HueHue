using HueHue.Helpers;
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

            timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(App.settings.Speed)
            };

            timer.Tick += Timer_Tick;
            timer.Start();

            GridRainbow.DataContext = App.settings;

            Effects.CalcRainbow(App.settings.Length, App.settings.Saturation / 10, App.settings.Lightness / 10);
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (timer != null && this.IsLoaded)
            {
                await Task.Run(() => Effects.ShiftRight());
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

                Effects.CalcRainbow(App.settings.Length, App.settings.Saturation / 10, App.settings.Lightness /10);
            }
        }

        private void sliders_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (timer != null && this.IsLoaded)
            {
                Effects.CalcRainbow(App.settings.Length, App.settings.Saturation / 10, App.settings.Lightness / 10);
            }
        }
    }
}
