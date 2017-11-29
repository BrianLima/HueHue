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
        ObservableCollection<JoystickButtonToColor> listButtonsToColors;
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

            listButtonsToColors = new ObservableCollection<JoystickButtonToColor>();

            for (int i = 0; i < 5; i++)
            {

            }

            foreach (var item in listButtonsToColors)
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

            listButtonsToColors[index].Color = (LEDBulb)e.CurrentColor;
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
<<<<<<< HEAD
                JoystickButtonToColor PressedColor = (JoystickButtonToColor)listButtonsToColors.Select(x => x.Button == state.Offset);
                if (PressedColor != null)
=======
                JoystickButtonToColor Pressed = buttonsToColors.FirstOrDefault(x => x.Button == state.Offset);
                if (Pressed != null)
>>>>>>> 73f4b49500b0ef76ee99aba3f03da0c42cdb381a
                {
                    if (Pressed.ButtonType == JoystickButtonToColor.ButtonTypeEnum.Color)
                    {
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

            Effects.FixedColor();
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
<<<<<<< HEAD
            var newButton = await DialogHost.Show(new AddButton(guids[combo_joysticks.SelectedIndex], joystickHelper, dialogHost));
            listButtonsToColors.Add((JoystickButtonToColor)newButton);
            StackColors.Children.Add(new ButtonToColor(listButtonsToColors[listButtonsToColors.Count - 1]));
        }

        private void dialogHost_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {

=======
            var view = new AddButton(guids[combo_joysticks.SelectedIndex], joystickHelper, JoystickButtonToColor.ButtonTypeEnum.Color);
            var newButton = await DialogHost.Show(view);
            var x = (JoystickButtonToColor)newButton;
            x.ButtonType = JoystickButtonToColor.ButtonTypeEnum.Color; //TODO: REMOVE POG
            buttonsToColors.Add(x);
            var panel = new ButtonToColor(buttonsToColors[buttonsToColors.Count - 1]);
            panel.colorPanel.ColorChanged += ColorPanel_ColorChanged;
            StackColors.Children.Add(panel);
>>>>>>> 73f4b49500b0ef76ee99aba3f03da0c42cdb381a
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
