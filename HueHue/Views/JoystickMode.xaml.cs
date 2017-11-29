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

            gridSettings.DataContext = App.settings;

            timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(App.settings.Speed)
            };
            timer.Tick += Timer_Tick;

            joystickHelper = new JoystickHelper();

            RefreshJoysticks();

            buttonsToColors = new ObservableCollection<JoystickButtonToColor>();

            for (int i = 0; i < 5; i++)
            {
                buttonsToColors.Add(new JoystickButtonToColor() { Button = JoystickOffset.Buttons0, ButtonType = JoystickButtonToColor.ButtonTypeEnum.Color, Color = new LEDBulb(), });
            }

            foreach (var item in buttonsToColors)
            {
                var panel = new ButtonToColor(item);
                panel.colorPanel.ColorChanged += ColorPanel_ColorChanged;

                StackColors.Children.Add(panel);
            }

            timer.Start();
        }

        private void ColorPanel_ColorChanged(object sender, ColorTools.ColorControlPanel.ColorChangedEventArgs e)
        {
            int index = StackColors.Children.IndexOf(((Grid)(((ColorControlPanel)sender).Parent)).Parent as UIElement);

            ((ButtonToColor)StackColors.Children[index]).rectangle.Fill = new SolidColorBrush(Color.FromRgb(e.CurrentColor.R, e.CurrentColor.G, e.CurrentColor.B));

            buttonsToColors[index].Color = (LEDBulb)e.CurrentColor;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (joystick == null)
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

                        Effects.Colors[0] = Pressed.Color;
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

            Effects.JoystickMode(pressedButtons, 5);
        }

        private void combo_joysticks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            timer.Stop();

            if (joystick != null)
            {
                joystick.Unacquire();
                joystick.Dispose();
            }

            timer.Start();
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
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
            StackColors.Children.Add(panel);
        }

        private void combo_MultipleButtons_Copy_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private async void Button_AddButtonBrightness_Click(object sender, RoutedEventArgs e)
        {
            var view = new AddButton(guids[combo_joysticks.SelectedIndex], joystickHelper, JoystickButtonToColor.ButtonTypeEnum.Color);
            var newButton = await DialogHost.Show(view);
            var x = (JoystickButtonToColor)newButton;
            x.ButtonType = JoystickButtonToColor.ButtonTypeEnum.Brightness; //TODO: REMOVE POG
            buttonsToColors.Add(x);
            var panel = new ButtonToBrightness(buttonsToColors[buttonsToColors.Count - 1]);
            StackColors.Children.Add(panel);
        }
    }
}
