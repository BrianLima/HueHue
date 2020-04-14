using HueHue.Helpers;
using HueHue.Helpers.Modes;
using HueHue.Helpers.Devices;
using HueHue.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using MaterialDesignColors;

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
        public static bool autoStarted = false;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            settings = new AppSettings();
            helper = new PaletteHelper();
            devices = new ObservableCollection<Device>();

            foreach (var device in settings.Devices)
            {
                devices.Add(device);
            }

            foreach (var item in settings.Colors)
            {
                Mode.Colors.Add(item);
            }

            Mode.SwitchCurrentMode();

            icon = new TrayIcon();

            MainWindow window = new MainWindow();

            this.MainWindow = window;

            //The app was auto started by windows from the user's startup folder
            var startArg = Environment.GetCommandLineArgs();

            if (startArg != null && App.settings.AutoStart)
            {
                foreach (var arg in startArg)
                {
                    if (arg.Contains("autostart"))
                    {
                        autoStarted = true;
                        window.Hide();
                        App.StartStopDevices();
                        break;
                    }
                }
            }

            if (App.settings.DarkMode)
            {
                // TODO: Implement a proper color handler here, allowing users to customize the color to their liking!
                // We should have a base helper class for this tho
                BaseTheme baseTheme;
                if (App.settings.DarkMode)
                {
                    baseTheme = BaseTheme.Dark;
                }
                else { baseTheme = BaseTheme.Light; }

                var theme = Theme.Create(baseTheme.GetBaseTheme(),
                SwatchHelper.Lookup[(MaterialDesignColor)PrimaryColor.DeepPurple],
                SwatchHelper.Lookup[(MaterialDesignColor)SecondaryColor.Lime]);

                App.helper.SetTheme(theme);
            }

            if (!autoStarted)
            {
                window.Show();
            }
            else
            {
                window.Hide();
            }

            settings.SaveDevices();
        }



        private void Application_Exit(object sender, ExitEventArgs e)
        {
            SaveDevices();

            settings.Colors = Mode.Colors;

            settings.Save();

            foreach (Device device in devices)
            {
                device.Stop();
            }

            icon.Close();
        }
        public static void SaveDevices()
        {
            var store = new List<Device>();

            foreach (var item in devices)
            {
                store.Add(item);
            }

            settings.Devices = store;
        }

        private static void StartDevices()
        {
            isRunning = true;
            try
            {
                foreach (var device in App.devices)
                {
                    device.Start();
                }
            }
            catch (Exception)
            {
            }
        }

        private static void StopDevices()
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
