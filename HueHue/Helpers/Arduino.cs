using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueHue.Helpers
{
    class Arduino : SerialStream
    {
        /// <summary>
        /// Represents an Arduino running HueHueClient connected to the user's PC
        /// </summary>
        /// <param name="_COM_PORT">COM Port which the arduino is connected</param>
        /// <param name="_COM_PORT">Name for the device on the list</param>

        public Arduino(string _COM_PORT, string _Name)
        {
            this.Type = "Arduino";
            this.Name = _Name;
            this.Icon = "/HueHue;component/Icons/Devices/Arduino.png";
            COM_PORT = _COM_PORT;
        }
    }
}
