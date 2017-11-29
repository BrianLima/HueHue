using HueHue.Helpers;
using System.Windows.Controls;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for AddButtonToBrightness.xaml
    /// </summary>
    public partial class ButtonToBrightness : UserControl
    {
        public ButtonToBrightness(JoystickButtonToColor joystickButtonToColor)
        {
            InitializeComponent();

            this.DataContext = joystickButtonToColor;
        }
    }
}
