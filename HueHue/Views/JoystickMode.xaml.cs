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
using SharpDX.DirectInput;
using System.Windows.Threading;
using HueHue.Helpers;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using ColorTools;
using Media = System.Windows.Media;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for JoystickMode.xaml
    /// </summary>
    public partial class JoystickMode : UserControl
    {
        DispatcherTimer timer;
        ObservableCollection<JoystickButtonToColor> buttonsToColors;
        List<JoystickButtonToColor> pressedButtons;
        List<Guid> guids;
        Joystick joystick;
        JoystickHelper joystickHelper;

        public JoystickMode()
        {
            InitializeComponent();

            Application.Current.Exit += Current_Exit;

            gridSettings.DataContext = App.settings;

            timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };
            timer.Tick += Timer_Tick;

            joystickHelper = new JoystickHelper();
            buttonsToColors = new ObservableCollection<JoystickButtonToColor>();
            pressedButtons = new List<JoystickButtonToColor>();

            RefreshJoysticks();

            for (int i = 0; i < guids.Count; i++)
            {
                if (guids[i].ToString() == App.settings.JoystickSelected)
                {
                    combo_joysticks.SelectedIndex = i;
                }
            }

            DefaultColor.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Effects.Colors[0].R, Effects.Colors[0].G, Effects.Colors[0].B));
            DefaultColor.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Effects.Colors[0].R, Effects.Colors[0].G, Effects.Colors[0].B));
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            timer.Stop();
            joystickHelper.SaveJoystickButtons(buttonsToColors.ToList(), guids[combo_joysticks.SelectedIndex]);
        }

        private void ColorPanel_ColorChanged(object sender, ColorTools.ColorControlPanel.ColorChangedEventArgs e)
        {
            int index = StackColors.Children.IndexOf(((Grid)(((ColorControlPanel)sender).Parent)).Parent as UIElement);

            ((ButtonToColor)StackColors.Children[index]).rectangle.Background = new SolidColorBrush(Color.FromRgb(e.CurrentColor.R, e.CurrentColor.G, e.CurrentColor.B));

            buttonsToColors[index].Color = (LEDBulb)e.CurrentColor;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (joystick == null || !this.IsLoaded)
            {
                return;
            }

            joystick.Poll();

            joystick.GetCurrentState();
            var datas = joystick.GetBufferedData();

            foreach (var state in datas)
            {
                JoystickButtonToColor Pressed = buttonsToColors.FirstOrDefault(x => x.Button == state.Offset);
                if (Pressed != null)
                {
                    if (Pressed.ButtonType == JoystickButtonToColor.ButtonTypeEnum.Color)
                    {
                        if (state.Value > 0)
                        {
                            pressedButtons.Add(Pressed);
                        }
                        else
                        {
                            pressedButtons.Remove(Pressed);
                        }
                    }
                    else
                    {
                        if (state.Value != 32511) //Guitar strum bar is centered
                        {
                            App.settings.Brightness = Pressed.PressedBrightness;
                        }
                        else
                        {
                            App.settings.Brightness = Pressed.ReleasedBrightness;
                        }
                    }
                }
            }

            if (pressedButtons.Count == 0 && App.settings.JoystickUseDefault > 0)
            {
                Effects.FixedColor();
            }
            else
            {
                Effects.JoystickMode(pressedButtons, App.settings.Length);
            }
        }

        private void combo_joysticks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (guids.Count <= 0 || combo_joysticks.SelectedIndex < 0)
            {
                return;
            }

            timer.Stop();

            buttonsToColors = new ObservableCollection<JoystickButtonToColor>();

            if (joystick != null)
            {
                joystick.Unacquire();
                joystick.Dispose();
            }

            App.settings.JoystickSelected = guids[combo_joysticks.SelectedIndex].ToString();
            App.settings.Save();

            HookSelectedJoystick();

            foreach (var item in joystickHelper.LoadJoystickButtons(guids[combo_joysticks.SelectedIndex]))
            {
                buttonsToColors.Add(item);
            }

            for (int i = 0; i < buttonsToColors.Count; i++)
            {
                var item = buttonsToColors[i];
                if (item.ButtonType == JoystickButtonToColor.ButtonTypeEnum.Color)
                {
                    var panel = new ButtonToColor(item);
                    panel.colorPanel.ColorChanged += ColorPanel_ColorChanged;
                    ContextMenu context = new ContextMenu();
                    MenuItem menu = new MenuItem();
                    menu.Header = "Remove";
                    menu.Click += Item_Click;
                    context.Items.Add(menu);
                    menu.Uid = i.ToString();
                    panel.ContextMenu = context;

                    StackColors.Children.Add(panel);
                }
                else
                {
                    var panel = new ButtonToBrightness(item);

                    ContextMenu context = new ContextMenu();
                    MenuItem menu = new MenuItem();
                    menu.Header = "Remove";
                    menu.Click += Item_Click;
                    context.Items.Add(menu);
                    menu.Uid = i.ToString();
                    panel.ContextMenu = context;
                    StackColors.Children.Add(panel);
                }
            }

            timer.Start();
        }

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            //Using the Uid to store the index of this item on the StackPanel
            //Might no be a good practice, but it prevents the need of a lot of ((type)sender).Parent
            int index = int.Parse(((MenuItem)sender).Uid);

            StackColors.Children.RemoveAt(index);
            buttonsToColors.RemoveAt(index);

            joystickHelper.SaveJoystickButtons(buttonsToColors.ToList(), guids[combo_joysticks.SelectedIndex]);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RefreshJoysticks();
        }

        private void RefreshJoysticks()
        {
            guids = joystickHelper.GetGuids();
            combo_joysticks.ItemsSource = joystickHelper.GetJoystickNames(guids);
        }

        private void HookSelectedJoystick()
        {
            if (combo_joysticks.SelectedIndex < 0)
            {
                return;
            }

            joystick = joystickHelper.HookJoystick(guids[combo_joysticks.SelectedIndex]);
        }

        private async void Button_AddButtonColor_Click(object sender, RoutedEventArgs e)
        {
            var view = new AddButton(guids[combo_joysticks.SelectedIndex], joystickHelper, JoystickButtonToColor.ButtonTypeEnum.Color);
            var newButton = await DialogHost.Show(view);
            var x = (JoystickButtonToColor)newButton;
            x.ButtonType = JoystickButtonToColor.ButtonTypeEnum.Color; //TODO: REMOVE POG
            buttonsToColors.Add(x);
            var panel = new ButtonToColor(buttonsToColors[buttonsToColors.Count - 1]);
            panel.colorPanel.ColorChanged += ColorPanel_ColorChanged;
            panel.colorPanel.MouseLeave += ColorPanel_MouseLeave;

            ContextMenu context = new ContextMenu();
            MenuItem menu = new MenuItem();
            menu.Header = "Remove";
            menu.Click += Item_Click;
            menu.Uid = (buttonsToColors.Count - 1).ToString();
            context.Items.Add(menu);
            panel.ContextMenu = context;

            StackColors.Children.Add(panel);

            joystickHelper.SaveJoystickButtons(buttonsToColors.ToList(), guids[combo_joysticks.SelectedIndex]);
        }

        private void ColorPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            joystickHelper.SaveJoystickButtons(buttonsToColors.ToList(), guids[combo_joysticks.SelectedIndex]);
        }

        private void combo_MultipleButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 1)
            {
                sliderSection.Visibility = Visibility.Visible;
                textSection.Visibility = Visibility.Visible;
            }
            else
            {
                sliderSection.Visibility = Visibility.Collapsed;
                textSection.Visibility = Visibility.Collapsed;
            }
        }

        private async void Button_AddButtonBrightness_Click(object sender, RoutedEventArgs e)
        {
            var view = new AddButton(guids[combo_joysticks.SelectedIndex], joystickHelper, JoystickButtonToColor.ButtonTypeEnum.Color);
            var newButton = await DialogHost.Show(view);
            var x = (JoystickButtonToColor)newButton;
            x.ButtonType = JoystickButtonToColor.ButtonTypeEnum.Brightness; //TODO: REMOVE POG
            buttonsToColors.Add(x);
            var panel = new ButtonToBrightness(buttonsToColors[buttonsToColors.Count - 1]);
            ContextMenu context = new ContextMenu();
            MenuItem menu = new MenuItem();
            menu.Header = "Remove";
            menu.Click += Item_Click;
            context.Items.Add(menu);
            panel.ContextMenu = context;

            StackColors.Children.Add(panel);
        }

        private void combo_UseDefault_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > 0)
            {
                gridDefaultColor.Visibility = Visibility.Visible;
            }
            else
            {
                gridDefaultColor.Visibility = Visibility.Collapsed;
            }
        }

        private void ColorControlPanel_ColorChanged(object sender, ColorControlPanel.ColorChangedEventArgs e)
        {
            if (Effects.Colors.Count == 0)
            {
                return;
            }

            Effects.Colors[0] = (LEDBulb)e.CurrentColor;

            Effects.FixedColor();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            joystickHelper.SaveJoystickButtons(buttonsToColors.ToList(), guids[combo_joysticks.SelectedIndex]);
        }
    }
}
