using HueHue.Helpers;
using HueHue.Views;
using HueHue.Views.Devices;
using System;
using System.Windows;
using System.Threading.Tasks;

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
            var startArg = Environment.GetCommandLineArgs();
            if (startArg != null && App.settings.AutoStart)
            {
                if (startArg.Length > 1)
                {
                    if (startArg[1].ToString().Contains("autostart"))
                    {
                        System.Windows.Forms.MessageBox.Show("autostart");
                        Minimize();
                        App.StartDevices();
                        buttonStart.Content = "Stop";
                    }
                    else
                    {
                        this.Show();
                    }
                }
                else
                {
                    this.Show();
                }
            }
            else
            {
                this.Show();
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
            NavigateModes();
        }

        public void NavigateModes()
        {
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    frame.Navigate(new FixedColors());
                    break;
                case 1:
                    frame.Navigate(new MusicMode());
                    break;
                case 2:
                    frame.Navigate(new ColorCycle());
                    break;
                case 3:
                    frame.Navigate(new SnakeMode());
                    break;
                case 4:
                    //Effects.ShutOff(); Breath?
                    break;
                case 5:
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
                e.Cancel = true;

                Minimize();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void Window_StateChanged(object sender, System.EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                Minimize();
            }
        }

        private void Minimize()
        {
            this.Hide();
            if (App.icon != null)
            {
                App.icon.ShowStandardBalloon();
            }
        }

        private void Button_ShowSettings_Click(object sender, RoutedEventArgs e)
        {
            //Stop the communication with the arduino, it might cause problems if some settings are changed while it's running
            App.StopDevices();
            SettingsWindow window = new SettingsWindow();
            window.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
        }

        private void Button_AddArduino_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new AddArduinoView());
        }

        public void DisplaySnackbar()
        {
            //use the message queue to send a message.
            var messageQueue = snackBar.MessageQueue;
            var message = "Device added";

            //the message queue can be called from any thread
            Task.Factory.StartNew(() => messageQueue.Enqueue(message));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (ListDevices.SelectedIndex < 0)
            {
                return;
            }
            App.devices.RemoveAt(ListDevices.SelectedIndex);

            App.SaveDevices();
        }
    }
}
