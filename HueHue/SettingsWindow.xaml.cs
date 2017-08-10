using HueHue.Helpers;
using IWshRuntimeLibrary;
using System;
using System.Windows;

namespace HueHue
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(AppSettings _settings)
        {
            InitializeComponent();
            grid_settings.DataContext = _settings;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Chip_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://twitter.com/brianostorm");
        }

        private void Chip1_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://github.com/brianlima");
        }

        private void Chip2_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=9YPV3FHEFRAUQ");
        }

        private void SetAutoStart()
        {
            try
            {
                string currentApp = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HueHue" + ".exe");
                WshShell shell = new WshShell();
                WshShortcut shortcut = (WshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\HueHue.lnk");
                shortcut.TargetPath = currentApp;
                shortcut.IconLocation = currentApp + ",0";
                shortcut.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                shortcut.Arguments = "autostart";
                shortcut.Save();
            }
            catch (Exception e)
            {
                throw new Exception("Error setting to auto start" + Environment.NewLine + e.Message);
            }
        }

        private void language_toggle_Checked(object sender, RoutedEventArgs e)
        {
            //When checked, let's create a .lnk on the user's Windows startup folder
            if (!System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\HueHue.lnk"))
            {
                SetAutoStart();
            }
        }

        private void language_toggle_Unchecked(object sender, RoutedEventArgs e)
        {
            //If the shortcut exists, delete it
            var shortcut = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\HueHue.lnk";
            if (System.IO.File.Exists(shortcut))
            {
                System.IO.File.Delete(shortcut);
            }
        }
    }
}
