using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueHue
{
    class AppSettings
    {
        public AppSettings()
        {
            this._brightness = Properties.Settings.Default.Brightness;
            this._com_port = Properties.Settings.Default.COM_PORT;
            this._current_mode = Properties.Settings.Default.CurrentMode;
            this._total_leds = Properties.Settings.Default.TotalLeds;
        }

        private int _current_mode;
        /// <summary>
        /// Gets or sets the current animation mode
        /// </summary>
        public int CurrentMode
        {
            get { return _current_mode; }
            set { _current_mode = value; }
        }

        private byte _brightness;
        /// <summary>
        /// Gets or sets the brightness
        /// </summary>
        public byte Brightness
        {
            get { return _brightness; }
            set { _brightness = value; }
        }

        private int _total_leds;
        /// <summary>
        /// Gets or sets the total number of LEDS on the LED Strip
        /// </summary>
        public int TotalLeds
        {
            get { return _total_leds; }
            set { _total_leds = value; }
        }

        private string _com_port;
        /// <summary>
        /// Gets or sets the current com_port
        /// </summary>
        public string COMPort
        {
            get { return _com_port; }
            set { _com_port = value; }
        }

    }
}
