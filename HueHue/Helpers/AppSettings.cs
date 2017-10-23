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
                default:
                    break;
            }
        }

        /// <summary>
        /// Serializes all the devices the user setup to a JSON file
        /// </summary>
        private void SaveDevices()
        {
            var json = JsonConvert.SerializeObject(_devices);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/Devices.json", json);
        }

        /// <summary>
        /// Serializes all the colors the user has set to a JSON file
        /// </summary>
        private void SaveColors()
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
    }
}