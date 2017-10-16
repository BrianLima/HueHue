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

        public ColorCycle()
        {
            InitializeComponent();

            gridSettings.DataContext = App.settings;

            //Starts with Red
            Effects.Colors[0].R = 255;
            Effects.Colors[0].G = 0;
            Effects.Colors[0].B = 0;

            Effects.ResetStep();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(App.settings.Speed);
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