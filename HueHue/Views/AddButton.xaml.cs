using HueHue.Helpers.Modes;
using MaterialDesignThemes.Wpf;
using RGB.NET.Core;
using SharpDX.DirectInput;
using System;
using System.Windows.Controls;
using System.Windows.Threading;
using static HueHue.Helpers.Modes.JoystickButtonToColor;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for AddButton.xaml
    /// </summary>
    public partial class AddButton : UserControl
    {
        Joystick joystick;
        DispatcherTimer timer;
        public JoystickButtonToColor buttonColor;
        bool firstRun = true;
        ControlTypeEnum controlType;

        public AddButton(Guid _guid, JoystickHelper helper, ControlTypeEnum _controlType)
        {
            InitializeComponent();

            joystick = helper.HookJoystick(_guid);
            controlType = _controlType;
            toggle_ignore.DataContext = this;

            timer = new DispatcherTimer() { Interval = new TimeSpan(20) };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            joystick.Poll();

            joystick.GetCurrentState();

            var datas = joystick.GetBufferedData();
            if (datas.Length > 0 && !firstRun)
            {
                foreach (var item in datas)
                {
                    JoystickUpdate x = datas[0];
                    if ((x.Offset != JoystickOffset.Buttons4 && (toggle_ignore.IsChecked ?? false)) && x.Value > 0)
                    {
                        buttonColor = new JoystickButtonToColor() { Button = x.Offset, Color = new Color(), ControlType = controlType, PressedBrightness = 64, CenteredBrightness = 255 };
                        buttonColor.SetMinMaxValues(x.Value);
                        timer.Stop();
                        DialogHost.CloseDialogCommand.Execute(buttonColor, this);
                    }
                }
            }

            firstRun = false;
        }

        private void UserControl_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            GC.Collect();
        }
    }
}