using HueHue.Helpers;
using System.Windows;
using System.Windows.Controls;
using Media = System.Windows.Media;
using ColorTools;
using System.Windows.Threading;
using System.Threading.Tasks;
using RGB.NET.Core;
using HueHue.Helpers.Modes;

namespace HueHue
{
    /// <summary>
    /// Interaction logic for FixedColors.xaml
    /// </summary>
    public partial class FixedColors : UserControl
    {
        static DispatcherTimer timer;

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

                panel.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, (byte)Mode.Colors[i].R, (byte)Mode.Colors[i].G, (byte)Mode.Colors[i].B));
                panel.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, (byte)Mode.Colors[i].R, (byte)Mode.Colors[i].G, (byte)Mode.Colors[i].B));
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

            FillColor();

            timer = new DispatcherTimer() { Interval = new System.TimeSpan(App.settings.Speed) };
            timer.Tick += Timer_Tick;
        }

        private async void Timer_Tick(object sender, System.EventArgs e)
        {
            if (timer != null && this.IsLoaded)
            {
                await Task.Run(() => Mode.ShiftLeft());
            }
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

            Mode.Colors[index] = new RGB.NET.Core.Color(e.CurrentColor.R, e.CurrentColor.G, e.CurrentColor.B);


            FillColor();
        }

        private void FillColor()
        {
            //if (App.settings.CurrentMode == 0)
            {
                Mode.FixedColor();
            }
            //else
            {
                //Mode.TwoAlternateColor();
            }
        }

        private void Button_Add_Color_Click(object sender, RoutedEventArgs e)
        {
            Mode.Colors.Add(new Color(255, 0, 0));

            ColorControlPanel panel = new ColorControlPanel();
            panel.Margin = new Thickness(10);

            int i = Mode.Colors.Count - 1;

            //I couldn't in ANY way make it bind the color properly, at least this works
            //Binding each value from the RGB is broken
            //Binding the color it self conflicts because the controler uses System.Windows.Media.Color instead of System.Drawing.Color
            //I give up, this is it, MVVM for a later day.
            panel.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, (byte)Mode.Colors[i].R, (byte)Mode.Colors[i].G, (byte)Mode.Colors[i].B));
            panel.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, (byte)Mode.Colors[i].R, (byte)Mode.Colors[i].G, (byte)Mode.Colors[i].B));
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
            //So i added this button as i can't find any event that gets galled EVERY TIME the user leaves the control
            //I added some events so that we avoid losing the color the user setted if the app is force quited tho
            App.settings.Colors = Mode.Colors;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow != null)
            {
                if (Application.Current.MainWindow.WindowState != WindowState.Minimized)
                {
                    timer.Stop();
                }
            }
            else
            {
                timer.Stop();
            }
        }
    }
}
