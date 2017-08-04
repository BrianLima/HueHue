using HueHue.Helpers;
using System.Windows;
using System.Windows.Controls;

namespace HueHue
{
    /// <summary>
    /// Interaction logic for FixedColors.xaml
    /// </summary>
    public partial class FixedColors : UserControl
    {
        AppSettings settings;

        public FixedColors(AppSettings _settings)
        {
            InitializeComponent();
            settings = _settings;
            GridFixedColorSettings.DataContext = settings;

            //Check the current effect
            if (Properties.Settings.Default.CurrentMode == 1)
            {
                colorPicker2.Visibility = Visibility.Visible;
                labelRandom.Visibility = Visibility.Collapsed;
                randomToggle.Visibility = Visibility.Collapsed;
                colorZone.Content = "Alternate Colors";
            }
            else
            {
                colorPicker2.Visibility = Visibility.Collapsed;
                labelRandom.Visibility = Visibility.Visible;
                randomToggle.Visibility = Visibility.Visible;
                colorZone.Content = "Fixed Color";
            }
            //colorPicker.DataContext = Effects.ColorOne; TODO: Bind to a converter maybe?
        }

        private void colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            Effects.ColorOne.B = e.NewValue.Value.B;
            Effects.ColorOne.G = e.NewValue.Value.G;
            Effects.ColorOne.R = e.NewValue.Value.R;

            FillColor();
        }

        private void FillColor()
        {
            if (settings.CurrentMode == 0)
            {
                Effects.FixedColor();
            }
            else
            {
                Effects.TwoAlternateColor();
            }
        }

        private void colorPicker2_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            Effects.ColorTwo.B = e.NewValue.Value.B;
            Effects.ColorTwo.G = e.NewValue.Value.G;
            Effects.ColorTwo.R = e.NewValue.Value.R;

            FillColor();
        }
    }
}
