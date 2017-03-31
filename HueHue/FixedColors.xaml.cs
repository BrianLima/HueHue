using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HueHue
{
    /// <summary>
    /// Interaction logic for FixedColors.xaml
    /// </summary>
    public partial class FixedColors : UserControl
    {
        ColorSettings settings = new ColorSettings();
        List<LEDBulb> leds;
        int countPreviousColors;

        public FixedColors(List<LEDBulb> _leds)
        {
            InitializeComponent();
            this.leds = _leds;
            GridFixedColorSettings.DataContext = settings;
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

        private void colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            foreach (var LEDBulb in leds)
            {
                LEDBulb.B = e.NewValue.Value.B;
                LEDBulb.G = e.NewValue.Value.G;
                LEDBulb.R = e.NewValue.Value.R;
            }
        }
    }
}
