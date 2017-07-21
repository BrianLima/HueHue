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

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for SnakeMode.xaml
    /// </summary>
    public partial class SnakeMode : UserControl
    {
        public SnakeMode(AppSettings settings)
        {
            InitializeComponent();
            GridSnakeColorSettings.DataContext = settings;
        }

        private void colorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Effects.ColorOne.B = e.NewValue.Value.B;
            Effects.ColorOne.G = e.NewValue.Value.G;
            Effects.ColorOne.R = e.NewValue.Value.R;
        }

        private void colorPicker2_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Effects.ColorTwo.B = e.NewValue.Value.B;
            Effects.ColorTwo.G = e.NewValue.Value.G;
            Effects.ColorTwo.R = e.NewValue.Value.R;
        }
    }
}
