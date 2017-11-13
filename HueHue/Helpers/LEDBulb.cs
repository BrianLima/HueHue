using System;

namespace HueHue.Helpers
{
    public class LEDBulb
    {
        /// <summary>
        /// Initializes with a new Random color
        /// </summary>
        public LEDBulb()
        {
            Random random = new Random();

            this.r = (byte)random.Next(255);
            this.g = (byte)random.Next(255);
            this.b = (byte)random.Next(255);
        }

        /// <summary>
        /// Initilizes with a determined color
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
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

        public static explicit operator LEDBulb(System.Drawing.Color v)
        {
            return new LEDBulb(v.R, v.G, v.B);
        }

        public static explicit operator LEDBulb(System.Windows.Media.Color v)
        {
            return new LEDBulb(v.R, v.G, v.B);
        }

        public static explicit operator System.Drawing.Color(LEDBulb v)
        {
            return System.Drawing.Color.FromArgb(v.R, v.G, v.B);
        }
    }
}
