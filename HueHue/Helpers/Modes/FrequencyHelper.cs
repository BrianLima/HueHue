using static HueHue.Helpers.LEDBulb;

namespace HueHue.Helpers.Modes
{
    class FrequencyHelper
    {
        private string _frequency;

        public string Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }

        private LEDBulb _color;

        public LEDBulb Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private float _threshold;

        public float Threshold
        {
            get { return _threshold; }
            set { _threshold = value; }
        }

        private bool _change_on_threshold;

        public bool ChangeOnThreshold
        {
            get { return _change_on_threshold; }
            set { _change_on_threshold = value; }
        }

        private ColorPropertyType _color_property_type;

        public ColorPropertyType ColorPropertyType
        {
            get { return _color_property_type; }
            set { _color_property_type = value; }
        }

        private bool _enabled;

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }
    }
}
