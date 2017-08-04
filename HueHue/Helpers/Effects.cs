using System;
using System.Collections.Generic;

namespace HueHue.Helpers
{
    /// <summary>
    /// Logic implementation for all of the available effects
    /// </summary>
    public static class Effects
    {
        //The colors the effect will be based on
        public static LEDBulb ColorOne = new LEDBulb();
        public static LEDBulb ColorTwo = new LEDBulb();
        public static LEDBulb ColorThree = new LEDBulb();

        public static List<LEDBulb> LEDs = new List<LEDBulb>();

        /// <summary>
        /// Fills a entire LED strip with a solid color
        /// </summary>
        public static void FixedColor()
        {
            //Console.Write("R: " + ColorOne.R + " B:" + ColorOne.B + " G: " + ColorOne.G);
            //Console.WriteLine();

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
        public static void TwoAlternateColor()
        {
            for (int i = 0; i < LEDs.Count; i++)
            {
                if (i % 2 == 0)
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

            FixedColor();
        }

        public static void music()
        {

        }

        private static int step;

        /// <summary>
        /// Resets the position of the snake, preventing out of bounds errors
        /// </summary>
        public static void ResetSnake()
        {
            step = 1;
        }

        /// <summary>
        /// Logic for the SnakeColor effect
        /// </summary>
        /// <param name="LEDs">LED strip to apply the effect</param>
        /// <param name="length">Lenght of the snake</param>
        public static void Snake(List<LEDBulb> LEDs, int length)
        {
            if (step + length == LEDs.Count + length) //When the snake is completely out of bounds, reset it's step
            {
                step = 1;
            }

            if (step + length > LEDs.Count) //If the snake is going out of bounds, decrease it's size so it actually fits the strip
            {
                length -= ((step + length) - LEDs.Count);
            }

            if (step >= length) //The snake is fully out ( ° ʖ °)
            {
                for (int i = 0; i < LEDs.Count; i++) //Loops through the strip
                {
                    if (i == step) //Starting point from the "Tail" of the snake 
                    {
                        for (int j = 0; j < length; j++) //Loop through the snake
                        {
                            LEDs[i + j] = ColorTwo; //Set color on the snake
                        }
                        i += length; //Move the strip counter foward
                    }
                    else
                    {
                        LEDs[i] = ColorOne; //Sets colors for non "snake" leds
                    }
                }
                step++;
            }
            else //The snake is coming out ( ° ʖ °)
            {
                for (int i = 0; i < step; i++)
                {
                    LEDs[i] = ColorTwo;
                }
                for (int i = step; i < LEDs.Count; i++)
                {
                    LEDs[i] = ColorOne;
                }
                step++;
            }
        }
    }
}
