using HueHue.Helpers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Media = System.Windows.Media;
using RGB.NET.Core;

namespace HueHue.Views
{
    /// <summary>
    /// Interaction logic for CometMode.xaml
    /// </summary>
    public partial class CometMode : UserControl
    {
        private CancellationTokenSource _cancellationTokenSource;
        private Thread _workerThread;

        public CometMode()
        {
            InitializeComponent();

            while (Effects.Colors.Count < 2)
            {
                Effects.Colors.Add(new Color());
            }

            backgroundColor.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Effects.Colors[0].R, Effects.Colors[0].G, Effects.Colors[0].B));
            backgroundColor.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Effects.Colors[0].R, Effects.Colors[0].G, Effects.Colors[0].B));
            cometColor.SelectedColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Effects.Colors[1].R, Effects.Colors[1].G, Effects.Colors[1].B));
            cometColor.InitialColorBrush = new Media.SolidColorBrush(Media.Color.FromArgb(0, Effects.Colors[1].R, Effects.Colors[1].G, Effects.Colors[1].B));

            gridMain.DataContext = App.settings;

            _cancellationTokenSource = new CancellationTokenSource();
            _workerThread = new Thread(BackgroundWorker_DoWork) { Name = "CometMode", IsBackground = true };
            _workerThread.Start(_cancellationTokenSource.Token);
        }

        private void BackgroundWorker_DoWork(object tokenObject)
        {
            var cancellationToken = (CancellationToken)tokenObject;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var Origin = new Color(Effects.Colors[0].R,Effects.Colors[0].G, Effects.Colors[0].B);

                    var Destiny = new Color(Effects.Colors[1].R, Effects.Colors[1].G, Effects.Colors[1].B);

                    List<Color> comet = new List<Color>();

                    for (int i = 0; i < App.settings.Length; i++)
                    {
                        comet.Add(new Color());
                    }


                    for (int i = 0; i < comet.Count; i++)
                    {
                       // comet[i] = Origin.
                    }



                    Effects.CometMode(comet);
                    Thread.Sleep(App.settings.Speed);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void BackgroundColor_ColorChanged(object sender, ColorTools.ColorControlPanel.ColorChangedEventArgs e)
        {
            Effects.Colors[0] = new Color(e.CurrentColor.R, e.CurrentColor.G, e.CurrentColor.B);

            //Effects.FixedColor();
        }

        private void TailColor_ColorChanged(object sender, ColorTools.ColorControlPanel.ColorChangedEventArgs e)
        {
            Effects.Colors[1] = new Color(e.CurrentColor.R, e.CurrentColor.G, e.CurrentColor.B);

            //Effects.FixedColor();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
            _workerThread.Join();
            _workerThread = null;
        }
    }
}
