using HueHue.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Media = System.Windows.Media;


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
                Effects.Colors.Add(new LEDBulb());
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
                    Decimal RStep = (Decimal)(Effects.Colors[0].R - Effects.Colors[1].R) / (App.settings.Length + 1);
                    Decimal GStep = (Decimal)(Effects.Colors[0].G - Effects.Colors[1].G) / (App.settings.Length + 1);
                    Decimal BStep = (Decimal)(Effects.Colors[0].B - Effects.Colors[1].B) / (App.settings.Length + 1);

                    List<LEDBulb> comet = new List<LEDBulb>();

                    for (int i = 0; i < App.settings.Length; i++)
                    {
                        comet.Add(new LEDBulb());
                    }

                    comet[0] = Effects.Colors[0];
                    comet[comet.Count - 1] = Effects.Colors[1];

                    for (int i = 1; i < comet.Count - 1; i++)
                    {
                        Console.WriteLine(i);
                        if (RStep > 1 || RStep < -1)
                        {
                            comet[i].R = Byte.Parse(Math.Floor(comet[i - 1].R - RStep).ToString());
                        }
                        if (GStep > 1 || GStep < -1)
                        {
                            comet[i].G = Byte.Parse(Math.Floor(comet[i - 1].G - GStep).ToString());
                        }
                        if (BStep > 1 || BStep < -1)
                        {
                            comet[i].B = Byte.Parse(Math.Floor(comet[i - 1].B - BStep).ToString());
                        }
                    }

                    //for (int i = 0; i < App.settings.Length; i++)
                    //{
                    //    comet.Add(new LEDBulb());

                    //    if (RStep > 1)
                    //    {
                    //        comet[i].R = Byte.Parse((Effects.Colors[0].R - (RStep * i)).ToString());
                    //    }
                    //    else if (RStep < 0)
                    //    {
                    //        comet[i].R = Byte.Parse(((RStep * i) + Effects.Colors[1].R).ToString());
                    //    }
                    //    else
                    //    {
                    //        comet[i].R = 0;
                    //    }

                    //    if (GStep > 1)
                    //    {
                    //        comet[i].G = Byte.Parse((Effects.Colors[0].G - (GStep * i)).ToString());
                    //    }
                    //    else if (GStep < 0)
                    //    {
                    //        comet[i].G = Byte.Parse(((GStep * i) + Effects.Colors[1].G).ToString());
                    //    }
                    //    else
                    //    {
                    //        comet[i].G = 0;
                    //    }
                    //    if (BStep > 1)
                    //    {
                    //        comet[i].B = Byte.Parse((Effects.Colors[0].B - (BStep * i)).ToString());
                    //    }
                    //    else if (BStep < 0)
                    //    {
                    //        comet[i].B = Byte.Parse(((BStep * i) + Effects.Colors[1].B).ToString());
                    //    }
                    //    else
                    //    {
                    //        comet[i].B = 0;
                    //    }
                    //}

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
            Effects.Colors[0] = (LEDBulb)e.CurrentColor;

            //Effects.FixedColor();
        }

        private void TailColor_ColorChanged(object sender, ColorTools.ColorControlPanel.ColorChangedEventArgs e)
        {
            Effects.Colors[1] = (LEDBulb)e.CurrentColor;

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
