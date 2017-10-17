using System;
using System.Collections.Generic;

namespace HueHue.Helpers
{
    /// <summary>
    /// Logic implementation for all of the available effects
    /// </summary>
    public static class Effects
    {
        /// <summary>
        /// List of colors the "Fixed Color" effect is based
        /// </summary>
        public static List<LEDBulb> Colors = new List<LEDBulb>();

        /// <summary>
        /// Strip of LEDs representing the LEDs attached to the Arduino
        /// </summary>
        public static List<LEDBulb> LEDs;

        /// <summary>
        /// Resets and setup de LEDs strip
        /// </summary>
        /// <param name="TotalLEDs">Amount of LEDs on the Strip</param>
        public static void Setup(int TotalLEDs)
        {
            if (LEDs == null || LEDs.Count > 0)
            {
                LEDs = new List<LEDBulb>();
            }

            for (int i = 0; i < TotalLEDs; i++)
            {
                LEDs.Add(new LEDBulb());
            }
        }

        /// <summary>
        /// Step used for effects
        /// </summary>
        private static int step;

        /// <summary>
        /// Fills a entire LED strip with the List<Colors> the user wants
        /// </summary>
        public static void FixedColor()
        {
            int count = 0;

            while (count < LEDs.Count)
            {
                if (count >= LEDs.Count)
                {
                    return;
                }

                foreach (var color in Colors)
                {
                    LEDs[count] = color;
                    count++;

                    if (count >= LEDs.Count)
                    {
                        return;
                    }
                }
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
                    LEDs[i].R = Colors[0].R;
                    LEDs[i].G = Colors[0].G;
                    LEDs[i].B = Colors[0].B;
                }
                else
                {
                    LEDs[i].R = Colors[1].R;
                    LEDs[i].G = Colors[1].G;
                    LEDs[i].B = Colors[1].B;
                }
            }
        }

        public static void RandomColor(List<LEDBulb> LEDs)
        {
            Random random = new Random();
            Colors[0].R = (byte)random.Next(255);
            Colors[0].G = (byte)random.Next(255);
            Colors[0].B = (byte)random.Next(255);

            FixedColor();
        }

        /// <summary>
        /// Placeholder
        /// </summary>
        public static void Music() /// selo rob de codigos bonito
        {

        }

        /// <summary>
        /// Resets the position of the snake, preventing out of bounds errors
        /// </summary>
        public static void ResetStep()
        {
            step = 1;
        }

        /// <summary>
        /// Logic for the SnakeColor effect
        /// </summary>
        /// <param name="length">Lenght of the snake</param>
        public static void Snake(int length)
        {
            Console.WriteLine(step);
            if (step + length == LEDs.Count + length) //When the snake is completely out of bounds, reset its step
            {
                ResetStep();
            }

            if (step + length > LEDs.Count) //If the snake is going out of bounds, decrease its size so it actually fits the strip
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
                            LEDs[i + j] = Colors[1]; //Set color on the snake
                        }
                        i += length; //Move the strip counter foward
                    }
                    else
                    {
                        LEDs[i] = Colors[0]; //Sets colors for non "snake" leds
                    }
                }
                step++;
            }
            else //The snake is coming out ( ° ʖ °)
            {
                for (int i = 0; i < step; i++)
                {
                    LEDs[i] = Colors[1];
                }
                for (int i = step; i < LEDs.Count; i++)
                {
                    LEDs[i] = Colors[0];
                }
                step++;
            }
        }

        /// <summary>
        /// Cycles through all the basic colors
        /// Run ResetStep() and set Effects.ColorOne.R to 255 before starting to use this effect
        /// </summary>
        public static void ColorCycle()
        {
            if (step == 1) //Started with Red, transition to Yellow
            {
                Colors[0].G++;

                if (Colors[0].G == 255)
                {
                    step++;
                }
            }
            else if (step == 2) //From Yellow to Green
            {
                Colors[0].R--;

                if (Colors[0].R == 0)
                {
                    step++;
                }
            }
            else if (step == 3) //From Green to Alice Blue
            {
                Colors[0].B++;

                if (Colors[0].B == 255)
                {
                    step++;
                }
            }
            else if (step == 4) //From Alice Blue to Blue
            {
                Colors[0].G--;

                if (Colors[0].G == 0)
                {
                    step++;
                }
            }
            else if (step == 5) // From Blue to Pink/Purple
            {
                Colors[0].R++;
                if (Colors[0].R == 255)
                {
                    step++;
                }
            }
            else if (step == 6) //From Pink/Purple to Red
            {
                Colors[0].B--;
                if (Colors[0].B == 0)
                {
                    ResetStep(); //We Cycled all the basic colors, reset and start again
                }
            }

            FixedColor(); //Fill the strip with the color
        }

        public static void ShutOff()
        {
            foreach (LEDBulb LED in LEDs)
            {
                LED.R = 0;
                LED.B = 0;
                LED.G = 0;
            }
        }
    }
}
