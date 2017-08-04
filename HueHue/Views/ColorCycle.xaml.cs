using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using HueHue.Helpers;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for ColorCycle.xaml
    /// </summary>
    public partial class ColorCycle : UserControl
    {
        enum Status
        {
            IncreasingR,
            IncreasingG,
            IncreasingB,
            Decreasing,
        };

        DispatcherTimer timer;
        AppSettings settings;
        Status status;

        public ColorCycle(AppSettings _settings)
        {
            InitializeComponent();

            status = Status.IncreasingG;

            //Starts with Red
            Effects.ColorOne.R = 255;
            Effects.ColorOne.G = 0;
            Effects.ColorOne.B = 0;

            settings = _settings;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(settings.Speed);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Effects.ColorOne.R == 255)
            {
                if (true)
                { 

                }
            }
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