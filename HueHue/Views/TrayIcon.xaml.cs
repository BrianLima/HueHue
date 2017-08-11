using Hardcodet.Wpf.TaskbarNotification;
using HueHue.Helpers;
using System.Diagnostics;
using System.Windows;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for NotifyIcon.xaml
    /// </summary>
    public partial class TrayIcon : Window
    {
        private AppSettings settings;
        private SerialStream stream;
        private MainWindow mainWindow;

        public TrayIcon(AppSettings _settings, SerialStream _stream, MainWindow _mainWindow)
        {
            InitializeComponent();

            this.settings = _settings;
            this.stream = _stream;
            this.mainWindow = _mainWindow;
        }

        public void ShowStandardBalloon()
        {
            icon.ShowBalloonTip("Hey, HueHue is now running on the system tray", @"If you intended to close the app, right click the tray icon and ""Quit""", BalloonIcon.Info);
        }

        private void icon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Show();
            App.Current.MainWindow.Focus();
        }

        private void MenuItem_Quit_Click(object sender, RoutedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void MenuItem_ShutOff_Click(object sender, RoutedEventArgs e)
        {
            settings.CurrentMode = 6; //Shut off leds
        }

        private void MenuItem_FixedColor_Click(object sender, RoutedEventArgs e)
        {
            settings.CurrentMode = 0;
        }

        private void MenuItem_AlternateColors_Click(object sender, RoutedEventArgs e)
        {
            settings.CurrentMode = 1;
        }

        private void MenuItem_MusicMode_Click(object sender, RoutedEventArgs e)
        {
            settings.CurrentMode = 2;
        }

        private void MenuItem_ColorCycling_Click(object sender, RoutedEventArgs e)
        {
            settings.CurrentMode = 3;
        }

        private void MenuItem_SnakeColor_Click(object sender, RoutedEventArgs e)
        {
            settings.CurrentMode = 4;
        }

        private void MenuItem_StartStop_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.StartStop();

            UpdateTrayLabel();
        }

        public void UpdateTrayLabel()
        {
            if (!stream.IsRunning())
            {
                item_start_stop.Header = "Start";
            }
            else
            {
                item_start_stop.Header = "Stop";
            }
        }
    }
}
