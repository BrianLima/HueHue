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
using System.Drawing;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HueHue
{
    /// <summary>
    /// Interaction logic for FixedColors.xaml
    /// </summary>
    public partial class FixedColors : UserControl
    {
        List<LEDBulb> leds;
        int countPreviousColors;

        public FixedColors(List<LEDBulb> _leds)
        {
            InitializeComponent();
            this.leds = _leds;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ApplyColor(object sender, RoutedEventArgs e)
        {
            foreach (var LEDBulb in leds)
            {
                LEDBulb.B = colorPicker.B;
                LEDBulb.G = colorPicker.G;
                LEDBulb.R = colorPicker.R;
            }
        }

        private void colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
          //Todo: Implement a color history view
        }
    }
}
