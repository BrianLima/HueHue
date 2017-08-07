using HueHue.Helpers;
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

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for SnakeMode.xaml
    /// </summary>
    public partial class SnakeMode : UserControl
    {
        static DispatcherTimer timer;
        AppSettings settings;

        public SnakeMode(AppSettings _settings)
        {
            InitializeComponent();

            this.settings = _settings;

            GridSnakeColorSettings.DataContext = settings;

            timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(settings.Speed)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Effects.Snake(settings.Length);
        }

        private void colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Effects.ColorOne.B = e.NewValue.Value.B;
            Effects.ColorOne.G = e.NewValue.Value.G;
            Effects.ColorOne.R = e.NewValue.Value.R;
        }

        private void colorPicker2_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Effects.ColorTwo.B = e.NewValue.Value.B;
            Effects.ColorTwo.G = e.NewValue.Value.G;
            Effects.ColorTwo.R = e.NewValue.Value.R;
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

        private void Grid_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void sliderWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Effects.ResetStep();
        }
    }
}
