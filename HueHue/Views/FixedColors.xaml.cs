using HueHue.Helpers;
using System.Windows;
using System.Windows.Controls;
using Media = System.Windows.Media;
using ColorTools;
using System.Windows.Threading;
using System.Threading.Tasks;
using HueHue.Helpers.Modes;

namespace HueHue
{
    /// <summary>
    /// Interaction logic for FixedColors.xaml
    /// </summary>
    public partial class FixedColors : UserControl
    {
        public FixedColors()
        {
            InitializeComponent();

            for (int i = 0; i < Mode.Colors.Count; i++)
            {
                ColorControlPanel panel = new ColorControlPanel();
                panel.Margin = new Thickness(10);

                //I couldn't in ANY way make it bind the color properly, at least this works
                //Binding each value from the RGB is broken
                //Binding the color it self conflicts because the controler uses System.Windows.Media.Color instead of System.Drawing.Color
                //I give up, this is it, MVVM for a later day.

                panel.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Mode.Colors[i].R, Mode.Colors[i].G, Mode.Colors[i].B));
                panel.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Mode.Colors[i].R, Mode.Colors[i].G, Mode.Colors[i].B));
                panel.DockAlphaVisibility = Visibility.Hidden;
                panel.Style = (Style)FindResource("StyleColorControlPanel");
                panel.ColorChanged += colorPicker_ColorChanged;
                panel.LostMouseCapture += Panel_LostMouseCapture;
                panel.LostKeyboardFocus += Panel_LostKeyboardFocus;

                ContextMenu context = new ContextMenu();
                MenuItem item = new MenuItem();
                item.Header = "Remove";
                item.Click += Item_Click;
                context.Items.Add(item);

                panel.ContextMenu = context;
                StackColors.Children.Add(panel);
            }

            Mode.FixedColor();
        }



        private void Panel_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            App.settings.Colors = Mode.Colors;
        }

        private void Panel_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            App.settings.Colors = Mode.Colors;
        }

        private void Panel_LostFocus(object sender, RoutedEventArgs e)
        {
            App.settings.Colors = Mode.Colors;
        }

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            if (StackColors.Children.Count > 1)
            {
                //Get the parent of the menuitem
                MenuItem menuItem = sender as MenuItem;
                ColorControlPanel color = null;
                if (menuItem != null)
                {
                    color = ((ContextMenu)menuItem.Parent).PlacementTarget as ColorControlPanel;
                }

                //Get it's index in the stack
                int index = StackColors.Children.IndexOf(color as UIElement);

                //Remove the color and the stack
                Mode.Colors.RemoveAt(index);
                StackColors.Children.Remove(color);
            }
        }

        private void colorPicker_ColorChanged(object sender, ColorTools.ColorControlPanel.ColorChangedEventArgs e)
        {
            int index = StackColors.Children.IndexOf(sender as UIElement);

            Mode.Colors[index] = new LEDBulb(e.CurrentColor.R, e.CurrentColor.G, e.CurrentColor.B);


            Mode.FixedColor();
        }

        private void Button_Add_Color_Click(object sender, RoutedEventArgs e)
        {
            Mode.Colors.Add(new LEDBulb(255, 0, 0));

            ColorControlPanel panel = new ColorControlPanel();
            panel.Margin = new Thickness(10);

            int i = Mode.Colors.Count - 1;

            //I couldn't in ANY way make it bind the color properly, at least this works
            //Binding each value from the RGB is broken
            //Binding the color it self conflicts because the controler uses System.Windows.Media.Color instead of System.Drawing.Color
            //I give up, this is it, MVVM for a later day.
            panel.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Mode.Colors[i].R, Mode.Colors[i].G, Mode.Colors[i].B));
            panel.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Mode.Colors[i].R, Mode.Colors[i].G, Mode.Colors[i].B));
            panel.DockAlphaVisibility = Visibility.Hidden;
            panel.Style = (Style)FindResource("StyleColorControlPanel");
            panel.ColorChanged += colorPicker_ColorChanged;
            panel.LostMouseCapture += Panel_LostMouseCapture;
            panel.LostKeyboardFocus += Panel_LostKeyboardFocus;


            ContextMenu context = new ContextMenu();
            MenuItem item = new MenuItem();
            item.Header = "Remove";
            item.Click += Item_Click;
            context.Items.Add(item);

            panel.ContextMenu = context;

            StackColors.Children.Add(panel);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.settings.Colors = Mode.Colors;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
