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

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for JoystickMode.xaml
    /// </summary>
    public partial class JoystickMode : UserControl
    {
        DispatcherTimer timer;
        List<Guid> guids;
        Joystick joystick;
        JoystickHelper joystickHelper;

        public JoystickMode()
        {
            InitializeComponent();

            //GetInput();

            timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(App.settings.Speed)
            };
            timer.Tick += Timer_Tick;

            joystickHelper = new JoystickHelper();

            guids = joystickHelper.GetGuids();
            combo_joysticks.ItemsSource = joystickHelper.GetJoystickNames(guids);
            if (combo_joysticks.Items.Count > 0)
            {
                combo_joysticks.SelectedIndex = 0;
            }
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (joystick == null)
            {
                return;
            }

            //LEDBulb color = new LEDBulb();
            joystick.Poll();

            joystick.GetCurrentState();

            var datas = joystick.GetBufferedData();

            foreach (var state in datas)
            {
                string button = state.ToString();

                if (button.Contains("Buttons5"))
                {
                    Effects.Colors[0] = new LEDBulb(0, 255, 0);
                }
                else if (button.Contains("Buttons1"))
                {
                    Effects.Colors[0] = new LEDBulb(255, 0, 0);
                }
                else if (button.Contains("Buttons0"))
                {
                    Effects.Colors[0] = new LEDBulb(255, 255, 0);
                }
                else if (button.Contains("Buttons2"))
                {
                    Effects.Colors[0] = new LEDBulb(0, 0, 255);
                }
                else if (button.Contains("Buttons3"))
                {
                    Effects.Colors[0] = new LEDBulb(255, 128, 0);
                }
            }
            Effects.FixedColor();
        }

        private void combo_joysticks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            timer.Stop();

            if (joystick!= null)
            {
                joystick.Unacquire();
                joystick.Dispose();
            }

            timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            guids = joystickHelper.GetGuids();
            combo_joysticks.ItemsSource = joystickHelper.GetJoystickNames(guids);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            joystick = joystickHelper.HookJoystick(guids[combo_joysticks.SelectedIndex]);
        }
    }
}
