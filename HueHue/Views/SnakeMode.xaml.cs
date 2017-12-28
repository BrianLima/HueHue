using HueHue.Helpers;
using RGB.NET.Core;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
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

            //Snake mode has a minimum of two colors for the effect
            while (Effects.Colors.Count < 2)
            {
                Effects.Colors.Add(new Color());
            }

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
            colorPicker.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Effects.Colors[0].R, Effects.Colors[0].G, Effects.Colors[0].B));
            colorPicker.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Effects.Colors[0].R, Effects.Colors[0].G, Effects.Colors[0].B));

            colorPicker2.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Effects.Colors[1].R, Effects.Colors[1].G, Effects.Colors[1].B));
            colorPicker2.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Effects.Colors[1].R, Effects.Colors[1].G, Effects.Colors[1].B));

            Effects.FillSNakeStrip();

            //I think this is a good limit
            sliderWidth.Maximum = Effects.LEDs.Count / 2;
        }

        private void colorPicker_ColorChanged(object sender, ColorTools.ColorControlPanel.ColorChangedEventArgs e)
        {
            if (timer != null && this.IsLoaded)
            {
                Effects.Colors[0] = new Color(e.CurrentColor.R, e.CurrentColor.G, e.CurrentColor.B);
                Effects.FillSNakeStrip();
            }
        }

        private void colorPicker2_ColorChanged(object sender, ColorTools.ColorControlPanel.ColorChangedEventArgs e)
        {
            if (timer != null && this.IsLoaded)
            {
                Effects.Colors[1] = new Color(e.CurrentColor.R, e.CurrentColor.G, e.CurrentColor.B);
                Effects.FillSNakeStrip();
            }
        }

        private void sliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (timer != null && this.IsLoaded)
            {
                timer.Stop();
                timer.Interval = TimeSpan.FromMilliseconds(e.NewValue);
                timer.Start();
            }
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (timer != null && this.IsLoaded)
            {
                await Task.Run(() => Effects.ShiftLeft());
            }
        }

        private void sliderWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (timer != null && this.IsLoaded)
            {
                Effects.FillSNakeStrip();
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
    }
}
