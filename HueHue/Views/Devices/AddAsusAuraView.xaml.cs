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
using RGB.NET.Devices.Asus;

namespace HueHue.Views.Devices
{
    /// <summary>
    /// Interaction logic for AddAsusAuraView.xaml
    /// </summary>
    public partial class AddAsusAuraView : UserControl
    {
        public AddAsusAuraView()
        {
            InitializeComponent();
App.surface.Devices.get
            //AsusDeviceProvider.Instance.Initialize(.)

            //foreach (var item )
            //{
            //
            //}
            //    
            //
            //ComboBox_SubType.Items = RGB.NET.Core.RGBDeviceType;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SubType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
