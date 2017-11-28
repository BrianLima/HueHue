using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace HueHue.Helpers
{
    /// <summary>
    /// Represents and stores/load Properties.Settings.Default, saves colors and devices the user setup onto a JSON file on the root of the directory where the app is
    /// </summary>
    public class AppSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public AppSettings()
        {
            this._current_mode = Properties.Settings.Default.CurrentMode;
            this._total_leds = Properties.Settings.Default.TotalLeds;
            this._brightness = Properties.Settings.Default.Brightness;
            this._random = Properties.Settings.Default.Random;
            this._breath = Properties.Settings.Default.Breath;
            this._speed = Properties.Settings.Default.Speed;
            this._length = Properties.Settings.Default.Length;
            this._auto_start = Properties.Settings.Default.AutoStart;
            this._minimize = Properties.Settings.Default.Minimize;
            this._auto_run = Properties.Settings.Default.AutoRun;
            this._dark_mode = Properties.Settings.Default.DarkMode;
            this._version = Properties.Settings.Default.Version;
            this._frequency_r = Properties.Settings.Default.FrequencyR;
            this._frequency_g = Properties.Settings.Default.FrequencyG;
            this._frequency_b = Properties.Settings.Default.FrequencyB;
            this._phase_r = Properties.Settings.Default.PhaseR;
            this._phase_g = Properties.Settings.Default.PhaseG;
            this._phase_b = Properties.Settings.Default.PhaseB;
            this._center = Properties.Settings.Default.Center;
            this._width = Properties.Settings.Default.Width;
            this._joystick_multiple_buttons = Properties.Settings.Default.JoystickMultipleButtons;
            this._joystick_use_default = Properties.Settings.Default.JoystickUseDefault;

            CheckVersion();

            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Colors.json"))
            {
                File.Create(AppDomain.CurrentDomain.BaseDirectory + "/Colors.json");
                this._colors = new List<LEDBulb>() { new LEDBulb() { R = 255, G = 0, B = 0 } };
            }
            else
            {
                this._colors = JsonConvert.DeserializeObject<List<LEDBulb>>(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Colors.json"));
            }

            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/Devices.json"))
            {
                File.Create(AppDomain.CurrentDomain.BaseDirectory + "/Devices.json");
                this._devices = new List<Device>();
            }
            else
            {
                this._devices = JsonConvert.DeserializeObject<List<Device>>(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Devices.json"));
            }

            //Just in case one of the lists failed to parse and returned null, start a new list to prevent errors
            if (_devices == null)
            {
                _devices = new List<Device>();
            }
            if (_colors == null)
            {
                _colors = new List<LEDBulb>() { new LEDBulb() { R = 255, G = 0, B = 0 } };
            }
        }

        private void CheckVersion()
        {
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            if (version != this.Version)
            {
                Properties.Settings.Default.Upgrade();
                this.Version = version;
                Save();
            }
        }

        public void Save()
        {
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Updates properties
        /// </summary>
        /// <param name="name">Property name</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

            //When a property is changed, let's alter the respective default in settings
            switch (name)
            {
                case "CurrentMode":
                    Properties.Settings.Default.CurrentMode = _current_mode;
                    break;
                case "Brightness":
                    Properties.Settings.Default.Brightness = _brightness;
                    break;
                case "TotalLeds":
                    Properties.Settings.Default.TotalLeds = _total_leds;
                    Effects.Setup(_total_leds);
                    break;
                case "Breath":
                    Properties.Settings.Default.Breath = _breath;
                    break;
                case "Random":
                    Properties.Settings.Default.Random = _random;
                    break;
                case "Speed":
                    Properties.Settings.Default.Speed = _speed;
                    break;
                case "Length":
                    Properties.Settings.Default.Length = _length;
                    break;
                case "AutoStart":
                    Properties.Settings.Default.AutoStart = _auto_start;
                    break;
                case "Minimize":
                    Properties.Settings.Default.Minimize = _minimize;
                    break;
                case "AutoRun":
                    Properties.Settings.Default.AutoRun = _auto_run;
                    break;
                case "Colors":
                    SaveColors();
                    break;
                case "Devices":
                    SaveDevices();
                    break;
                case "DarkMode":
                    Properties.Settings.Default.DarkMode = _dark_mode;
                    break;
                case "Version":
                    Properties.Settings.Default.Version = _version;
                    break;
                case "FrequencyR":
                    Properties.Settings.Default.FrequencyR = _frequency_r;
                    break;
                case "FrequencyG":
                    Properties.Settings.Default.FrequencyG = _frequency_g;
                    break;
                case "FrequencyB":
                    Properties.Settings.Default.FrequencyB = _frequency_b;
                    break;
                case "Center":
                    Properties.Settings.Default.Center = _center;
                    break;
                case "Width":
                    Properties.Settings.Default.Width = _width;
                    break;
                case "JoystickMultipleButtons":
                    Properties.Settings.Default.JoystickMultipleButtons = _joystick_multiple_buttons;
                    break;
                case "JoystickUseDefault":
                    Properties.Settings.Default.JoystickUseDefault = _joystick_use_default;
                    break;
                case "JoystickSelected":
                    Properties.Settings.Default.JoystickSelected = _joystick_selected;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Serializes all the devices the user setup to a JSON file
        /// </summary>
        public void SaveDevices()
        {
            var json = JsonConvert.SerializeObject(_devices);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/Devices.json", json);
        }

        /// <summary>
        /// Serializes all the colors the user has set to a JSON file
        /// </summary>
        public void SaveColors()
        {
            var json = JsonConvert.SerializeObject(_colors);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/Colors.json", json);
        }

        private int _current_mode;
        /// <summary>
        /// Gets or sets the current animation mode
        /// </summary>
        public int CurrentMode
        {
            get { return _current_mode; }
            set { _current_mode = value; OnPropertyChanged("CurrentMode"); }
        }

        private byte _brightness;
        /// <summary>
        /// Gets or sets the brightness
        /// </summary>
        public byte Brightness
        {
            get { return _brightness; }
            set { _brightness = value; OnPropertyChanged("Brightness"); }
        }

        private int _total_leds;
        /// <summary>
        /// Gets or sets the total number of LEDS on the LED Strip
        /// </summary>
        public int TotalLeds
        {
            get { return _total_leds; }
            set { _total_leds = value; OnPropertyChanged("TotalLeds"); }
        }

        private bool _breath;
        /// <summary>
        /// Gets or sets if it's currently in breath mode
        /// </summary>
        public bool Breath
        {
            get { return _breath; }
            set { _breath = value; OnPropertyChanged("Breath"); }
        }

        private bool _random;

        public bool Random
        {
            get { return _random; }
            set { _random = value; OnPropertyChanged("Random"); }
        }

        private int _speed;
        /// <summary>
        /// Gets or sets the speed for some effects like Snake Mode
        /// </summary>
        public int Speed
        {
            get { return _speed; }
            set { _speed = value; OnPropertyChanged("Speed"); }
        }

        private int _length;
        /// <summary>
        /// Gets or sets the width for some effects like Snake Mode
        /// </summary>
        public int Length
        {
            get { return _length; }
            set { _length = value; OnPropertyChanged("Length"); }
        }

        private bool _auto_start;
        /// <summary>
        /// Gets or sets whether the app will auto run on Windows startup or not
        /// </summary>
        public bool AutoStart
        {
            get { return _auto_start; }
            set { _auto_start = value; OnPropertyChanged("AutoStart"); }
        }

        private bool _minimize;
        /// <summary>
        /// Gets or sets whether the app will minimize on close or not
        /// </summary>
        public bool Minimize
        {
            get { return _minimize; }
            set { _minimize = value; OnPropertyChanged("Minimize"); }
        }

        private bool _auto_run;
        /// <summary>
        /// Gets or sets whether the app will change effects on the fly according to running apps
        /// </summary>
        public bool AutoRun
        {
            get { return _auto_run; }
            set { _auto_run = value; OnPropertyChanged("AutoRun"); }
        }

        private List<LEDBulb> _colors;
        /// <summary>
        /// List of colors the user is using
        /// </summary>
        public List<LEDBulb> Colors
        {
            get { return _colors; }
            set { _colors = value; OnPropertyChanged("Colors"); }
        }

        private List<Device> _devices;
        /// <summary>
        /// List of devices the user has
        /// </summary>
        public List<Device> Devices
        {
            get { return _devices; }
            set { _devices = value; OnPropertyChanged("Devices"); }
        }

        private bool _dark_mode;
        /// <summary>
        /// Gets or sets if the app is in dark mode
        /// </summary>
        public bool DarkMode
        {
            get { return _dark_mode; }
            set { _dark_mode = value; OnPropertyChanged("DarkMode"); }
        }

        private string _version;
        /// <summary>
        /// Gets or sets the current version of the app
        /// </summary>
        public string Version
        {
            get { return _version; }
            set { _version = value; OnPropertyChanged("Version"); }
        }

        private decimal _frequency_r;
        /// <summary>
        /// Gets or sets the frequency for Red color
        /// </summary>
        public decimal FrequencyR
        {
            get { return _frequency_r; }
            set { _frequency_r = value; OnPropertyChanged("FrequencyR"); }
        }

        private int _phase_r;
        /// <summary>
        /// Gets or sets the Phase for R
        /// </summary>
        public int PhaseR
        {
            get { return _phase_r; }
            set { _phase_r = value; OnPropertyChanged("PhaseR"); }
        }

        private decimal _frequency_g;
        /// <summary>
        /// Gets or sets the frequency for G
        /// </summary>
        public decimal FrequencyG
        {
            get { return _frequency_g; }
            set { _frequency_g = value; OnPropertyChanged("FrequencyG"); }
        }

        private int _phase_g;
        /// <summary>
        /// Gets or sets the phase for G
        /// </summary>
        public int PhaseG
        {
            get { return _phase_g; }
            set { _phase_g = value; OnPropertyChanged("PhaseG"); }
        }

        private decimal _frequency_b;
        /// <summary>
        /// Gets or sets the frequency for B
        /// </summary>
        public decimal FrequencyB
        {
            get { return _frequency_b; }
            set { _frequency_b = value; OnPropertyChanged("FrequencyB"); }
        }

        private int _phase_b;
        /// <summary>
        /// Gets or sets the phase for B
        /// </summary>
        public int PhaseB
        {
            get { return _phase_b; }
            set { _phase_b = value; OnPropertyChanged("PhaseB"); }
        }

        private int _center;
        /// <summary>
        /// Gets or sets the Center for rainbow effect
        /// </summary>
        public int Center
        {
            get { return _center; }
            set { _center = value; OnPropertyChanged("Center"); }
        }

        private int _width;
        /// <summary>
        /// Gets or sets the Width for Rainbow effects
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; OnPropertyChanged("Width"); }
        }

        private int _joystick_multiple_buttons;
        /// <summary>
        /// Gets or sets what the app will do when multiple buttons are pressed
        /// </summary>
        public int JoystickMultipleButtons
        {
            get { return _joystick_multiple_buttons; }
            set { _joystick_multiple_buttons = value; OnPropertyChanged("JoystickMultipleButtons"); }
        }

        private int _joystick_use_default;
        /// <summary>
        /// Gets or sets if the app should return to a default color when the user releases a button
        /// </summary>
        public int JoystickUseDefault
        {
            get { return _joystick_use_default; }
            set { _joystick_use_default = value; OnPropertyChanged("JoystickUseDefault"); }
        }

        private int _joystick_selected;
        /// <summary>
        /// Gets or sets the selected joystick by the user
        /// </summary>
        public int JoystickSelected
        {
            get { return _joystick_selected; }
            set { _joystick_selected = value; OnPropertyChanged("JoystickSelected"); }
        }
    }
}