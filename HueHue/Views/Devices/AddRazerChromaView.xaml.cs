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
    /// Interaction logic for AddRazerChromaView.xaml
    /// </summary>
    public partial class AddRazerChromaView : UserControl
    {
        public AddRazerChromaView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.devices.Add(new RazerChroma("Chroma Keyboard", "Keyboard"));

            var main = App.Current.MainWindow as MainWindow;

            main.DisplaySnackbar("Razer Chroma Device Added!");

            App.SaveDevices();

            //Find the main navigation frame on the MainWindow, go back because the user has finished adding this device
            var MainFrame = main.FindName("frame") as Frame;
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }
    }
}
