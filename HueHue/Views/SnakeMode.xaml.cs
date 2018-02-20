using HueHue.Helpers;
using HueHue.Helpers.Modes;
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
            while (Mode.Colors.Count < 2)
            {
                Mode.Colors.Add(new Color());
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
            colorPicker.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Mode.Colors[0].R, Mode.Colors[0].G, Mode.Colors[0].B));
            colorPicker.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Mode.Colors[0].R, Mode.Colors[0].G, Mode.Colors[0].B));

            colorPicker2.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Mode.Colors[1].R, Mode.Colors[1].G, Mode.Colors[1].B));
            colorPicker2.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Mode.Colors[1].R, Mode.Colors[1].G, Mode.Colors[1].B));

            Mode.FillSNakeStrip();

            //I think this is a good limit
            sliderWidth.Maximum = Mode.LEDs.Count / 2;
        }

        private void colorPicker_ColorChanged(object sender, ColorTools.ColorControlPanel.ColorChangedEventArgs e)
        {
            if (timer != null && this.IsLoaded)
            {
                Mode.Colors[0] = new Color(e.CurrentColor.R, e.CurrentColor.G, e.CurrentColor.B);
                Mode.FillSNakeStrip();
            }
        }

        private void colorPicker2_ColorChanged(object sender, ColorTools.ColorControlPanel.ColorChangedEventArgs e)
        {
            if (timer != null && this.IsLoaded)
            {
                Mode.Colors[1] = new Color(e.CurrentColor.R, e.CurrentColor.G, e.CurrentColor.B);
                Mode.FillSNakeStrip();
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
                await Task.Run(() => Mode.ShiftLeft());
            }
        }

        private void sliderWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (timer != null && this.IsLoaded)
            {
                Mode.FillSNakeStrip();
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
