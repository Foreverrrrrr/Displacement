using Displacement.AutomaticMain;
using Displacement.FunctionCall;
using Displacement.ViewModels;
using ImTools;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Timers;
using System.Windows;
using static Displacement.FunctionCall.Geometry;
using Parameter = Displacement.ViewModels.Parameter;

namespace Displacement.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static AsyncUDPServer asyncUDP;
        private static MainWindowViewModel model;
        public static MainWindow thiswindow;
        public static ConfigurationFiles files;
        public MainWindow(IRegionManager region, IDialogService dialog)
        {
            InitializeComponent();
            model = MainWindowViewModel.thisModel;
            model.MessageLog = new MainWindowViewModel.MessageDialogs(MessageShowLog);
            thiswindow = this;
            this.ShowInTaskbar = false;
            this.Visibility = Visibility.Collapsed;
            LoginSystem login = new LoginSystem();
            login.ShowDialog();
            //files = new ConfigurationFiles();
            //login.Close();
            System.Timers.Timer timerT = new System.Timers.Timer();
            timerT.Enabled = true;
            timerT.Interval = 1000; //执行间隔时间,单位为毫秒; 这里实际间隔为10分钟  
            timerT.Start();
            timerT.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TemperatureChart chart1 = new TemperatureChart();
            chart1.Time = DateTime.Now;
            Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                for (int i = 0; i < 8; i++)
                {
                    chart1.Value = realtime[i];
                    switch (i)
                    {
                        case 0:
                            if (model.BounceOneAdSamplingAuto.Count < 200)
                                model.BounceOneAdSamplingAuto.Add(chart1);
                            else
                            {
                                model.BounceOneAdSamplingAuto.RemoveAt(0);
                                model.BounceOneAdSamplingAuto.Add(chart1);
                            }
                            break;
                        case 1:
                            if (model.BounceTwoAdSamplingAuto.Count < 200)
                                model.BounceTwoAdSamplingAuto.Add(chart1);
                            else
                            {
                                model.BounceTwoAdSamplingAuto.RemoveAt(0);
                                model.BounceTwoAdSamplingAuto.Add(chart1);
                            }
                            break;
                        case 2:
                            if (model.BounceThreeAdSamplingAuto.Count < 200)
                                model.BounceThreeAdSamplingAuto.Add(chart1);
                            else
                            {
                                model.BounceThreeAdSamplingAuto.RemoveAt(0);
                                model.BounceThreeAdSamplingAuto.Add(chart1);
                            }
                            break;
                        case 3:

                            if (model.BounceFourAdSamplingAuto.Count < 200)
                                model.BounceFourAdSamplingAuto.Add(chart1);
                            else
                            {
                                model.BounceFourAdSamplingAuto.RemoveAt(0);
                                model.BounceFourAdSamplingAuto.Add(chart1);
                            }
                            break;
                        case 4:
                            if (model.SerializationOneAdSamplingAuto.Count < 200)
                                model.SerializationOneAdSamplingAuto.Add(chart1);
                            else
                            {
                                model.SerializationOneAdSamplingAuto.RemoveAt(0);
                                model.SerializationOneAdSamplingAuto.Add(chart1);
                            }
                            break;
                        case 5:
                            if (model.SerializationTwoAdSamplingAuto.Count < 200)
                                model.SerializationTwoAdSamplingAuto.Add(chart1);
                            else
                            {
                                model.SerializationTwoAdSamplingAuto.RemoveAt(0);
                                model.SerializationTwoAdSamplingAuto.Add(chart1);
                            }
                            break;
                        case 6:
                            if (model.PressureOneAdSamplingAuto.Count < 200)
                                model.PressureOneAdSamplingAuto.Add(chart1);
                            else
                            {
                                model.PressureOneAdSamplingAuto.RemoveAt(0);
                                model.PressureOneAdSamplingAuto.Add(chart1);
                            }
                            break;
                        case 7:
                            if (model.PressureTwoAdSamplingAuto.Count < 200)
                                model.PressureTwoAdSamplingAuto.Add(chart1);
                            else
                            {
                                model.PressureTwoAdSamplingAuto.RemoveAt(0);
                                model.PressureTwoAdSamplingAuto.Add(chart1);
                            }
                            break;
                    }
                }

            });

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            //foreach (Process p in Process.GetProcesses())//GetProcessesByName(strProcessesByName))
            //{
            //    if (p.ProcessName.ToUpper().Contains("W500"))
            //    {
            //        try
            //        {
            //            p.Kill();
            //            p.WaitForExit(); // possibly with a timeout
            //        }
            //        catch (Win32Exception s)
            //        {
            //            MessageBox.Show(s.Message.ToString());   // process was terminating or can't be terminated - deal with it
            //        }
            //        catch (InvalidOperationException s)
            //        {
            //            MessageBox.Show(s.Message.ToString()); // process has already exited - might be able to let this one go
            //        }
            //    }
            //}
            foreach (var item in MainWindowViewModel.Dialogs)
            {
                item.Button_Click(sender, e);
            }
            this.Close();
            System.Environment.Exit(0);
        }

        private void Max_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void Min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void ContentControl_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Hometoggle.IsChecked = false;
        }

        private void Grid_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Hometoggle.IsChecked = false;
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            foreach (var item in MainWindowViewModel.Dialogs)
            {
                item.Button_Click(sender, null);
            }
            this.Close();
            System.Environment.Exit(0);
        }

        /// <summary>
        /// 下滑消息滑窗
        /// </summary>
        /// <param name="str">消息</param>
        /// <param name="color">颜色</param>
        /// <param name="time">持续时间</param>
        private void MessageShowLog(string str, string color, int time)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                NotifyData data = new NotifyData();
                data.MessageLogText = str;
                data.MessageLogColor = color;
                Point ptRightDown = new Point();
                ptRightDown = new Point(this.ActualWidth, this.ActualHeight);
                ptRightDown = this.PointToScreen(ptRightDown);
                MessageLogVision dialog = new MessageLogVision();
                dialog.Closed += Dialog_Closed;
                dialog.Y = GetTopFrom(ptRightDown.Y);
                dialog.X = ptRightDown.X;
                dialog.Time = time;
                MainWindowViewModel.Dialogs.Add(dialog);
                dialog.DataContext = data;
                dialog.Show();
                Log.LogVision(str, color);
            });

        }

        private double GetTopFrom(double y)
        {
            double topFrom = y;
            bool isContinueFind = MainWindowViewModel.Dialogs.Any(o => o.Y == topFrom);

            while (isContinueFind)
            {
                topFrom = topFrom - 130;//通知间距
                isContinueFind = MainWindowViewModel.Dialogs.Any(o => o.Y == topFrom);
            }

            if (topFrom <= 0)
                topFrom = System.Windows.SystemParameters.WorkArea.Bottom - 10;

            return topFrom;
        }

        private void Dialog_Closed(object sender, EventArgs e)
        {
            var closedDialog = sender as MessageLogVision;
            MainWindowViewModel.Dialogs.Remove(closedDialog);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            InitializationCompleteAsync(); /*"SELECT * FROM Server.dbo.Test WHERE Time >= DATEADD(day, -1, GETDATE()) AND Time <= GETDATE()"*/
        }

        private void InitializationCompleteAsync()
        {
            //model.Sqlserver = SQL_Server.SpecialQuery(DateTime.Now.AddDays(-1), DateTime.Now, true);
            //for (int i = 0; i < model.Sqlserver.Tables[0].Rows.Count; i++)
            //{
            //    for (int j = 0; j < model.Sqlserver.Tables[0].Columns.Count; j++)
            //    {
            //        Console.WriteLine(model.Sqlserver.Tables[0].Rows[i][j]);
            //    }
            //}
            //SQL_Server.ExecteNonQuery(CommandType.Text, $"insert into Test values('{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','一号','Ng')");

            ConfigurationFiles files = new ConfigurationFiles();
            ExcelTool.SavePath = model.SysteamPath;
            ExcelTool.Read(model.Arguments);
            var excellist = ExcelTool.GetTest();
            for (int i = 0; i < excellist.Count; i++)
            {
                model.ParameterNameList.Add(new MainWindowViewModel.ComboxList { ID = i, Name = excellist[i] });
            }
            model.ParameterIndexes = model.ParameterNameList.Find(x => x.Name == model.Arguments).ID;
            for (int i = 0; i < ExcelTool.Arguments.Count; i++)
            {
                model.ArgumentsData.Add(new Parameter { Name = ExcelTool.Arguments[i].TestName, Value = ExcelTool.Arguments[i].Value });
            }
            model.oneammeters = new OneAmmeter(model.OneAmmeterCom, Convert.ToInt32(model.OneAmmeterBaudrate));
            model.twoammeters = new TwoAmmeter(model.TwoAmmeterCom, Convert.ToInt32(model.TwoAmmeterBaudrate));

            ModbusTCP_PLC modbusTCP_ = new ModbusTCP_PLC(model.ModbusTCPiP, Convert.ToInt32(model.ModbusTCPport));
            Dispense232 dispense = new Dispense232(model.Dispense232ComName, Convert.ToInt32(model.Dispense232_Baudrate));
            ControlPower controlPower = new ControlPower(model.PowerComName, Convert.ToInt32(model.PowerBaudRate));
            asyncUDP = new AsyncUDPServer(IPAddress.Parse(model.GatheringModule1_IP), 1090, 10);
            asyncUDP.OnTCPReadEvent += Async_OnTCPReadEvent;
            asyncUDP.OnTCPReadEvent += Auto_OnTCPReadEvent;
            Auto auto = new Auto();
            ExcelTool.NewSaveData(model.SysteamPath);
            float[] te = ModbusTCP_PLC.thisModbus.ReadSave();
            ExcelTool.SaveData(te);
            var jk = Auto.GetSever(te);
            SQL_Server.Write(jk[0]);
            SQL_Server.Write(jk[1]);
            //ExcelTool.SaveData(new No4(), new No5(), new float[] {0,1});

        }

        public static byte[] String_Byte(string bytestr)
        {
            try
            {
                int d = Convert.ToInt32(bytestr);
                char[] t = d.ToString("x2").ToArray(); //X4
                string[] strings = new string[2];
                int k = 0;
                for (int i = 0; i < t.Length; i += 2)
                {

                    if (i + 1 < t.Length)
                    {
                        strings[k] = "0x" + t[i].ToString() + t[i + 1].ToString();

                    }
                    else
                    {
                        strings[k] = "0x" + t[i].ToString();
                    }
                    k++;
                }
                byte[] bytes = new byte[2];
                bytes[0] = Convert.ToByte(strings[0], 16);
                bytes[1] = Convert.ToByte(strings[1], 16);
                return bytes;

            }
            catch (Exception)
            {
                throw;
            }
        }

        static List<Point2D> Serializationone = new List<Point2D>();
        static List<Point2D> Serializationtwo = new List<Point2D>();
        static List<Point2D> Bounceone = new List<Point2D>();
        static List<Point2D> Bouncetwo = new List<Point2D>();
        static List<Point2D> Bouncethree = new List<Point2D>();
        static List<Point2D> Bouncefour = new List<Point2D>();
        static int dog = 0;

        static byte[][] bytes = new byte[13][];

        /// <summary>
        /// 位移采集
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        private void Async_OnTCPReadEvent(DateTime arg1, IPEndPoint arg2, byte[] arg3)
        {
            try
            {
                if (AsyncUDPServer.IF_Read)
                {
                    bytes[dog] = arg3;

                    dog++;
                    if (dog == get_list)
                        AsyncUDPServer.IF_Read = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }

        }

        public static double[] realtime = new double[8];

        /// <summary>
        /// 实时
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        private void Auto_OnTCPReadEvent(DateTime arg1, IPEndPoint arg2, byte[] arg3)
        {
            realtime[0] = ((arg3[0] * 256 + arg3[1]) * model.ArgumentsData[31].Value) - model.ArgumentsData[27].Value;
            realtime[1] = ((arg3[2] * 256 + arg3[3]) * model.ArgumentsData[32].Value) - model.ArgumentsData[28].Value;
            realtime[2] = ((arg3[4] * 256 + arg3[5]) * model.ArgumentsData[33].Value) - model.ArgumentsData[29].Value;
            realtime[3] = ((arg3[6] * 256 + arg3[7]) * model.ArgumentsData[34].Value) - model.ArgumentsData[30].Value;
            realtime[4] = ((arg3[8] * 256 + arg3[9]) * model.ArgumentsData[4].Value) - model.ArgumentsData[0].Value;
            realtime[5] = ((arg3[10] * 256 + arg3[11]) * model.ArgumentsData[5].Value) - model.ArgumentsData[1].Value;
            realtime[7] = (((arg3[12] * 256 + arg3[13]) * model.ArgumentsData[6].Value)) - model.ArgumentsData[2].Value;
            realtime[6] = (((arg3[14] * 256 + arg3[15]) * model.ArgumentsData[7].Value)) - model.ArgumentsData[3].Value;
            //ModbusTCP_PLC.thisModbus.Write("118", (float)realtime[6]);
            //ModbusTCP_PLC.thisModbus.Write("116", (float)realtime[7]);
            model.Doubles = realtime;
        }

        static double[] doubles1 = new double[975];
        static double[] doubles2 = new double[975];
        static double[] doubles3 = new double[975];
        static double[] doubles4 = new double[975];
        static double[] doubles5 = new double[975];
        static double[] doubles6 = new double[975];
        static double[] doubles7 = new double[975];
        static double[] doubles8 = new double[975];

        static int get_list = 0;

        #region 轴动采集
        /// <summary>
        /// 8AD采集
        /// </summary>
        /// <param name="time_ms">采集时间</param>
        /// <param name="numr">采集数 0==采集5工位全部载具 1==采集5工位1号载具 2==采集5工位2号载具</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static MainWindowViewModel.BounceTest Await_Get_Sampling(int time_ms, int numr)
        {
            AsyncUDPServer.IF_Read = false;
            get_list = time_ms / 75;
            bytes = new byte[get_list][];
            Point2D[] t1 = new Point2D[get_list * 75];
            Point2D[] t2 = new Point2D[get_list * 75];
            Point2D[] t3 = new Point2D[get_list * 75];
            Point2D[] t4 = new Point2D[get_list * 75];
            Point2D[] t5 = new Point2D[get_list * 75];
            Point2D[] t6 = new Point2D[get_list * 75];
            doubles1 = new double[get_list * 75];
            doubles2 = new double[get_list * 75];
            doubles3 = new double[get_list * 75];
            doubles4 = new double[get_list * 75];
            doubles5 = new double[get_list * 75];
            doubles6 = new double[get_list * 75];
            doubles7 = new double[get_list * 75];
            doubles8 = new double[get_list * 75];
            Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
            {
                model.BounceOneAdSampling.Clear();
                model.BounceTwoAdSampling.Clear();
                model.BounceThreeAdSampling.Clear();
                model.BounceFourAdSampling.Clear();
                model.BounceOnePeak.Clear();
                model.BounceTwoPeak.Clear();
                model.BounceOneGrain.Clear();
                model.BounceTwoGrain.Clear();
                model.BounceThreePeak.Clear();
                model.BounceFourPeak.Clear();
                model.BounceThreeGrain.Clear();
                model.BounceFourGrain.Clear();
            });
            MainWindowViewModel.BounceTest test = new MainWindowViewModel.BounceTest();
            var testdata = model.ArgumentsData;
            //byte[] senddata = new byte[] { 0xff, 0x00, 0xff, time_byte[0], 0x08, 0x00, 0x00, 0xfe, 0x00, 0xfe };
            //asyncUDP.Send(senddata);
            Serializationone.Clear();
            Serializationtwo.Clear();
            Bounceone.Clear();
            Bouncetwo.Clear();
            Bouncethree.Clear();
            Bouncefour.Clear();
            dog = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            AsyncUDPServer.IF_Read = true;

            do
            {
                if (stopwatch.ElapsedMilliseconds > time_ms + 500)
                    goto UDP_GET_TimeOut;
                System.Threading.Thread.Sleep(50);
            } while (AsyncUDPServer.IF_Read);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            double[] strings = new double[6];
            for (int i = 0; i < bytes.Length; i++)
            {
                for (int k = 0; k < bytes[i].Length; k += 16)
                {
                    strings[0] = ((bytes[i][k] * 256 + bytes[i][k + 1]) * model.ArgumentsData[31].Value) - model.ArgumentsData[27].Value;
                    t1[(k / 16) + i * 75].X = (k / 16) + i * 75;
                    t1[(k / 16) + i * 75].Y = Math.Round(strings[0], 4);
                    doubles1[(k / 16) + i * 75] = t1[(k / 16) + i * 75].Y;
                    strings[1] = ((bytes[i][k + 2] * 256 + bytes[i][k + 3]) * model.ArgumentsData[32].Value) - model.ArgumentsData[28].Value;
                    t2[(k / 16) + i * 75].X = (k / 16) + i * 75;
                    t2[(k / 16) + i * 75].Y = Math.Round(strings[1], 4);
                    doubles2[(k / 16) + i * 75] = t2[(k / 16) + i * 75].Y;

                    strings[2] = ((bytes[i][k + 4] * 256 + bytes[i][k + 5]) * model.ArgumentsData[33].Value) - model.ArgumentsData[29].Value;
                    t3[(k / 16) + i * 75].X = (k / 16) + i * 75;
                    t3[(k / 16) + i * 75].Y = Math.Round(strings[2], 4);
                    doubles3[(k / 16) + i * 75] = t3[(k / 16) + i * 75].Y;
                    strings[3] = ((bytes[i][k + 6] * 256 + bytes[i][k + 7]) * model.ArgumentsData[34].Value) - model.ArgumentsData[30].Value;
                    t4[(k / 16) + i * 75].X = (k / 16) + i * 75;
                    t4[(k / 16) + i * 75].Y = Math.Round(strings[3], 4);
                    doubles4[(k / 16) + i * 75] = t4[(k / 16) + i * 75].Y;
                }
            }
            double[] p1 = new double[2];
            double[] p2 = new double[2];
            double[] p3 = new double[2];
            double[] p4 = new double[2];

            double[] g1 = new double[2];
            double[] g2 = new double[2];
            double[] g3 = new double[2];
            double[] g4 = new double[2];
            if (numr == 0)//全部渲染
            {
                Bounceone.AddRange(t1);
                Bouncetwo.AddRange(t2);
                Bouncethree.AddRange(t3);
                Bouncefour.AddRange(t4);
                //p1 = FindPeaks(doubles1, Wave.Wavecrest);
                //g1 = FindPeaks(doubles1, Wave.Trough);

                //p2 = FindPeaks(doubles2, Wave.Wavecrest);
                //g2 = FindPeaks(doubles2, Wave.Trough);

                //p3 = FindPeaks(doubles3, Wave.Wavecrest);
                //g3 = FindPeaks(doubles3, Wave.Trough);
                //p4 = FindPeaks(doubles4, Wave.Wavecrest);
                //g4 = FindPeaks(doubles4, Wave.Trough);



                p1[0] = Bounceone.Max(t => t.Y);
                p1[1] = Bounceone.Max(t => t.X);

                g1[0] = Bounceone.Min(t => t.Y);
                g1[1] = Bounceone.Max(t => t.X);

                p2[0] = Bouncetwo.Max(t => t.Y);
                p2[1] = Bouncetwo.Max(t => t.X);

                g2[0] = Bouncetwo.Min(t => t.Y);
                g2[1] = Bouncetwo.Max(t => t.X);


                p3[0] = Bouncethree.Max(t => t.Y);
                p3[1] = Bouncethree.Max(t => t.X);

                g3[0] = Bouncethree.Min(t => t.Y);
                g3[1] = Bouncethree.Max(t => t.X);

                p4[0] = Bouncefour.Max(t => t.Y);
                p4[1] = Bouncefour.Max(t => t.X);

                g4[0] = Bouncefour.Min(t => t.Y);
                g4[1] = Bouncefour.Max(t => t.X);

                test.OneReturn = Math.Round(p1[0] - g1[0], 5);
                test.TwoReturn = Math.Round(p2[0] - g2[0], 5);
                test.ThreeReturn = Math.Round(p3[0] - g3[0], 5);
                test.FourReturn = Math.Round(p4[0] - g4[0], 5);
                Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                {


                    model.BounceOneAdSampling = new ObservableCollection<Point2D>(Bounceone);
                    model.BounceTwoAdSampling = new ObservableCollection<Point2D>(Bouncetwo);
                    model.BounceThreeAdSampling = new ObservableCollection<Point2D>(Bouncethree);
                    model.BounceFourAdSampling = new ObservableCollection<Point2D>(Bouncefour);

                    model.BounceOnePeak.Add(new Point2D { X = 0, Y = p1[0] });
                    model.BounceOnePeak.Add(new Point2D { X = p1[1], Y = p1[0] });
                    model.BounceOnePeak.Add(new Point2D { X = p1[1], Y = 0 });

                    model.BounceOneGrain.Add(new Point2D { X = -2, Y = g1[0] });
                    model.BounceOneGrain.Add(new Point2D { X = g1[1], Y = g1[0] });
                    model.BounceOneGrain.Add(new Point2D { X = g1[1], Y = 0 });

                    model.BounceTwoPeak.Add(new Point2D { X = 0, Y = p2[0] });
                    model.BounceTwoPeak.Add(new Point2D { X = p2[1], Y = p2[0] });
                    model.BounceTwoPeak.Add(new Point2D { X = p2[1], Y = 0 });

                    model.BounceTwoGrain.Add(new Point2D { X = 0, Y = g2[0] });
                    model.BounceTwoGrain.Add(new Point2D { X = g2[1], Y = g2[0] });
                    model.BounceTwoGrain.Add(new Point2D { X = g2[1], Y = 0 });

                    model.BounceThreePeak.Add(new Point2D { X = 0, Y = p3[0] });
                    model.BounceThreePeak.Add(new Point2D { X = p3[1], Y = p3[0] });
                    model.BounceThreePeak.Add(new Point2D { X = p3[1], Y = 0 });

                    model.BounceThreeGrain.Add(new Point2D { X = 0, Y = g3[0] });
                    model.BounceThreeGrain.Add(new Point2D { X = g3[1], Y = g3[0] });
                    model.BounceThreeGrain.Add(new Point2D { X = g3[1], Y = 0 });

                    model.BounceFourPeak.Add(new Point2D { X = 0, Y = p4[0] });
                    model.BounceFourPeak.Add(new Point2D { X = p4[1], Y = p4[0] });
                    model.BounceFourPeak.Add(new Point2D { X = p4[1], Y = 0 });

                    model.BounceFourGrain.Add(new Point2D { X = 0, Y = g4[0] });
                    model.BounceFourGrain.Add(new Point2D { X = g4[1], Y = g4[0] });
                    model.BounceFourGrain.Add(new Point2D { X = g4[1], Y = 0 });
                });

            }
            else if (numr == 1)//渲染1号
            {
                Bounceone.AddRange(t1);
                Bouncetwo.AddRange(t2);
                p1 = FindPeaks(doubles1, Wave.Wavecrest);
                g1 = FindPeaks(doubles1, Wave.Trough);

                p2 = FindPeaks(doubles2, Wave.Wavecrest);
                g2 = FindPeaks(doubles2, Wave.Trough);
                test.OneReturn = Math.Round(p1[0] - g1[0], 5);
                test.TwoReturn = Math.Round(p2[0] - g2[0], 5);
                Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    model.BounceOneAdSampling = new ObservableCollection<Point2D>(Bounceone);
                    model.BounceTwoAdSampling = new ObservableCollection<Point2D>(Bouncetwo);

                    model.BounceOnePeak.Add(new Point2D { X = 0, Y = p1[0] });
                    model.BounceOnePeak.Add(new Point2D { X = p1[1], Y = p1[0] });
                    model.BounceOnePeak.Add(new Point2D { X = p1[1], Y = 0 });

                    model.BounceOneGrain.Add(new Point2D { X = -2, Y = g1[0] });
                    model.BounceOneGrain.Add(new Point2D { X = g1[1], Y = g1[0] });
                    model.BounceOneGrain.Add(new Point2D { X = g1[1], Y = 0 });

                    model.BounceTwoPeak.Add(new Point2D { X = 0, Y = p2[0] });
                    model.BounceTwoPeak.Add(new Point2D { X = p2[1], Y = p2[0] });
                    model.BounceTwoPeak.Add(new Point2D { X = p2[1], Y = 0 });

                    model.BounceTwoGrain.Add(new Point2D { X = 0, Y = g2[0] });
                    model.BounceTwoGrain.Add(new Point2D { X = g2[1], Y = g2[0] });
                    model.BounceTwoGrain.Add(new Point2D { X = g2[1], Y = 0 });
                });

            }
            else if (numr == 2)//渲染2号
            {
                Bouncethree.AddRange(t3);
                Bouncefour.AddRange(t4);
                p3 = FindPeaks(doubles3, Wave.Wavecrest);
                g3 = FindPeaks(doubles3, Wave.Trough);

                p4 = FindPeaks(doubles4, Wave.Wavecrest);
                g4 = FindPeaks(doubles4, Wave.Trough);
                test.ThreeReturn = Math.Round(p3[0] - g3[0], 5);
                test.FourReturn = Math.Round(p4[0] - g4[0], 5);
                Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    model.BounceThreeAdSampling = new ObservableCollection<Point2D>(Bouncethree);
                    model.BounceFourAdSampling = new ObservableCollection<Point2D>(Bouncefour);
                    model.BounceThreePeak.Add(new Point2D { X = 0, Y = p3[0] });
                    model.BounceThreePeak.Add(new Point2D { X = p3[1], Y = p3[0] });
                    model.BounceThreePeak.Add(new Point2D { X = p3[1], Y = 0 });

                    model.BounceThreeGrain.Add(new Point2D { X = 0, Y = g3[0] });
                    model.BounceThreeGrain.Add(new Point2D { X = g3[1], Y = g3[0] });
                    model.BounceThreeGrain.Add(new Point2D { X = g3[1], Y = 0 });

                    model.BounceFourPeak.Add(new Point2D { X = 0, Y = p4[0] });
                    model.BounceFourPeak.Add(new Point2D { X = p4[1], Y = p4[0] });
                    model.BounceFourPeak.Add(new Point2D { X = p4[1], Y = 0 });

                    model.BounceFourGrain.Add(new Point2D { X = 0, Y = g4[0] });
                    model.BounceFourGrain.Add(new Point2D { X = g4[1], Y = g4[0] });
                    model.BounceFourGrain.Add(new Point2D { X = g4[1], Y = 0 });
                });
            }
            return test;
        UDP_GET_TimeOut://等待UDP数据包超时
            stopwatch.Stop();
            throw new Exception($"等待UDP(模拟量数据)返回数据超时！");
            return test;
        }
        #endregion

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ConfigurationFiles.thisfiles.AmendPath(model, "Arguments", model.ParameterNameList.Find(x => x.ID == model.ParameterIndexes).Name);
            ReTable(ParametName.Text);
        }

        private void ReTable(string ns)
        {
            ExcelTool.Read(ns);
            model.ArgumentsData.Clear();
            for (int i = 0; i < ExcelTool.Arguments.Count; i++)
            {
                model.ArgumentsData.Add(new Parameter { Name = ExcelTool.Arguments[i].TestName, Value = ExcelTool.Arguments[i].Value });
            }
        }

        private void Chip_Click(object sender, RoutedEventArgs e)
        {
            LoginSystem login = new LoginSystem();
            login.Show();
        }
    }
}

