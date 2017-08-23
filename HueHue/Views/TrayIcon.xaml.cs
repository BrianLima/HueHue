using Hardcodet.Wpf.TaskbarNotification;
using System.Diagnostics;
using System.Windows;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for NotifyIcon.xaml
    /// </summary>
    public partial class TrayIcon : Window
    {
        private MainWindow mainWindow;

        public TrayIcon(MainWindow _mainWindow)
        {
            InitializeComponent();

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
            App.settings.CurrentMode = 6; //Shut off leds
        }

        private void MenuItem_FixedColor_Click(object sender, RoutedEventArgs e)
        {
            App.settings.CurrentMode = 0;
        }

        private void MenuItem_AlternateColors_Click(object sender, RoutedEventArgs e)
        {
            App.settings.CurrentMode = 1;
        }

        private void MenuItem_MusicMode_Click(object sender, RoutedEventArgs e)
        {
            App.settings.CurrentMode = 2;
        }

        private void MenuItem_ColorCycling_Click(object sender, RoutedEventArgs e)
        {
            App.settings.CurrentMode = 3;
        }

        private void MenuItem_SnakeColor_Click(object sender, RoutedEventArgs e)
        {
            App.settings.CurrentMode = 4;
        }

        private void MenuItem_StartStop_Click(object sender, RoutedEventArgs e)
        {
            App.StartStopDevices();

            UpdateTrayLabel();
        }

        public void UpdateTrayLabel()
        {
            if (!App.isRunning)
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
