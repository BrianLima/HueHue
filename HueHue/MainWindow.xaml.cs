using HueHue.Properties;
using System.Windows;

namespace HueHue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isRunning = false;
        SerialStream s = new SerialStream();

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < Settings.Default.AmountOfLEDS; i++)
            {
                s.LEDS.Add(new LEDBulb());
            }

            switch (Settings.Default.CurrentMode)
            {
                case "FixedColors":
                    frame.Navigate(new FixedColors(s.LEDS));
                    break;
                default:
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isRunning)
            {
                isRunning = true;
                buttonStart.Content = "Stop";
                ///s.Start();
            }
            else
            {
                isRunning = false;
                buttonStart.Content = "Start";
                //s.Stop();
            }
        }

        private void comboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    frame.Navigate(new FixedColors(s.LEDS));
                    break;
                default:

                    break;
            }
        }
    }
}
