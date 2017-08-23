using HueHue.Helpers;
using HueHue.Views;
using HueHue.Views.Devices;
using System;
using System.Windows;

namespace HueHue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            GridMain.DataContext = App.settings;

            //The app was auto started by windows from the user's startup folder
            if (Environment.GetCommandLineArgs() != null)
            {
                if (App.settings.AutoStart && Environment.GetCommandLineArgs().Length > 1)
                {
                    this.Minimize();
                    App.StartStopDevices();
                }
            }

            if (App.settings.DarkMode)
            {
                App.helper.SetLightDark(App.settings.DarkMode);
            }

            ListDevices.ItemsSource = App.devices;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.StartStopDevices();
            if (App.isRunning)
            {
                buttonStart.Content = "Stop";
            }
            else
            {
                buttonStart.Content = "Start";
            }
        }

        private void comboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    frame.Navigate(new FixedColors());
                    break;
                case 1:
                    frame.Navigate(new FixedColors());
                    break;
                case 2:
                    frame.Navigate(new MusicMode());
                    break;
                case 3:
                    frame.Navigate(new ColorCycle());
                    break;
                case 4:
                    frame.Navigate(new SnakeMode());
                    break;
                case 5:
                    //Effects.ShutOff(); Breath?
                    break;
                case 6:
                    frame.NavigationService.RemoveBackEntry();
                    frame.Content = "LED's currently shut off";
                    Effects.ShutOff();
                    break;
                default:
                    break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (App.settings.Minimize)
            {
                Minimize();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void Window_StateChanged(object sender, System.EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Minimize();
            }
        }

        private void Minimize()
        {
            Hide();
            App.icon.ShowStandardBalloon();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Stop the communication with the arduino, it might cause problems if some settings are changed while it's running
            SettingsWindow window = new SettingsWindow();
            window.ShowDialog();
            buttonStart.Content = "Start";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AddDeviceView view = new AddDeviceView();
            view.Show();
        }
    }
}
