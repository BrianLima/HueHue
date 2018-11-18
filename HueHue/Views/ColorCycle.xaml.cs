using HueHue.Helpers;
using HueHue.Helpers.Modes;
using System;
using System.Text.RegularExpressions;
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
            Mode.Colors[0] = new LEDBulb(255, 0, 0);

            Mode.ResetStep();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(App.settings.Speed);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Mode.ColorCycle();
        }

        private void Grid_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
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

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}