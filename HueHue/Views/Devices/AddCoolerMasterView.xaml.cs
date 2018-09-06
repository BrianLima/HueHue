using HueHue.Helpers.Devices;
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
    /// Interaction logic for AddCoolerMasterView.xaml
    /// </summary>
    public partial class AddCoolerMasterView : UserControl
    {
        public AddCoolerMasterView()
        {
            InitializeComponent();

            ComboBox_SubType.ItemsSource = CoolerMaster.GetCoolerMasterDevices();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.devices.Add(new CoolerMaster("mouse", Device.SubType.Mouse, "MMouse"));
            App.devices[1].Start();
        }

        private void ComboBox_SubType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
