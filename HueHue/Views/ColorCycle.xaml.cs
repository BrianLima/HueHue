using HueHue.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for ColorCycle.xaml
    /// </summary>
    public partial class ColorCycle : UserControl
    {
        DispatcherTimer timer;
        AppSettings settings;

        public ColorCycle(AppSettings _settings)
        {
            InitializeComponent();

            gridSettings.DataContext = settings;

            //Starts with Red
            Effects.ColorOne.R = 255;
            Effects.ColorOne.G = 0;
            Effects.ColorOne.B = 0;

            Effects.ResetStep();

            settings = _settings;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(settings.Speed);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Effects.ColorCycle();
        }

        private void Grid_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void sliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Interval = TimeSpan.FromMilliseconds(e.NewValue);
                timer.Start();
            }
        }
    }
}