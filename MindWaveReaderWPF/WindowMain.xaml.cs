using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;
using MessageBox = System.Windows.MessageBox;

namespace MindWaveReaderWPF
{
    public partial class WindowMain
    {
        private ThingGearController _thingGearController;

        private double stepUp;
        private double stepDown;
        private double stepRight;
        private double stepLeft;

        public WindowMain()
        {
            InitializeComponent();
            BallRectangleStartingPositions();
            _thingGearController = new ThingGearController(LogAdd, RefreshUi);

            this.SizeChanged += MainWindow_SizeChanged;
            Closing += WindowMain_Closing;
            ButtonDisconnect.IsEnabled = false;
        }

        private void WindowMain_Closing(object sender, CancelEventArgs e)
        {
            _thingGearController.ThingGearDisconnect();
        }

        #region Initializing ball and goalframe

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BallRectangleStartingPositions();
        }
        private void BallRectangleStartingPositions()
        {

            if (ballSpace.IsLoaded)
            {
                double width = ballSpace.ActualWidth;
                double height = ballSpace.ActualHeight;

                double ballStartingWidth = width * 0.1;
                double ballStartingHeight = height * 0.8;

                Canvas.SetLeft(Ball, ballStartingWidth);
                Canvas.SetTop(Ball, ballStartingHeight);

                double goalFrameStartingWidth = width * 0.7;
                double goalFrameStartingHeight = height * 0.3;

                Canvas.SetLeft(goalFrame, goalFrameStartingWidth);
                Canvas.SetTop(goalFrame, goalFrameStartingHeight);

                stepUp = Math.Abs(ballStartingHeight - goalFrameStartingHeight) / 6;
                //stepDown = stepUp / 2;
                stepRight = Math.Abs(ballStartingWidth - goalFrameStartingWidth) / 6;
                //stepLeft = stepRight / 2;

            };

        }
        private void isLoaded(object sender, RoutedEventArgs e)
        {
            if (ballSpace.IsLoaded)
            {
                double width = ballSpace.ActualWidth;
                double height = ballSpace.ActualHeight;

                double ballStartingWidth = width * 0.1;
                double ballStartingHeight = height * 0.8;

                Canvas.SetLeft(Ball, ballStartingWidth);
                Canvas.SetTop(Ball, ballStartingHeight);

                double goalFrameStartingWidth = width * 0.7;
                double goalFrameStartingHeight = height * 0.3;

                Canvas.SetLeft(goalFrame, goalFrameStartingWidth);
                Canvas.SetTop(goalFrame, goalFrameStartingHeight);

                stepUp = Math.Abs(ballStartingHeight - goalFrameStartingHeight) / 10;
                stepRight = Math.Abs(ballStartingWidth - goalFrameStartingWidth) / 10;

            };
        }

        #endregion

        #region TextBox Logger methods

