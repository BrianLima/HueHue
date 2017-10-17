using HueHue.Helpers;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;

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

            if (ComboBox_ports.Items.Count > 0)
            {
                ComboBox_ports.SelectedIndex = ComboBox_ports.Items.Count - 1;
            }
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            App.devices.Add(new Arduino(ComboBox_ports.Text, TextBoxName.Text));
            var parent = this.Parent;
            MainWindow.framex.GoBack();
            if (parent != null)
            {
                //if (parent.CanGoBack)
                //{
                //    parent.GoBack();
                //}
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(@"HueHue.Resources.HueHueClient.ino"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    TextBox_script.Text = reader.ReadToEnd();
                }
            }
        }

        private void Button_Clipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TextBox_script.Text);
        }
    }
}
