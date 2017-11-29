using HueHue.Helpers;
using System.Windows.Controls;
using Media = System.Windows.Media;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for ButtonToColor.xaml
    /// </summary>
    public partial class ButtonToColor : UserControl
    {
        public ButtonToColor(JoystickButtonToColor buttonColor)
        {
            InitializeComponent();

            this.colorPanel.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, buttonColor.Color.R, buttonColor.Color.G, buttonColor.Color.B));
            this.colorPanel.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, buttonColor.Color.R, buttonColor.Color.G, buttonColor.Color.B));
            this.labelBindedButton.DataContext = buttonColor.Button;
        }
    }
}
