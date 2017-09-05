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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Drawing = System.Drawing;
using Media = System.Windows.Media;


namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for SnakeMode.xaml
    /// </summary>
    public partial class SnakeMode : UserControl
    {
        static DispatcherTimer timer;

        public SnakeMode()
        {
            InitializeComponent();

            GridSnakeColorSettings.DataContext = App.settings;

            timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(App.settings.Speed)
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            //I couldn't in ANY way make it bind the color properly, at least this works
            //Binding each value from the RGB is broken
            //Binding the color it self conflicts because the controler uses System.Windows.Media.Color instead of System.Drawing.Color
            //I give up, this is it, MVVM for a later day.
            colorPicker.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(App.settings.ColorOne.A, App.settings.ColorOne.R, App.settings.ColorOne.G, App.settings.ColorOne.B));
            colorPicker.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(App.settings.ColorOne.A, App.settings.ColorOne.R, App.settings.ColorOne.G, App.settings.ColorOne.B));

            colorPicker2.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(App.settings.ColorTwo.A, App.settings.ColorTwo.R, App.settings.ColorTwo.G, App.settings.ColorTwo.B));
            colorPicker2.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(App.settings.ColorTwo.A, App.settings.ColorTwo.R, App.settings.ColorTwo.G, App.settings.ColorTwo.B));
        }


        private void colorPicker_ColorChanged(object sender, ColorTools.ColorControlPanel.ColorChangedEventArgs e)
        {
            Effects.ColorOne = (LEDBulb)e.CurrentColor;
            App.settings.ColorOne = (Drawing.Color)Effects.ColorOne;
        }

        private void colorPicker2_ColorChanged(object sender, ColorTools.ColorControlPanel.ColorChangedEventArgs e)
        {
            Effects.ColorTwo = (LEDBulb)e.CurrentColor;
            App.settings.ColorTwo = (Drawing.Color)Effects.ColorTwo;
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
        private void Timer_Tick(object sender, EventArgs e)
        {
            Effects.Snake(App.settings.Length);
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
