using ColorTools;
using HueHue.Helpers;
using MaterialDesignThemes.Wpf;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Media = System.Windows.Media;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for JoystickMode.xaml
    /// </summary>
    public partial class JoystickMode : UserControl
    {
        private Thread _workerThread;
        private CancellationTokenSource _cancellationTokenSource;
        bool running = false;
        int selectedjoystick;

        ObservableCollection<JoystickButtonToColor> buttonsToColors;
        List<JoystickButtonToColor> pressedButtons;
        List<Guid> guids;
        JoystickHelper joystickHelper;

        public JoystickMode()
        {
            InitializeComponent();

            Application.Current.Exit += Current_Exit;

            gridSettings.DataContext = App.settings;

            joystickHelper = new JoystickHelper();
            buttonsToColors = new ObservableCollection<JoystickButtonToColor>();
            pressedButtons = new List<JoystickButtonToColor>();

            RefreshJoysticks();

            for (int i = 0; i < guids.Count; i++)
            {
                if (guids[i].ToString() == App.settings.JoystickSelected)
                {
                    combo_joysticks.SelectedIndex = i;
                    Start();
                }
            }

            DefaultColor.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Effects.Colors[0].R, Effects.Colors[0].G, Effects.Colors[0].B));
            DefaultColor.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Effects.Colors[0].R, Effects.Colors[0].G, Effects.Colors[0].B));
        }

        private void Start()
        {
            if (_workerThread != null) return;

            selectedjoystick = combo_joysticks.SelectedIndex;

            _cancellationTokenSource = new CancellationTokenSource();
            _workerThread = new Thread(BackgroundWorker_DoWork)
            {
                Name = "Joystick Hook",
                IsBackground = true
            };
            _workerThread.Start(_cancellationTokenSource.Token);
            running = true;
        }

        public void Stop()
        {
            if (_workerThread == null) return;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
            _workerThread.Join();
            _workerThread = null;
            running = false;
        }

        private void BackgroundWorker_DoWork(object tokenObject)
        {
            var cancellationToken = (CancellationToken)tokenObject;
            Joystick joystick = joystickHelper.HookJoystick(guids[selectedjoystick]); ;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
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

                    Task.Delay(16, cancellationToken).Wait(cancellationToken); //60 FPS
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch
                {

                }
            }
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            Stop();
            if (combo_joysticks.SelectedIndex >= 0)
            {
                joystickHelper.SaveJoystickButtons(buttonsToColors.ToList(), guids[combo_joysticks.SelectedIndex]);
            }
        }

        private void ColorPanel_ColorChanged(object sender, ColorTools.ColorControlPanel.ColorChangedEventArgs e)
        {
            int index = StackColors.Children.IndexOf(((Grid)(((ColorControlPanel)sender).Parent)).Parent as UIElement);

            ((ButtonColorPicker)StackColors.Children[index]).rectangle.Background = new SolidColorBrush(Color.FromRgb(e.CurrentColor.R, e.CurrentColor.G, e.CurrentColor.B));

            buttonsToColors[index].Color = new RGB.NET.Core.Color(e.CurrentColor.R, e.CurrentColor.G, e.CurrentColor.B);
        }

        private void combo_joysticks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (guids.Count <= 0 || combo_joysticks.SelectedIndex < 0)
            {
                return;
            }

            Stop();

            buttonsToColors = new ObservableCollection<JoystickButtonToColor>();

            App.settings.JoystickSelected = guids[combo_joysticks.SelectedIndex].ToString();
            App.settings.Save();

            foreach (JoystickButtonToColor item in joystickHelper.LoadJoystickButtons(guids[combo_joysticks.SelectedIndex]))
            {
                buttonsToColors.Add(item);
            }

            for (int i = 0; i < buttonsToColors.Count; i++)
            {
                var item = buttonsToColors[i];
                if (item.ButtonType == JoystickButtonToColor.ButtonTypeEnum.Color)
                {
                    var panel = new ButtonColorPicker(item);
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
                    var panel = new ButtonBrightnessPicker(item);

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

            Start();
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

        private async void Button_AddButtonColor_Click(object sender, RoutedEventArgs e)
        {
            var view = new AddButton(guids[combo_joysticks.SelectedIndex], joystickHelper, JoystickButtonToColor.ButtonTypeEnum.Color);
            JoystickButtonToColor newButton = (JoystickButtonToColor)await DialogHost.Show(view);

            if (newButton == null)
            {
                return;
            }

            newButton.ButtonType = JoystickButtonToColor.ButtonTypeEnum.Color; //TODO: REMOVE POG
            buttonsToColors.Add(newButton);
            var panel = new ButtonColorPicker(buttonsToColors[buttonsToColors.Count - 1]);
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
            JoystickButtonToColor newButton = (JoystickButtonToColor)await DialogHost.Show(view);

            if (newButton == null)
            {
                return;
            }

            newButton.ButtonType = JoystickButtonToColor.ButtonTypeEnum.Brightness; //TODO: REMOVE POG
            buttonsToColors.Add(newButton);
            var panel = new ButtonBrightnessPicker(buttonsToColors[buttonsToColors.Count - 1]);
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

            Effects.Colors[0] = new RGB.NET.Core.Color(e.CurrentColor.R, e.CurrentColor.G, e.CurrentColor.B);


            Effects.FixedColor();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Stop();
            if (combo_joysticks.SelectedIndex >= 0)
            {
                joystickHelper.SaveJoystickButtons(buttonsToColors.ToList(), guids[combo_joysticks.SelectedIndex]);
            }
        }
    }
}
