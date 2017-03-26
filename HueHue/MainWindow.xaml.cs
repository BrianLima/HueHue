using HueHue.Properties;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HueHue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialStream s = new SerialStream();

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < Settings.Default.AmountOfLEDS; i++)
            {
                s.LEDS.Add(new LEDBulb());
            }

            frame.Navigate(new FixedColors(s.LEDS));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            s.Start();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }
    }
}
