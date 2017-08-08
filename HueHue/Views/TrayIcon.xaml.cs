using Hardcodet.Wpf.TaskbarNotification;
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
using System.Windows.Shapes;

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
            App.Current.MainWindow.Close();
        }
    }
}
