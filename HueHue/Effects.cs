using HueHue.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueHue
{
    public static class Effects
    {
        public static bool firstRun;
        private static byte step;
        private static byte stepR, stepG, stepB, originalR, originalG, originalB;

        /// <summary>
        /// Interation logic to breath effect a List<LEDBulb>
        /// </summary>
        /// <param name="LEDS"></param>
        public static void Breathing(List<LEDBulb> LEDS)
        {
            //The perfect way to do this would be by dimming the brightness from 255 to 0, but i still don't have a way to set brightness in C#
            //So instead i'm trying to fade the collors to black

            if (firstRun)
            {
                originalR = 
                step = (byte)(255 / Settings.Default.BreathDelay);
                firstRun = false;
            }
            else
            {
                Thread.Sleep(Settings.Default.BreathDelay / step);
            }

            if (LEDS[0].B == 0 && LEDS[0].G == 0 && LEDS[0].G == 0)
            {

            }
            else
            {

            }


        }
    }
}
