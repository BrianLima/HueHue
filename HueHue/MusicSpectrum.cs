using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HueHue;
using CSCore.DSP;

namespace HueHue
{
    class MusicSpectrum:SpectrumBase
    {
        public MusicSpectrum(FftSize fftSize)
        {
            FftSize = fftSize;
        }

        public int Count { get; internal set; }

        public void CreateSpectrum(float[] fftBuffer)
        {
            SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(255, fftBuffer);
        }


    }
}
