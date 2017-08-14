using HueHue.Helpers;
using System.Drawing;
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

            //Check the current effect
            if (settings.CurrentMode == 1)
            {
                colorPicker2.Visibility = Visibility.Visible;
                colorZone.Content = "Alternate Colors";
            }
            else
            {
                colorPicker2.Visibility = Visibility.Collapsed;
                colorZone.Content = "Fixed Color";
            }

            //I couldn't in ANY way make it bind the color properly, at least this works
            //Binding each value from the RGB is broken
            //Binding the color it self conflicts because the controler uses System.Windows.Media.Color instead of System.Drawing.Color
            //I give up, this is it, MVVM for a later day.
            colorPicker.SelectedColor =System.Windows.Media.Color.FromArgb( settings.ColorOne.A, settings.ColorOne.R, settings.ColorOne.G, settings.ColorOne.B);

            colorPicker2.SelectedColor = System.Windows.Media.Color.FromArgb(settings.ColorTwo.A, settings.ColorTwo.R, settings.ColorTwo.G, settings.ColorTwo.B);
        }

        private void colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            Effects.ColorOne.R = e.NewValue.Value.R;
            Effects.ColorOne.G = e.NewValue.Value.G;
            Effects.ColorOne.B = e.NewValue.Value.B;

            settings.ColorOne = Color.FromArgb(e.NewValue.Value.R, e.NewValue.Value.G, e.NewValue.Value.B);

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

            settings.ColorTwo = Color.FromArgb(e.NewValue.Value.B, e.NewValue.Value.G, e.NewValue.Value.R);

            FillColor();
        }
    }
}
