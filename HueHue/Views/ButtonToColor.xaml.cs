using HueHue.Helpers;
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
using Media = System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for ButtonToColor.xaml
    /// </summary>
    public partial class ButtonToColor : UserControl
    {
        public ButtonToColor(ButtonColor buttonColor)
        {
            InitializeComponent();

            this.colorPanel.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, buttonColor.Color.R, buttonColor.Color.G, buttonColor.Color.B));
            this.colorPanel.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, buttonColor.Color.R, buttonColor.Color.G, buttonColor.Color.B));
            this.labelBindedButton.DataContext = buttonColor.Button;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
