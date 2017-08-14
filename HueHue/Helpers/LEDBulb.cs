using System;
using System.Drawing;

namespace HueHue.Helpers
{
    public class LEDBulb
    {
        public LEDBulb()
        {
            this.r = 255;
            this.g = 255;
            this.b = 255;
        }

        public LEDBulb(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
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

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public static explicit operator LEDBulb(Color v)
        {
            return new LEDBulb(v.R, v.G, v.B);
        }
    }
}
