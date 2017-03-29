namespace HueHue
{
    public class LEDBulb
    {
        public LEDBulb()
        {
            this.r = 255;
            this.g = 255;
            this.b = 255;
        }
        private byte r;

        public byte R
        {
            get { return r; }
            set { r = value; }
        }

        private byte g;

        public byte G
        {
            get { return g; }
            set { g = value; }
        }

        private byte b;

        public byte B
        {
            get { return b; }
            set { b = value; }
        }

        private int _id;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
    }
}
