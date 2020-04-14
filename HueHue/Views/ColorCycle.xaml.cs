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
        public ColorCycle()
        {
            InitializeComponent();

            gridSettings.DataContext = App.settings;

            //Starts with Red
            Mode.Colors[0] = new LEDBulb(255, 0, 0);

            Mode.ResetStep();
        }

        private void Grid_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void sliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}