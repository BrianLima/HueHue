using System;
using System.Collections.Generic;

namespace HueHue
{
    public static class Effects
    {
        //The colors the effect will be based on
        public static LEDBulb ColorOne = new LEDBulb();
        public static LEDBulb ColorTwo = new LEDBulb();
         public static LEDBulb ColorThree = new LEDBulb();

        /// <summary>
        /// Fills a entire LED strip with a solid color
        /// </summary>
        /// <param name="LEDs"></param>
        public static void FixedColor(List<LEDBulb> LEDs)
        {
            foreach (LEDBulb LED in LEDs)
            {
                LED.R = ColorOne.R;
                LED.B = ColorOne.B;
                LED.G = ColorOne.G;
            }
        }

        /// <summary>
        /// Fills a strip alternating between two colors
        /// </summary>
        /// <param name="LEDs"></param>
        public static void TwoAlternateColor(List<LEDBulb> LEDs)
        {
            for (int i = 0; i < LEDs.Count; i++)
            {
                if (i%2 == 0)
                {
                    LEDs[i].R = ColorOne.R;
                    LEDs[i].G = ColorOne.G;
                    LEDs[i].B = ColorOne.B;
                }
                else
                {
                    LEDs[i].R = ColorTwo.R;
                    LEDs[i].G = ColorTwo.G;
                    LEDs[i].B = ColorTwo.B;
                }
            }
        }

        public static void RandomColor(List<LEDBulb> LEDs)
        {
            Random random = new Random();
            ColorOne.R = (byte)random.Next(255);
            ColorOne.G = (byte)random.Next(255);
            ColorOne.B = (byte)random.Next(255);

            FixedColor(LEDs);
        }
    }
}
