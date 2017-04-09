using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HueHue;
using CSCore.DSP;
using System.Drawing;

namespace HueHue
{
    class MusicSpectrum:SpectrumBase
    {
        public MusicSpectrum(FftSize fftSize)
        {
            FftSize = fftSize;
            SpectrumResolution = Properties.Settings.Default.TotalLeds *3;
        }

        public int Count { get; internal set; }

        public void CreateSpectrum()
        {
               var fftBuffer = new float[(int)FftSize];
            SpectrumProvider.GetFftData(fftBuffer, this);
            SpectrumPointData[] spectrumPoints = CalculateSpectrumPoints(255, fftBuffer);
            
            Effects.ColorOne.R =  (byte)(spectrumPoints[0].Value);
            Effects.ColorOne.G = 0; //150;//(byte)(255-spectrumPoints[1].Value);
            Effects.ColorOne.B = 0;//150;//(byte)(255-spectrumPoints[2].Value);

        }


    }
}
