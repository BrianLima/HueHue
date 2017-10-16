using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HueHue;
using CSCore.DSP;
using System.Drawing;
using HueHue.Helpers;

namespace HueHue.Helpers
{
    class MusicSpectrum:SpectrumBase
    {
        public MusicSpectrum(FftSize fftSize)
        {
            FftSize = fftSize;
            SpectrumResolution = 6;
        }

        public int Count { get; internal set; }

        public void CreateSpectrum()
        {
               var fftBuffer = new float[(int)FftSize];
            SpectrumProvider.GetFftData(fftBuffer, this);
            SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(255, fftBuffer);
            if (spectrumPoints[0].Value > 0)
            {
                Effects.Colors[0].R = CalculateScale(spectrumPoints[0].Value);
            }
            if (spectrumPoints[1].Value > 0)
            {
                Effects.Colors[0].G = CalculateScale(spectrumPoints[0].Value);
            }
            if (spectrumPoints[2].Value > 0)
            {
                Effects.Colors[0].B = CalculateScale(spectrumPoints[0].Value);
            }
        }

        private double minimum;
        private double maximum;

        public byte CalculateScale(double value)
        {
            if (value == 0)
            {
                return 0;
            }
            else if (value < minimum)
            {
                minimum = value;
            }
            else if (value > maximum)
            {
                maximum = value;
            }

            var x = 255/maximum;

            return (byte)(x*value);
        }
    }
}
