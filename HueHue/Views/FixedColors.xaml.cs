using HueHue.Helpers;
using System.Windows;
using System.Windows.Controls;
using Drawing = System.Drawing;
using Media = System.Windows.Media;

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

            //Check the current effect
            if (App.settings.CurrentMode == 1)
            {
                colorPicker2.Visibility = Visibility.Visible;
                colorZone.Content = "Alternate Colors";
            }
            else
            {
                colorPicker2.Visibility = Visibility.Hidden;
                colorZone.Content = "Fixed Color";
            }

            //I couldn't in ANY way make it bind the color properly, at least this works
            //Binding each value from the RGB is broken
            //Binding the color it self conflicts because the controler uses System.Windows.Media.Color instead of System.Drawing.Color
            //I give up, this is it, MVVM for a later day.
            colorPicker.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(App.settings.ColorOne.A, App.settings.ColorOne.R, App.settings.ColorOne.G, App.settings.ColorOne.B));
            colorPicker.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(App.settings.ColorOne.A, App.settings.ColorOne.R, App.settings.ColorOne.G, App.settings.ColorOne.B));

            colorPicker2.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(App.settings.ColorTwo.A, App.settings.ColorTwo.R, App.settings.ColorTwo.G, App.settings.ColorTwo.B));
            colorPicker2.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(App.settings.ColorTwo.A, App.settings.ColorTwo.R, App.settings.ColorTwo.G, App.settings.ColorTwo.B));
        }

        private void colorPicker_ColorChanged(object sender, ColorTools.ColorControlPanel.ColorChangedEventArgs e)
        {
            Effects.ColorOne = (LEDBulb)e.CurrentColor;
            App.settings.ColorOne = (Drawing.Color)Effects.ColorOne;

            FillColor();
        }

        private void colorPicker2_ColorChanged(object sender, ColorTools.ColorControlPanel.ColorChangedEventArgs e)
        {
            Effects.ColorTwo = (LEDBulb)e.CurrentColor;
            App.settings.ColorTwo = (Drawing.Color)Effects.ColorTwo;

            FillColor();
        }

        private void FillColor()
        {
            if (App.settings.CurrentMode == 0)
            {
                Effects.FixedColor();
            }
            else
            {
                Effects.TwoAlternateColor();
            }
        }
    }
}
