using System;
//using Spectrum;
using RGB.NET.Core;

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
            //Generating a random color via the HSL colorspace and then convert it to
            //Tradicional RGB, this makes the software generate randomized, yet saturated colors
            var BaseColor = Color.FromHSV(random.Next(0, 360), 1, 1);
            this.r = BaseColor.R;
            this.g = BaseColor.G;
            this.b = BaseColor.B;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        public LEDBulb(LEDBulb origin)
        {
            this.r = origin.r;
            this.g = origin.g;
            this.b = origin.b;
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

        public static implicit operator LEDBulb(Color rgb)
        {
            return new LEDBulb(rgb.R, rgb.G, rgb.B);
        }
    }
}