        private void LogAdd(string logLine)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    TextBlockLog.Text += logLine + "\r\n";
                    TextBlockLog.InvalidateVisual();
                }, DispatcherPriority.ContextIdle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LogClear()
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    TextBlockLog.Text = null;
                    TextBlockLog.InvalidateVisual();
                }, DispatcherPriority.ContextIdle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Button actions

        private void ButtonConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _thingGearController.ThingGearConnect(ComboBoxPortName.Text);

                ButtonConnect.IsEnabled = false;
                ButtonDisconnect.IsEnabled = true;
            }
            catch (Exception ex)
            {
                LogAdd("Exception: ButtonConnect_Click");
                LogAdd(ex.Message);
            }
        }

        private void ButtonDisconnect_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                _thingGearController.ThingGearDisconnect();

                ButtonConnect.IsEnabled = true;
                ButtonDisconnect.IsEnabled = false;
            }
            catch (Exception ex)
            {
                LogAdd("Exception: ButtonDisconnect_Click");
                LogAdd(ex.Message);
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            ButtonDisconnect_Click(sender, e);
            InitializeComponent();
            _thingGearController = new ThingGearController(LogAdd, RefreshUi);
        }

        #endregion

        #region UI Refresh

        private void RefreshUi(ThinkGearData currentTgData)
        {
            Dispatcher.Invoke(() =>
            {
                try
                {
                    RefreshGaugesValues(currentTgData);    
                    RefreshAngularGaugesValues(currentTgData);

                    //RefreshTextLogData();
                }
                catch (Exception ex)
                {
                    LogAdd("Exception: RefreshUI");
                    LogAdd(ex.Message);
                }

            }, DispatcherPriority.ContextIdle);
        }

        private void RefreshGaugesValues(ThinkGearData currentTgData)
        {

            if (currentTgData.Attention > -1)
            {
                if (currentTgData.Attention>=40 && Canvas.GetTop(Ball) > 0 && Canvas.GetLeft(Ball) < ballSpace.ActualWidth - Ball.ActualWidth)
                {
                    Canvas.SetTop(Ball, Canvas.GetTop(Ball) - stepUp);
                    Canvas.SetLeft(Ball, Canvas.GetLeft(Ball) + stepRight);
                    if (checkIfGoal())
                    {
                        stepUp = 0;
                        stepRight = 0;
                        ScoreAGoal.Visibility = Visibility.Hidden;
                        Goaaaaal.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    Canvas.SetTop(Ball, Canvas.GetTop(Ball) + stepUp);
                    Canvas.SetLeft(Ball, Canvas.GetLeft(Ball) - stepRight);
                }

                if (Canvas.GetLeft(Ball) <= ballSpace.ActualWidth * 0.1)
                {
                    Canvas.SetLeft(Ball, ballSpace.ActualWidth * 0.1);
                }
                if (Canvas.GetTop(Ball) >= ballSpace.ActualHeight*0.8)
                {
                    Canvas.SetTop(Ball, ballSpace.ActualHeight * 0.8);
                }

                GaugeAttention.Value = _thingGearController.TgLatestData.Attention = currentTgData.Attention;
                GaugeAttention.LabelFormatter = value => value + "%";
            }

        }
        private bool checkIfGoal()
        {
            if(Canvas.GetTop(goalFrame) <= Canvas.GetTop(Ball) && Canvas.GetTop(goalFrame)+goalFrame.ActualHeight >= Canvas.GetTop(Ball) + Ball.ActualHeight)
            {
                if(Canvas.GetLeft(goalFrame) <= Canvas.GetLeft(Ball) && Canvas.GetLeft(goalFrame)+goalFrame.ActualWidth >= Canvas.GetLeft(Ball)+Ball.ActualWidth)
                {
                    return true;
                }
            }
            return false;
        }

        private void RefreshAngularGaugesValues(ThinkGearData currentTgData)
        {
            // AngularGauges: Signal

            if (!(currentTgData.PoorSignal > -1)) return;
            GaugeSignalStrength.Value = Converters.PoorSignalTranscoder(currentTgData.PoorSignal);
            _thingGearController.TgLatestData.PoorSignal = currentTgData.PoorSignal;
        }

        #region ChartsHelpers

        private static IEnumerable<LineSeries> SetupSeriesForChart(ThinkGearData[] data)
        {
            return new List<LineSeries>
            {
                GetSeriesForParameters("Delta Power",
                    new ChartValues<double>(data.Select(q => q.EegPowerDelta))),
                GetSeriesForParameters("Theta Power",
                    new ChartValues<double>(data.Select(q => q.EegPowerTheta))),
                GetSeriesForParameters("Low Alpha Power",
                    new ChartValues<double>(data.Select(q => q.EegPowerAlpha1))),
                GetSeriesForParameters("High Alpha Power",
                    new ChartValues<double>(data.Select(q => q.EegPowerAlpha2))),
                GetSeriesForParameters("Low Beta Power",
                    new ChartValues<double>(data.Select(q => q.EegPowerBeta1))),
                GetSeriesForParameters("High Beta Power",
                    new ChartValues<double>(data.Select(q => q.EegPowerBeta2))),
                GetSeriesForParameters("Low Gamma Power",
                    new ChartValues<double>(data.Select(q => q.EegPowerGamma1))),
                GetSeriesForParameters("High Gamma Power",
                    new ChartValues<double>(data.Select(q => q.EegPowerGamma2)))
            };
        }

        private static LineSeries GetSeriesForParameters(string title, IChartValues values)
        {
            return new LineSeries
            {
                Title = title,
                Values = values,
                PointGeometry = null,
            };
        }

        #endregion

        private void UpdateLatestDataEeg(ThinkGearData currentTgData)
        {
            _thingGearController.TgLatestData.EegPowerDelta = currentTgData.EegPowerDelta;
            _thingGearController.TgLatestData.EegPowerTheta = currentTgData.EegPowerTheta;
            _thingGearController.TgLatestData.EegPowerAlpha1 = currentTgData.EegPowerAlpha1;
            _thingGearController.TgLatestData.EegPowerAlpha2 = currentTgData.EegPowerAlpha2;
            _thingGearController.TgLatestData.EegPowerBeta1 = currentTgData.EegPowerBeta1;
            _thingGearController.TgLatestData.EegPowerBeta2 = currentTgData.EegPowerBeta2;
            _thingGearController.TgLatestData.EegPowerGamma1 = currentTgData.EegPowerGamma1;
            _thingGearController.TgLatestData.EegPowerGamma2 = currentTgData.EegPowerGamma2;
        }

        private void RefreshTextLogData()
        {
            var textLog = new StringBuilder();
            textLog.AppendLine(Properties.Resources.Data);
            textLog.AppendLine(_thingGearController.TgLatestData.ToString());
            TextBlockLog.Text = textLog.ToString();
        }

        #endregion

        #region CSV Saving Function

        /// <summary>
        /// Saving collected records to CSV file  - click button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CSVexport_Click(object sender, RoutedEventArgs e)
        {
            if (_thingGearController.TgQueue.Count < Properties.Settings.Default.ResultsChartSize)
            {
                MessageBox.Show(Properties.Resources.SavingFileNotPossible,
                    Properties.Resources.TryConnectMindWave);
                return;
            }

            try
            {
                var saveAsfile = new SaveFileDialog
                {
                    InitialDirectory = Properties.Settings.Default.SavingFileInitialDirectory,
                    Title = Properties.Resources.FileTitle,
                    Filter = Properties.Settings.Default.SavingFileTypes,
                    DefaultExt = Properties.Settings.Default.SavingFileDefaultType,
                    AddExtension = true
                };

                if (saveAsfile.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

                var file = new StreamWriter(new FileStream(saveAsfile.FileName, FileMode.Create, FileAccess.Write));

                file.WriteLine(ThinkGearData.GenerateCsvHeader());

                foreach (var dataRow in Converters.TgQueueDataMerge(_thingGearController.TgQueue))
                {
                    file.WriteLine(ThinkGearData.ThinkGearDataToCsvRow(dataRow));
                }

                file.Close();
                MessageBox.Show(Properties.Resources.SavingSuccessfull);
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.ToString(), Properties.Resources.SavingFailed);
            }
        }

        #endregion

    }
}
