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
            SpectrumResolution = 3;
        }

        public int Count { get; internal set; }

        public void CreateSpectrum()
        {
               var fftBuffer = new float[(int)FftSize];
            SpectrumProvider.GetFftData(fftBuffer, this);
            SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(255, fftBuffer);
            if (spectrumPoints[0].Value > 0)
            {
                Effects.ColorOne.R = (byte)(spectrumPoints[0].Value);
            }
            if (spectrumPoints[1].Value > 0)
            {
                Effects.ColorOne.G = (byte)((2*spectrumPoints[1].Value));
            }
            if (spectrumPoints[2].Value > 0)
            {
                Effects.ColorOne.B = (byte) ((2-spectrumPoints[2].Value));
            }
        }


    }
}
