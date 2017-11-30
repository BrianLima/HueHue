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
        /// Step used for some effects
        /// </summary>
        private static int step;

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
        /// Shifts the LEDs to the right, placing the last one at first
        /// </summary>
        public static void ShiftRight()
        {
            LEDs.Insert(0, LEDs[LEDs.Count - 1]);
            LEDs.RemoveAt(LEDs.Count - 1);
        }

        /// <summary>
        /// Applies the rainbow effect on the LED strip
        /// https://krazydad.com/tutorials/makecolors.php
        /// </summary>
        /// <param name="center"></param>
        /// <param name="width"></param>
        /// <param name="frequencyR"></param>
        /// <param name="frequencyG"></param>
        /// <param name="frequencyB"></param>
        /// <param name="phaseR"></param>
        /// <param name="phaseG"></param>
        /// <param name="phaseB"></param>
        public static void CalcRainbow(int center, int width, decimal frequencyR, decimal frequencyG, decimal frequencyB, int phaseR, int phaseG, int phaseB)
        {
            for (int i = 0; i < LEDs.Count / 2; ++i)
            {
                var red = Math.Sin((double)(frequencyR * i + phaseR)) * width + center;
                var grn = Math.Sin((double)(frequencyG * i + phaseG)) * width + center;
                var blu = Math.Sin((double)(frequencyB * i + phaseB)) * width + center;

                LEDs[i] = new LEDBulb((byte)red, (byte)grn, (byte)blu);
                //To make the rainbow effect less jarring to the eye and more smooth, a workaround is to fill half the strip with it and then just mirror it along so it ends and starts with the same color
                LEDs[LEDs.Count - i - 1] = new LEDBulb((byte)red, (byte)grn, (byte)blu);
            }
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pressedButtons"></param>
        /// <param name="spotLength"></param>
        public static void JoystickMode(List<JoystickButtonToColor> pressedButtons, int spotLength)
        {
            if (pressedButtons.Count == 0)
            {
                return;
            }

            int i = 0;
            while (i < LEDs.Count)
            {
                foreach (var item in pressedButtons)
                {
                    if (spotLength == 0)
                    {
                        LEDs[i] = item.Color;
                        i++;
                    }
                    else
                    {
                        for (int j = 0; j < spotLength; j++)
                        {
                            if (i >= LEDs.Count)
                            {
                                break;
                            }

                            LEDs[i] = item.Color;
                            i++;

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Placeholder
        /// </summary>
        public static void Music() /// selo rob de codigos bonito
        {

        }

        /// <summary>
        /// Fills the strip first with Colors[0] for the length of App.settings.Length
        /// The fills the rest of the strip with Colors[1]
        /// </summary>
        public static void FillSNakeStrip()
        {
            for (int i = 0; i < LEDs.Count; i++)
            {
                if (i < App.settings.Length) //Defines the "snake"
                {
                    LEDs[i] = Colors[0];
                }
                else
                {
                    LEDs[i] = Colors[1];
                }
            }
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
        //public static void Snake(int length) //Why did i even did this to my self?
        //“Why are we still here? Just to suffer? Every night, I can feel my leg… and my arm… even my fingers.The body I’ve lost… the comrades I’ve lost… won’t stop hurting… It’s like they’re all still there.You feel it, too, don’t you?”
        //{
        //    Console.WriteLine(step);
        //    if (step + length == LEDs.Count + length) //When the snake is completely out of bounds, reset its step
        //    {
        //        ResetStep();
        //    }

        //    if (step + length > LEDs.Count) //If the snake is going out of bounds, decrease its size so it actually fits the strip
        //    {
        //        length -= ((step + length) - LEDs.Count);
        //    }

        //    if (step >= length) //The snake is fully out ( ° ʖ °)
        //    {
        //        for (int i = 0; i < LEDs.Count; i++) //Loops through the strip
        //        {
        //            if (i == step) //Starting point from the "Tail" of the snake 
        //            {
        //                for (int j = 0; j < length; j++) //Loop through the snake
        //                {
        //                    LEDs[i + j] = Colors[1]; //Set color on the snake
        //                }
        //                i += length; //Move the strip counter foward
        //            }
        //            else
        //            {
        //                LEDs[i] = Colors[0]; //Sets colors for non "snake" leds
        //            }
        //        }
        //        step++;
        //    }
        //    else //The snake is coming out ( ° ʖ °)
        //    {
        //        for (int i = 0; i < step; i++)
        //        {
        //            LEDs[i] = Colors[1];
        //        }
        //        for (int i = step; i < LEDs.Count; i++)
        //        {
        //            LEDs[i] = Colors[0];
        //        }
        //        step++;
        //    }
        //}

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

            //Fill the strip with the color
            for (int i = 0; i < LEDs.Count; i++)
            {
                LEDs[i] = Colors[0];
            }
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
