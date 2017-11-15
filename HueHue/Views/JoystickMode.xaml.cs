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
        static DispatcherTimer timer;
        Joystick joystick;

        public JoystickMode()
        {
            InitializeComponent();

            GetInput();

            timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(App.settings.Speed)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //LEDBulb color = new LEDBulb();
            joystick.Poll();
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
                LabelTest.Content = state.Value;
            }
            // = color;
            Effects.FixedColor();
                //Console.WriteLine(state);
        }

        void GetInput()
        {
            // Initialize DirectInput
            var directInput = new DirectInput();

            // Find a Joystick Guid
            var joystickGuid = Guid.Empty;

            //foreach (var deviceInstance in directInput.GetDevices(DeviceType.ControlDevice, DeviceEnumerationFlags.AllDevices))
            //    joystickGuid = deviceInstance.InstanceGuid;

            // If Gamepad not found, look for a Joystick
            if (joystickGuid == Guid.Empty)
                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                {
                    joystickGuid = deviceInstance.InstanceGuid;
                    break;
                }

            // If Joystick not found, throws an error
            if (joystickGuid == Guid.Empty)
            {
                Console.WriteLine("No joystick/Gamepad found.");
                Console.ReadKey();
                Environment.Exit(1);
            }

            // Instantiate the joystick
            joystick = new Joystick(directInput, joystickGuid);

            Console.WriteLine("Found Joystick/Gamepad with GUID: {0}", joystickGuid);

            // Query all suported ForceFeedback effects
            var allEffects = joystick.GetEffects();
            foreach (var effectInfo in allEffects)
                Console.WriteLine("Effect available {0}", effectInfo.Name);

            // Set BufferSize in order to use buffered data.
            joystick.Properties.BufferSize = 128;

            // Acquire the joystick
            joystick.Acquire();

            // Poll events from joystick
            //while (true)
            //{
            //    joystick.Poll();
            //    var datas = joystick.GetBufferedData();
            //    foreach (var state in datas)
            //        Console.Write(state);
            //}
        }
    }
}
