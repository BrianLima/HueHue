using HueHue.Helpers.Modes;
using System.Windows.Controls;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for AddButtonToBrightness.xaml
    /// </summary>
    public partial class ButtonBrightnessPicker : UserControl
    {
        public ButtonBrightnessPicker(JoystickButtonToColor joystickButtonToColor)
        {
            InitializeComponent();

            gridMain.DataContext = joystickButtonToColor;
        }
    }
}
