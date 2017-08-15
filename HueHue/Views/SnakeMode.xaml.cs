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

            //I couldn't in ANY way make it bind the color properly, at least this works
            //Binding each value from the RGB is broken
            //Binding the color it self conflicts because the controler uses System.Windows.Media.Color instead of System.Drawing.Color
            //I give up, this is it, MVVM for a later day.
            colorPicker.SelectedColor = System.Windows.Media.Color.FromArgb(settings.ColorOne.A, settings.ColorOne.R, settings.ColorOne.G, settings.ColorOne.B);

            colorPicker2.SelectedColor = System.Windows.Media.Color.FromArgb(settings.ColorTwo.A, settings.ColorTwo.R, settings.ColorTwo.G, settings.ColorTwo.B);
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

            settings.ColorOne = System.Drawing.Color.FromArgb(e.NewValue.Value.R, e.NewValue.Value.G, e.NewValue.Value.B);
        }

        private void colorPicker2_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Effects.ColorTwo.B = e.NewValue.Value.B;
            Effects.ColorTwo.G = e.NewValue.Value.G;
            Effects.ColorTwo.R = e.NewValue.Value.R;

            settings.ColorTwo = System.Drawing.Color.FromArgb(e.NewValue.Value.R, e.NewValue.Value.G, e.NewValue.Value.B);
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
