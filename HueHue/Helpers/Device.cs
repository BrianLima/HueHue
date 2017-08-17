namespace HueHue.Helpers
{
    public class Device
    {
        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _icon;

        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }
    }
}
