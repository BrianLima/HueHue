using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Spectrum;

namespace HueHue.Helpers
{
    public class LEDBulb
    {
        public enum ColorPropertyType
        {
            Hue,
            Saturation,
            Lightness,
        }

        /// <summary>
        /// Initializes with a new Random color
        /// </summary>
        public LEDBulb()
        {
            var random = new Random(DateTime.Now.Millisecond).Next(1, 360);
            //Generating a random color via the HSL colorspace and then convert it to
            //Tradicional RGB, this makes the software generate randomized, yet saturated colors

            var BaseColor = new Color.HSL(random, 1, 0.5).ToRGB();
            this.R = BaseColor.R;
            this.G = BaseColor.G;
            this.B = BaseColor.B;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        public LEDBulb(LEDBulb origin)
        {
            this.R = origin.R;
            this.G = origin.G;
            this.B = origin.B;
        }

        /// <summary>
        /// Initilizes with a determined color
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public LEDBulb(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        /// <summary>
        /// Rotates a color by n degrees
        /// </summary>
        /// <param name="degree"></param>
        public void AddHue(double degree)
        {
            var rotated = new Color.RGB(this.R, this.G, this.B).ToHSL().ShiftHue(degree).ToRGB();
            this.R = rotated.R;
            this.G = rotated.G;
            this.B = rotated.B;

            Console.WriteLine("R: {0} G:{1} B:{2}", this.R, this.G, this.B);
        }

        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }

        public static LEDBulb Subtract(double v, ColorPropertyType t, LEDBulb original)
        {
            if (v  == 0)
            {
                return new LEDBulb(0, 0, 0);
            }

            Color.HSL newColor = new Color.RGB(original.R, original.G, original.B).ToHSL();
            switch (t)
            {
                case ColorPropertyType.Hue:
                    newColor.ShiftHue(v).ToRGB();
                    break;
                case ColorPropertyType.Saturation:
                    newColor = new Color.HSL(newColor.H, 1, v);
                    break;
                case ColorPropertyType.Lightness:
                    newColor.ShiftLightness(-v);
                    break;
                default:
                    break;
            }

            return new LEDBulb(newColor);
        }

        public int Id { get; set; }

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

        public static implicit operator LEDBulb(Color.HSV hsv)
        {
            var c = hsv.ToRGB();
            return new LEDBulb(c.R, c.G, c.B);
        }

        public static implicit operator LEDBulb(Color.HSL hsl)
        {
            var c = hsl.ToRGB();
            return new LEDBulb(c.R, c.G, c.B);
        }

        //public static implicit operator LEDBulb(Spectrum.Color rgb)
        //{
        //    return new LEDBulb(rgb.R, rgb.G, rgb.B);
        //}

    }

    static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}

