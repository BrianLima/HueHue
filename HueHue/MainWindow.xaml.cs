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
        AppSettings settings;
        SerialStream stream;

        public MainWindow()
        {
            InitializeComponent();

            settings = new AppSettings();
            stream = new SerialStream(settings);

            GridMain.DataContext = settings;

            for (int i = 0; i < settings.TotalLeds; i++)
            {
                stream.LEDS.Add(new LEDBulb());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isRunning)
            {
                isRunning = true;
                buttonStart.Content = "Stop";
                stream.Start();
            }
            else
            {
                isRunning = false;
                buttonStart.Content = "Start";
                stream.Stop();
            }
        }

        private void comboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    frame.Navigate(new FixedColors(stream.LEDS));
                    break;
                default:

                    break;
            }
        }
    }
}
