using System.Windows;

namespace HueHue.Views.Devices
{
    /// <summary>
    /// Interaction logic for AddDeviceView.xaml
    /// </summary>
    public partial class AddDeviceView : Window
    {
        public AddDeviceView()
        {
            InitializeComponent();
            Frame.Navigate(new AddArduinoView());
            
        }
    }
}
