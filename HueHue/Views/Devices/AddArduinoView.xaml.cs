using HueHue.Helpers;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HueHue.Views.Devices
{
    /// <summary>
    /// Interaction logic for AddArduinoView.xaml
    /// </summary>
    public partial class AddArduinoView : UserControl
    {
        public AddArduinoView()
        {
            InitializeComponent();
            ComboBox_ports.ItemsSource = SerialStream.GetPorts();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
