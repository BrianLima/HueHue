using HueHue.Helpers.Devices;
using RGB.NET.Devices.Razer;
using System;
using System.Windows;
using System.Windows.Controls;
using static HueHue.Helpers.Devices.Device;

namespace HueHue.Views.Devices
{
    /// <summary>
    /// Interaction logic for AddRazerChromaView.xaml
    /// </summary>
    public partial class AddRazerChromaView : UserControl
    {
        SubType Subtype;

        public AddRazerChromaView()
        {
            InitializeComponent();

            if (!RazerDeviceProvider.Instance.IsInitialized)
            {
                try
                {
                    RazerDeviceProvider.Instance.Initialize();
                }
                catch (Exception)
                {
                    var main = App.Current.MainWindow as MainWindow;

                    main.DisplaySnackbar("Install the Razer Chroma SDK before continuing!");
                }

            }

            ComboBox_SubType.SelectedIndex = 0;
        }

        private void GetSubType()
        {
            switch (ComboBox_SubType.SelectedIndex)
            {
                case 0:
                    Subtype = RazerChroma.SubType.Keyboard;
                    break;
                case 1:
                    Subtype = RazerChroma.SubType.Mouse;
                    break;
                case 2:
                    Subtype = RazerChroma.SubType.Headset;
                    break;
                case 3:
                    Subtype = RazerChroma.SubType.Mousepad;
                    break;
                case 4:
                    Subtype = RazerChroma.SubType.Keypad;
                    break;
                default:
                    Subtype = RazerChroma.SubType.All;
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetSubType();

            App.devices.Add(new RazerChroma("Chroma", Subtype, TextBoxName.Text));

            var main = App.Current.MainWindow as MainWindow;

            main.DisplaySnackbar(String.Format("Razer Chroma {0} Added!", Subtype.ToString()));

            App.SaveDevices();

            //Find the main navigation frame on the MainWindow, go back because the user has finished adding this device
            var MainFrame = main.FindName("frame") as Frame;
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

        private void ComboBox_SubType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetSubType();
            TextBoxName.Text = "Chroma " + Subtype.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://developer.razerzone.com/works-with-chroma/download/");
        }
    }
}
