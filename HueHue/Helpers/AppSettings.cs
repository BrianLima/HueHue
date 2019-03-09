using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using HueHue.Helpers.Devices;
using HueHue.Helpers.Modes;

namespace HueHue.Helpers
{
    /// <summary>
    /// Represents and stores/load Properties.Settings.Default, saves colors and devices the user setup onto a JSON file on the root of the directory where the app is
    /// </summary>
    public class AppSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DirectoryInfo AppData = new DirectoryInfo(Environment.ExpandEnvironmentVariables("%appdata%") + @"/Briano/HueHue/");

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
            this._saturation = Properties.Settings.Default.Saturation;
            this._lightness = Properties.Settings.Default.Lightness;
            this._joystick_multiple_buttons = Properties.Settings.Default.JoystickMultipleButtons;
            this._joystick_use_default = Properties.Settings.Default.JoystickUseDefault;
            this._joystick_selected = Properties.Settings.Default.JoystickSelected;
            this._breath_randomize = Properties.Settings.Default.BreathRandomize;
            this._music_use_averages = Properties.Settings.Default.MusicUseAverages;
            this._music_use_log = Properties.Settings.Default.MusicUseLog;
            this._music_scale_strategy = Properties.Settings.Default.MusicScaleStrategy;
            this._music_fft_size = Properties.Settings.Default.MusicFFTSize;
            this._music_point_count = Properties.Settings.Default.MusicPointCount;
            this._music_sample_rate = Properties.Settings.Default.MusicSampleRate;
            this._music_channels = Properties.Settings.Default.MusicChannels;
            this._music_bits = Properties.Settings.Default.MusicBits;
            this._music_device_code = Properties.Settings.Default.MusicDeviceCode;

            CheckVersion();

            if (!Directory.Exists(AppData.FullName))
            {
                Directory.CreateDirectory(AppData.FullName);
            }

            if (!File.Exists(AppData.FullName + "Colors.json"))
            {
                File.Create(AppData.FullName + "Colors.json");
                this._colors = new List<LEDBulb>() { new LEDBulb() { } };
            }
            else
            {
                this._colors = JsonConvert.DeserializeObject<List<LEDBulb>>(File.ReadAllText(AppData.FullName + "Colors.json"), new ColorsJsonConverter());
            }

            if (!File.Exists(AppData.FullName + "Devices.json"))
            {
                File.Create(AppData.FullName + "Devices.json");
                this._devices = new List<Device>();
            }
            else
            {
                this._devices = JsonConvert.DeserializeObject<List<Device>>(File.ReadAllText(AppData.FullName + "Devices.json"));
            }

            //Just in case one of the lists failed to parse and returned null, start a new list to prevent errors
            if (_devices == null)
            {
                _devices = new List<Device>();
            }
            if (_colors == null)
            {
                _colors = new List<LEDBulb>() { new LEDBulb() { } };
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
                    Mode.Setup(_total_leds);
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
                case "Saturation":
                    Properties.Settings.Default.Saturation = _saturation;
                    break;
                case "Lightness":
                    Properties.Settings.Default.Lightness = _lightness;
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
                case "BreathRandomize":
                    Properties.Settings.Default.BreathRandomize = _breath_randomize;
                    break;
                case "MusicUseAverages":
                    Properties.Settings.Default.MusicUseAverages = _music_use_averages;
                    break;
                case "MusicUseLog":
                    Properties.Settings.Default.MusicUseLog = _music_use_log;
                    break;
                case "MusicScaleStrategy":
                    Properties.Settings.Default.MusicScaleStrategy = _music_scale_strategy;
                    break;
                case "MusicFFTSize":
                    Properties.Settings.Default.MusicFFTSize = _music_fft_size;
                    break;
                case "MusicPointCount":
                    Properties.Settings.Default.MusicPointCount = _music_point_count;
                    break;
                case "MusicSampleRate":
                    Properties.Settings.Default.MusicSampleRate = _music_sample_rate;
                    break;
                case "MusicChannels":
                    Properties.Settings.Default.MusicChannels = _music_channels;
                    break;
                case "MusicBits":
                    Properties.Settings.Default.MusicBits = _music_bits;
                    break;
                case "MusicDeviceCode":
                    Properties.Settings.Default.MusicDeviceCode = _music_device_code;
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
            File.WriteAllText(AppData.FullName + "Devices.json", json);
        }

        /// <summary>
        /// Serializes all the colors the user has set to a JSON file
        /// </summary>
        public void SaveColors()
        {
            var json = JsonConvert.SerializeObject(_colors);
            File.WriteAllText(AppData.FullName + "Colors.json", json);
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

        private double _saturation;
        /// <summary>
        /// Gets or sets the Saturation for a effect
        /// </summary>
        public double Saturation
        {
            get { return _saturation; }
            set { _saturation = value; OnPropertyChanged("Saturation"); }
        }

        private double _lightness;
        /// <summary>
        /// Gets or sets the Lightness for a effect
        /// </summary>
        public double Lightness
        {
            get { return _lightness; }
            set { _lightness = value; OnPropertyChanged("Lightness"); }
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

        private string _joystick_selected;
        /// <summary>
        /// Gets or sets the selected joystick by the user
        /// </summary>
        public string JoystickSelected
        {
            get { return _joystick_selected; }
            set { _joystick_selected = value; OnPropertyChanged("JoystickSelected"); }
        }

        private bool _breath_randomize;
        /// <summary>
        /// Gets or sets if every time the brightness fade away, the color should change
        /// </summary>
        public bool BreathRandomize
        {
            get { return _breath_randomize; }
            set { _breath_randomize = value; OnPropertyChanged("BreathRandomize"); }
        }

        private bool _music_use_averages;

        public bool MusicUseAverages
        {
            get { return _music_use_averages; }
            set { _music_use_averages = value; }
        }

        private bool _music_use_log;

        public bool MusicUseLog
        {
            get { return _music_use_log; }
            set { _music_use_log = value; }
        }

        private int _music_scale_strategy;

        public int MusicScaleStrategy
        {
            get { return _music_scale_strategy; }
            set { _music_scale_strategy = value; }
        }

        private int _music_fft_size;

        public int MusicFFTSize
        {
            get { return _music_fft_size; }
            set { _music_fft_size = value; }
        }

        private int _music_point_count;

        public int MusicPointCount
        {
            get { return _music_point_count; }
            set { _music_point_count = value; }
        }

        private int _music_sample_rate;

        public int MusicSampleRate
        {
            get { return _music_sample_rate; }
            set { _music_sample_rate = value; }
        }

        private int _music_channels;

        public int MusicChannels
        {
            get { return _music_channels; }
            set { _music_channels = value; }
        }

        private int _music_bits;

        public int MusicBits
        {
            get { return _music_bits; }
            set { _music_bits = value; }
        }

        private string _music_device_code;

        public string MusicDeviceCode
        {
            get { return _music_device_code; }
            set { _music_device_code = value; }
        }

    }
}