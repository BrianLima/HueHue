using HueHue.Helpers;
using HueHue.Views;
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
        TrayIcon icon;

        public MainWindow()
        {
            InitializeComponent();

            settings = new AppSettings();
            stream = new SerialStream();
            comboBox_ComPort.ItemsSource = stream.GetPorts();

            GridMain.DataContext = settings;

            for (int i = 0; i < settings.TotalLeds; i++)
            {
                Effects.LEDs.Add(new LEDBulb());
            }

            icon = new TrayIcon();
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
                    frame.Navigate(new FixedColors(settings));
                    break;
                case 1:
                    frame.Navigate(new FixedColors(settings));
                    break;
                case 2:
                    frame.Navigate(new MusicMode());
                    break;
                case 3:
                    frame.Navigate(new ColorCycle(settings));
                    break;
                case 4:
                    frame.Navigate(new SnakeMode(settings));
                    break;

                default:

                    break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (settings.Minimize)
            {
                Minimize();
            }
            else
            {
                icon.Close();
                stream.Stop();
            }
        }

        private void Window_StateChanged(object sender, System.EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Minimize();
            }
            else
            {
            }
        }

        private void Minimize()
        {
            Hide();
            icon.ShowStandardBalloon();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow(settings);
            window.ShowDialog();
        }
    }
}
