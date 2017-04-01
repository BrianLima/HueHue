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

        public FixedColors()
        {
            InitializeComponent();
            GridFixedColorSettings.DataContext = settings;
            //colorPicker.DataContext = Effects.ColorOne; TODO: Bind to a converter maybe?
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ApplyColor(object sender, RoutedEventArgs e)
        {
        }

        private void colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            Effects.ColorOne.B = e.NewValue.Value.B;
            Effects.ColorOne.G = e.NewValue.Value.G;
            Effects.ColorOne.R = e.NewValue.Value.R;
        }
    }
}
