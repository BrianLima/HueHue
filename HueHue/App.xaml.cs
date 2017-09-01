using HueHue.Helpers;
using HueHue.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace HueHue
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static AppSettings settings;
        public static ObservableCollection<Device> devices;
        public static bool isRunning { get; private set; }
        public static TrayIcon icon;
        public static PaletteHelper helper;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            settings = new AppSettings();
            helper = new PaletteHelper();

            devices = new ObservableCollection<Device> { new Arduino(settings.COMPort, "Arduino") };

            Effects.Setup(App.settings.TotalLeds);
            Effects.ColorOne = (LEDBulb)App.settings.ColorOne;
            Effects.ColorTwo = (LEDBulb)App.settings.ColorTwo;

            MainWindow window = new MainWindow();
            icon = new TrayIcon(window);

            window.Show();

            if (App.settings.DarkMode)
            {
                App.helper.SetLightDark(App.settings.DarkMode);
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            settings.Save();

            foreach (Device device in devices)
            {
                device.Stop();
            }
            icon.Close();
        }

        public static void StartDevices()
        {
            isRunning = true;
            try
            {
                foreach (Device device in App.devices)
                {
                    device.Start();
                }
            }
            catch (Exception)
            {
            }
        }

        public static void StopDevices()
        {
            isRunning = false;
            try
            {
                foreach (Device device in App.devices)
                {
                    device.Stop();
                }
            }
            catch (Exception)
            {
            }
        }

        public static void StartStopDevices()
        {
            if (!isRunning)
            {
                StartDevices();
            }
            else
            {
                StopDevices();
            }

            icon.UpdateTrayLabel();
        }
    }
}
