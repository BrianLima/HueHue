namespace HueHue.Helpers
{
    /// <summary>
    /// Generic class representing a device supported by HueHue
    /// </summary>
    public partial class Device
    {
        private string _type;
        /// <summary>
        /// Type of the device, ie: Arduino, Aura Device, RazerChroma Device, CUE device
        /// </summary>
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _name;
        /// <summary>
        /// Name the user has given to the device
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _icon;
        /// <summary>
        /// Path of the icon that represents the Device
        /// </summary>
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        /// <summary>
        /// The Device MUST implement a Start procedure which begins communication
        /// </summary>
        public virtual void Start() { }
        
        /// <summary>
        /// The Device MUST implement a Stop procedure which stops communication 
        /// </summary>
        public virtual void Stop() { }
    }
}
