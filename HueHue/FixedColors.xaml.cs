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

namespace HueHue
{
    /// <summary>
    /// Interaction logic for FixedColors.xaml
    /// </summary>
    public partial class FixedColors : UserControl
    {
        public FixedColors()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ApplyColor(object sender, RoutedEventArgs e)
        {
            foreach (var LEDBulb in SerialStream.LEDS)
            {
                LEDBulb.B = colorPicker.B;
                LEDBulb.G = colorPicker.G;
                LEDBulb.R = colorPicker.R;
            }
        }
    }
}
