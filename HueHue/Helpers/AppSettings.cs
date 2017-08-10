using System.ComponentModel;

namespace HueHue.Helpers
{
    public class AppSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public AppSettings()
        {
            this._com_port = Properties.Settings.Default.COM_PORT;
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
                    Properties.Settings.Default.Save();
                    break;
                case "Brightness":
                    Properties.Settings.Default.Brightness = _brightness;
                    Properties.Settings.Default.Save();
                    break;
                case "TotalLeds":
                    Properties.Settings.Default.TotalLeds = _total_leds;
                    Properties.Settings.Default.Save();
                    break;
                case "COMPort":
                    Properties.Settings.Default.COM_PORT = _com_port;
                    Properties.Settings.Default.Save();
                    break;
                case "Breath":
                    Properties.Settings.Default.Breath = _breath;
                    Properties.Settings.Default.Save();
                    break;
                case "Random":
                    Properties.Settings.Default.Random = _random;
                    Properties.Settings.Default.Save();
                    break;
                case "Speed":
                    Properties.Settings.Default.Speed = _speed;
                    Properties.Settings.Default.Save();
                    break;
                case "Length":
                    Properties.Settings.Default.Length = _length;
                    Properties.Settings.Default.Save();
                    break;
                case "AutoStart":
                    Properties.Settings.Default.AutoStart = _auto_start;
                    Properties.Settings.Default.Save();
                    break;
                case "Minimize":
                    Properties.Settings.Default.Minimize = _minimize;
                    Properties.Settings.Default.Save();
                    break;
                case "AutoRun":
                    Properties.Settings.Default.AutoRun = _auto_run;
                    Properties.Settings.Default.Save();
                    break;
                default:
                    break;
            }
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

        private string _com_port;
        /// <summary>
        /// Gets or sets the current com_port
        /// </summary>
        public string COMPort
        {
            get { return _com_port; }
            set { _com_port = value; OnPropertyChanged("COMPort"); }
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

    }
}
