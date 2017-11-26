using HueHue.Helpers;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using SharpDX.DirectInput;
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
using System.Windows.Threading;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for AddButton.xaml
    /// </summary>
    public partial class AddButton : UserControl
    {
        DialogHost parent;
        Joystick joystick;
        DispatcherTimer timer;
        public ButtonColor buttonColor;
        bool firstRun = true;

        public AddButton(Guid _guid, JoystickHelper helper, DialogHost _parent)
        {
            InitializeComponent();

            this.parent = _parent;
            joystick = helper.HookJoystick(_guid);

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
                        buttonColor = new ButtonColor { Button = x.Offset, Color = new LEDBulb() };

                        DialogHost.CloseDialogCommand.Execute(buttonColor, null);
                    }
                }
            }

            firstRun = false;
        }

        private void UserControl_DialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            //return buttonColor;
        }
    }
}