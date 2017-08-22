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
        public Arduino(string _COM_PORT)
        {
            this.Type = "Arduino";
            this.Name = "Arduino";
            this.Icon = "/HueHue;component/Icons/Devices/Arduino.png";
            COM_PORT = _COM_PORT;
        }
    }
}
