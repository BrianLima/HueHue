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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CoolerMaster.GetCoolerMasterDevices();
        }
    }
}
