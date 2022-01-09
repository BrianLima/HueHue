using System;
using CSCore.DSP;

namespace HueHue.Helpers.Modes
{
    class MusicSpectrum : SpectrumBase
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
                // Mode.Colors[0].SetR(CalculateScale(spectrumPoints[0].Value));
            }
            if (spectrumPoints[1].Value > 0)
            {
                // Mode.Colors[0].SetG(CalculateScale(spectrumPoints[1].Value));
            }
            if (spectrumPoints[2].Value > 0)
            {
                // Mode.Colors[0].SetB(CalculateScale(spectrumPoints[2].Value));
            }
        }

        private const int intervalLength = 255;
        private double minimum = Double.MaxValue;
        private double maximum = Double.MinValue;

        public byte CalculateScale(double value)
        {
            if (value < minimum)//value can be min and max at 1st step
            {
                minimum = value;
            }
            if (value > maximum)
            {
                maximum = value;
            }

            double delta = maximum - minimum;//total interval which will be mapped to 255
            if (delta.Equals(0.0))//will be at 1st step.
            {
                return 0;
            }

            return (byte)(value * intervalLength / delta);//astually, it's (value/delta)*intervalLength, but it's better to multiply first.
        }
    }
}
