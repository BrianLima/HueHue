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
        public TrayIcon()
        {
            InitializeComponent();
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
            Effects.FixedColor();
        }
    }
}
