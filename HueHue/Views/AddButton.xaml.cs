using HueHue.Helpers;
using MaterialDesignThemes.Wpf;
using SharpDX.DirectInput;
using System;
using System.Windows.Controls;
using System.Windows.Threading;
using static HueHue.Helpers.JoystickButtonToColor;

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
        ButtonTypeEnum buttonType;

        public AddButton(Guid _guid, JoystickHelper helper, ButtonTypeEnum _buttonType)
        {
            InitializeComponent();

            joystick = helper.HookJoystick(_guid);
            buttonType = _buttonType;

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
                    if (x.Offset != JoystickOffset.Buttons4 && x.Value > 0)
                    {
                        buttonColor = new JoystickButtonToColor { Button = x.Offset, Color = new LEDBulb(), ButtonType = buttonType, PressedBrightness = 64, ReleasedBrightness = 255 };
                        timer.Stop();
                        DialogHost.CloseDialogCommand.Execute(buttonColor, null);
                    }
                }
            }

            firstRun = false;
        }

        private void UserControl_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
        }
    }
}