using HueHue.Helpers.Modes;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for RainbowMode.xaml
    /// </summary>
    public partial class RainbowMode : UserControl
    {
        public RainbowMode()
        {
            InitializeComponent();
            GridRainbow.DataContext = App.settings;
        }


        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void sliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Mode.CalcRainbow();
        }

        private void sliders_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Mode.CalcRainbow();
        }
    }
}
