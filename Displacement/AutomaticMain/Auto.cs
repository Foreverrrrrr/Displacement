using Displacement.FunctionCall;
using Displacement.ViewModels;
using Displacement.Views;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace Displacement.AutomaticMain
{
    public class Auto
    {

        static MainWindowViewModel model;
        public static Auto Instance { get; set; }

        static ConcurrentQueue<No4> no4s = new ConcurrentQueue<No4>();

        static ConcurrentQueue<No5> no5s = new ConcurrentQueue<No5>();

        static ConcurrentQueue<Test> Save_Test = new ConcurrentQueue<Test>();
        public static object On4_Lock = new object();
        public static object On5_Lock = new object();

        public struct Sql_SeverTest
        {
            /// <summary>
            /// 时间
            /// </summary>
            public DateTime Time { get; set; }
            /// <summary>
            /// 生产总数
            /// </summary>
            public float total_production { get; set; }
            /// <summary>
            /// NG数
            /// </summary>
            public float NG_number { get; set; }
            /// <summary>
            /// 良率
            /// </summary>
            public float yield { get; set; }
            /// <summary>
            /// ct
            /// </summary>
            public float ct_s { get; set; }
            /// <summary>
            /// 稼动率
            /// </summary>
            public float utilization { get; set; }
            /// <summary>
            /// 总运行时长
            /// </summary>
            public float total_running_time { get; set; }
            /// <summary>
            /// 正常运行时长
            /// </summary>
            public float normal_duration { get; set; }
            /// <summary>
            /// 异常运行时长
            /// </summary>
            public float bnormal_duration { get; set; }


            /// <summary>
            /// 载具号
            /// </summary>
            public int carrier_number { get; set; }
            /// <summary>
            /// 总结果
            /// </summary>
            public string total_result { get; set; }
            /// <summary>
            /// 一工位结果
            /// </summary>
            public string one_result { get; set; }
            /// <summary>
            /// 二工位结果
            /// </summary>
            public string two_result { get; set; }
            /// <summary>
            /// 三工位结果
            /// </summary>
            public string three_result { get; set; }
            /// <summary>
            /// 四工位结果
            /// </summary>
            public string four_result { get; set; }

            /// <summary>
            /// 四工位基准值
            /// </summary>
            public float four_standard { get; set; }

            /// <summary>
            /// 四工位串动值（mm）
            /// </summary>
            public float bunch_movement_value { get; set; }
            /// <summary>
            /// 四工位推位移设定上限（mm）
            /// </summary>
            public float push_movement_upper_limit { get; set; }
            /// <summary>
            /// 四工位推位移值
            /// </summary>
            public float push_movement_value { get; set; }
            /// <summary>
            /// 四工位推位移设定下限（mm）
            /// </summary>
            public float push_movement_lower_limit { get; set; }
            /// <summary>
            /// 四工位拉位移设定上限（mm）
            /// </summary>
            public float pull_movement_upper_limit { get; set; }
            /// <summary>
            /// 四工位拉位移值
            /// </summary>
            public float pull_movement_value { get; set; }

            /// <summary>
            /// 四工位拉位移设定下限（mm）
            /// </summary>
            public float pull_movement_lower_limit { get; set; }

            /// <summary>
            /// 四工位推力设定上限（N）
            /// </summary>
            public float push_force_upper_limit { get; set; }
            /// <summary>
            /// 四工位推力值（N）
            /// </summary>
            public float push_force_value { get; set; }
            /// <summary>
            /// 四工位推力设定下限（N）
            /// </summary>
            public float push_force_lower_limit { get; set; }
            /// <summary>
            /// 四工位拉力设定上限（N）
            /// </summary>
            public float pull_force_upper_limit { get; set; }
            /// <summary>
            /// 四工位拉力值（N）
            /// </summary>
            public float pull_force_value { get; set; }
            /// <summary>
            /// 四工位拉力设定下限（N）
            /// </summary>
            public float pull_force_lower_limit { get; set; }
            /// <summary>
            /// 五工位结果
            /// </summary>
            public string five_result { get; set; }

            /// <summary>
            /// 空载电流上限
            /// </summary>
            public float no_load_current_upper_limit { get; set; }

            /// <summary>
            /// 空载电流
            /// </summary>
            public float no_load_current { get; set; }
            /// <summary>
            /// 空载电流下限
            /// </summary>
            public float no_load_current_lower_limit { get; set; }

            /// <summary>
            /// 轴跳范围（mm）
            /// </summary>
            public float axis_jump_range { get; set; }
            /// <summary>
            /// 轴跳值(mm）
            /// </summary>
            public float axis_jump_value { get; set; }
            /// <summary>
            /// 蜗杆跳范围（mm）
            /// </summary>
            public float worm_jump_range { get; set; }
            /// <summary>
            /// 蜗杆跳值(mm）
            /// </summary>
            public float worm_jump_value { get; set; }

            /// <summary>
            /// 测试电压
            /// </summary>
            public float voltage { get; set; }
            
            /// <summary>
            /// 六工位注油结果
            /// </summary>
            public string six_result { get; set; }
            /// <summary>
            /// 注油量(g)
            /// </summary>
            public float greasing_value { get; set; }
            /// <summary>
            /// 注油速度(g/s)
            /// </summary>
            public float greasing_speed { get; set; }
            /// <summary>
            /// 七工位结果
            /// </summary>
            public string seven_result { get; set; }
            /// <summary>
            /// 八工位结果
            /// </summary>
            public string eight_result { get; set; }

        }
        public Auto()
        {
            Instance = this;
            model = MainWindowViewModel.thisModel;
            model.Auto_Threads[0] = new Thread(NO4_Auto_1);
            model.Auto_Threads[0].IsBackground = true;
            model.Auto_Threads[0].Name = "NO4";
            model.Auto_Threads[0].Start();

            model.Auto_Threads[1] = new Thread(NO5_Auto_1);
            model.Auto_Threads[1].IsBackground = true;
            model.Auto_Threads[1].Name = "NO5";
            model.Auto_Threads[1].Start();

            model.Auto_Threads[2] = new Thread(N06_1);
            model.Auto_Threads[2].IsBackground = true;
            model.Auto_Threads[2].Name = "NO6_1";
            model.Auto_Threads[2].Start();

            model.Auto_Threads[3] = new Thread(N06_2);
            model.Auto_Threads[3].IsBackground = true;
            model.Auto_Threads[3].Name = "NO6_2";
            model.Auto_Threads[3].Start();

            model.Auto_Threads[4] = new Thread(Rest);
            model.Auto_Threads[4].IsBackground = true;
            model.Auto_Threads[4].Start();
            model.Auto_Threads[5] = new Thread(N09);
            model.Auto_Threads[5].IsBackground = true;
            model.Auto_Threads[5].Name = "NO9";
            model.Auto_Threads[5].Start();
        }

        public static Sql_SeverTest[] GetSever(float[] floats)
        {
            Sql_SeverTest[] sql_Sever = new Sql_SeverTest[2];
            for (int i = 0; i < sql_Sever.Length; i++)
            {
                sql_Sever[i].Time = DateTime.Now;
                sql_Sever[i].total_production = (float)Math.Round(floats[40]);
                model.Total_Production = (int)sql_Sever[i].total_production;
                sql_Sever[i].NG_number = (float)Math.Round(floats[41], 2);
                model.NG_number = (int)sql_Sever[i].NG_number;
                sql_Sever[i].yield = (float)Math.Round(floats[42], 2);
                model.Yield = sql_Sever[i].yield;
                sql_Sever[i].ct_s = (float)Math.Round(floats[43], 2);
                model.Ct_S = sql_Sever[i].ct_s;
                sql_Sever[i].utilization = (float)Math.Round(floats[44], 2);
                model.Utilization = sql_Sever[i].utilization;
                sql_Sever[i].total_running_time = (float)Math.Round(floats[45], 2);
                model.Total_Running_Time = sql_Sever[i].total_running_time;
                sql_Sever[i].normal_duration = (float)Math.Round(floats[46], 2);
                model.Normal_Duration = sql_Sever[i].normal_duration;
                sql_Sever[i].bnormal_duration = (float)Math.Round(floats[47], 2);
                model.Bnormal_Duration = sql_Sever[i].bnormal_duration;
                sql_Sever[i].carrier_number = i + 1;
                sql_Sever[i].total_result = floats[38 + i] == 1 ? "Pass" : "NG";
                sql_Sever[i].one_result = floats[0 + i] == 1 ? "Pass" : "NG";
                sql_Sever[i].two_result = floats[2 + i] == 1 ? "Pass" : "NG";
                sql_Sever[i].three_result = floats[4 + i] == 1 ? "Pass" : "NG";
                sql_Sever[i].four_result = floats[6 + i] == 1 ? "Pass" : "NG";
                sql_Sever[i].four_standard = (float)Math.Round(floats[8 + i],3); 
                sql_Sever[i].bunch_movement_value = (float)Math.Round(floats[14 + i], 3);
                sql_Sever[i].push_movement_upper_limit = (float)Math.Round(MainWindowViewModel.thisModel.ArgumentsData[11 + i].Value, 3);
                sql_Sever[i].push_movement_value = (float)Math.Round(floats[10 + i], 3);
                sql_Sever[i].push_movement_lower_limit = (float)Math.Round(MainWindowViewModel.thisModel.ArgumentsData[13 + i].Value, 3);
                sql_Sever[i].pull_movement_upper_limit = (float)Math.Round(MainWindowViewModel.thisModel.ArgumentsData[15 + i].Value, 3);
                sql_Sever[i].pull_movement_value = (float)Math.Round(floats[12 + i], 3);
                sql_Sever[i].pull_movement_lower_limit = (float)Math.Round(MainWindowViewModel.thisModel.ArgumentsData[17 + i].Value, 3);
                sql_Sever[i].push_force_upper_limit = (float)Math.Round(MainWindowViewModel.thisModel.ArgumentsData[19 + (i * 4)].Value, 3);
                sql_Sever[i].push_force_value = (float)Math.Round(floats[16 + i], 3);
                sql_Sever[i].push_force_lower_limit = (float)Math.Round(MainWindowViewModel.thisModel.ArgumentsData[20 + (i * 4)].Value, 3);
                sql_Sever[i].pull_force_upper_limit = (float)Math.Round(MainWindowViewModel.thisModel.ArgumentsData[21 + (i * 4)].Value, 3);
                sql_Sever[i].pull_force_value = (float)Math.Round(floats[18 + i], 3);
                sql_Sever[i].pull_force_lower_limit = (float)Math.Round(MainWindowViewModel.thisModel.ArgumentsData[22 + (i * 4)].Value, 3);
                sql_Sever[i].five_result = floats[20 + i] == 1 ? "Pass" : "NG";
                sql_Sever[i].no_load_current_upper_limit = (float)Math.Round(MainWindowViewModel.thisModel.ArgumentsData[39].Value, 3);
                sql_Sever[i].no_load_current = (float)Math.Round(floats[26 + i], 3);
                sql_Sever[i].no_load_current_lower_limit = (float)Math.Round(MainWindowViewModel.thisModel.ArgumentsData[40].Value, 3);
                sql_Sever[i].axis_jump_range = (float)Math.Round(MainWindowViewModel.thisModel.ArgumentsData[41 + (i * 2)].Value, 3);
                sql_Sever[i].axis_jump_value = (float)Math.Round(floats[22 + i], 3);
                sql_Sever[i].worm_jump_range = (float)Math.Round(MainWindowViewModel.thisModel.ArgumentsData[42 + (i * 2)].Value, 3);
                sql_Sever[i].worm_jump_value = (float)Math.Round(floats[24 + i], 3);
                sql_Sever[i].voltage = (float)Math.Round(floats[28 + i], 3);
                sql_Sever[i].six_result = floats[20 + i] == 1 ? "Pass" : "NG";
                sql_Sever[i].greasing_value = (float)Math.Round(floats[32 + i], 4);
                sql_Sever[i].greasing_speed = (float)Math.Round(floats[34 + i], 4);
                sql_Sever[i].seven_result = floats[36 + i] == 1 ? "Pass" : "NG";
            }
            return sql_Sever;
        }

        private void N09()
        {
            while (true)
            {
                do
                {
                    ModbusTCP_PLC.thisModbus.Write("234", 0);
                    Thread.Sleep(100);
                } while (ModbusTCP_PLC.thisModbus.PLC_Control[17] == 1);
                do
                {
                    Thread.Sleep(100);
                } while (ModbusTCP_PLC.thisModbus.PLC_Control[17] != 1);
                ModbusTCP_PLC.thisModbus.Write("234", 0);
                float[] te = ModbusTCP_PLC.thisModbus.ReadSave();
                var jk = Auto.GetSever(te);
                SQL_Server.Write(jk[0]);
                SQL_Server.Write(jk[1]);
                ExcelTool.SaveData(te);
                ModbusTCP_PLC.thisModbus.Write("192", 1);
                Log.LogVision("九工位数据绑定完成！");
                for (int i = 236; i < 20; i += 2)
                {
                    ModbusTCP_PLC.thisModbus.Write(i.ToString(), 0);
                }
                do
                {
                    ModbusTCP_PLC.thisModbus.Write("234", 0);
                    Thread.Sleep(100);
                } while (ModbusTCP_PLC.thisModbus.PLC_Control[17] != 0);
            }
        }

        private void Rest()
        {
            while (true)
            {
                do
                {
                    Thread.Sleep(100);
                } while (ModbusTCP_PLC.thisModbus.PLC_Control[16] == 1);
                do
                {
                    Thread.Sleep(100);
                } while (ModbusTCP_PLC.thisModbus.PLC_Control[16] != 1);
                Log.LogVision("数据缓存队列清空触发");
                if (no4s != null)
                {
                    Log.LogVision($"四工位缓存队列剩余{no4s.Count}条数据");
                    no4s = new ConcurrentQueue<No4>();
                }
                if (Save_Test != null)
                {
                    Log.LogVision($"总缓存队列剩余{Save_Test.Count}条数据");
                    Save_Test = new ConcurrentQueue<Test>();
                }
                ModbusTCP_PLC.thisModbus.Write("232", 0);
                Log.LogVision("数据缓存队列清空完成");
            }

            //throw new NotImplementedException();
        }

        /// <summary>
        /// 四工位自动流程
        /// </summary>
        public void NO4_Auto_1()
        {
            ModbusTCP_PLC.thisModbus.Write("204", 0);
            ModbusTCP_PLC.thisModbus.Write("206", 0);
            ModbusTCP_PLC.thisModbus.Write("210", 0);
            ModbusTCP_PLC.thisModbus.Write("212", 0);
            ModbusTCP_PLC.thisModbus.Write("217", 0);
            ModbusTCP_PLC.thisModbus.Write("216", 0);
            double[] criterion = new double[2];//基准值
            double[,] measuredvalue = new double[2, 4];//测量值

            while (true)
            {
                try
                {
                    Stopwatch stopwatch = new Stopwatch();
                    Thread.Sleep(100);
                    if (ModbusTCP_PLC.thisModbus.IsConnet)
                    {
                        bool[] bools = new bool[8];
                        List<double> SpOne = new List<double>();//1号基准
                        List<double> SpTwo = new List<double>();//2号基准
                        List<double> OnePushRoute = new List<double>();//1号推行程
                        List<double> OnePullForce = new List<double>();//1号推力
                        List<double> TwoPushRoute = new List<double>();//2号推行程
                        List<double> TwoPullForce = new List<double>();//2号推力
                        No4 no4 = new No4();
                        do
                        {
                            if (ModbusTCP_PLC.thisModbus.PLC_Control[10] == 1)
                                goto Rest;
                            Thread.Sleep(100);
                        } while (ModbusTCP_PLC.thisModbus.PLC_Control[2] == 1);
                        do
                        {
                            if (ModbusTCP_PLC.thisModbus.PLC_Control[10] == 1)
                                goto Rest;
                            Thread.Sleep(100);
                        } while (ModbusTCP_PLC.thisModbus.PLC_Control[2] != 1);//基准点采集开始
                        bool one = false;
                        bool two = false;
                        ModbusTCP_PLC.thisModbus.Write("204", 0);
                        Log.LogVision("四工位基准采集触发");
                        //model.MessageLog($"基准采集触发开始，类型{ModbusTCP_PLC.thisModbus.PLC_Control[6]}");
                        if (ModbusTCP_PLC.thisModbus.PLC_Control[6] == 1)
                        {
                            Log.LogVision("四工位测试模式两边OK");
                            one = true;
                            two = true;
                        }
                        else if (ModbusTCP_PLC.thisModbus.PLC_Control[6] == 2)
                        {
                            Log.LogVision("四工位测试模式1号OK");

                            one = true;
                            two = false;
                        }
                        else if (ModbusTCP_PLC.thisModbus.PLC_Control[6] == 3)
                        {
                            Log.LogVision("四工位测试模式2号OK");
                            one = false;
                            two = true;
                        }
                        else if (ModbusTCP_PLC.thisModbus.PLC_Control[6] == 4)
                        {
                            Log.LogVision("四工位测试模式两边NG");

                            no4.Time = DateTime.Now.ToString();
                            no4.OneResult = "NULL";
                            no4.TwoResult = "NULL";
                            ModbusTCP_PLC.thisModbus.Write("212", 0);
                            ModbusTCP_PLC.thisModbus.Write("204", 0);
                            if (ModbusTCP_PLC.thisModbus.PLC_Control[11] == 1)
                            {
                                lock (On4_Lock)
                                {
                                    no4s.Enqueue(no4);
                                }
                            }
                            Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                            {
                                if (model.No4TestResult.Count > 100)
                                {
                                    model.No4TestResult.RemoveAt(model.No4TestResult.Count - 1);
                                    model.No4TestResult.Insert(0, no4);
                                }
                                else
                                {
                                    model.No4TestResult.Insert(0, no4);
                                }
                            });
                            continue;
                        }
                        ModbusTCP_PLC.thisModbus.Write("212", 0);
                        no4.Time = DateTime.Now.ToString();
                        Stopwatch stopwatch1 = new Stopwatch();
                        stopwatch1.Start();
                        stopwatch.Start();
                        do
                        {
                            if (MainWindow.realtime[4] != 0 && one)
                                SpOne.Add(MainWindow.realtime[4]);
                            if (MainWindow.realtime[5] != 0 && two)
                                SpTwo.Add(MainWindow.realtime[5]);
                            Thread.Sleep(30);
                        } while (stopwatch.ElapsedMilliseconds <= model.ArgumentsData[8].Value);
                        Log.LogVision("四工位测试采集基准位移数据完成");

                        stopwatch.Stop();
                        model.ArgumentsData[2].Value += MainWindow.realtime[7];
                        model.ArgumentsData[3].Value += MainWindow.realtime[6];
                        ModbusTCP_PLC.thisModbus.Write("204", 0);
                        ModbusTCP_PLC.thisModbus.Write("212", 0);
                        if (one && two)//全部采集
                        {
                            if (SpOne.Count > 0 && SpTwo.Count > 0)
                            {
                                criterion[0] = Geometry.SampleMode(SpOne.ToArray());
                                criterion[1] = Geometry.SampleMode(SpTwo.ToArray());
                                ModbusTCP_PLC.thisModbus.Write("194", criterion[0]);
                                ModbusTCP_PLC.thisModbus.Write("196", criterion[1]);

                                ModbusTCP_PLC.thisModbus.Write("120", 1);
                                ModbusTCP_PLC.thisModbus.Write("134", 1);
                                do
                                {
                                    if (ModbusTCP_PLC.thisModbus.PLC_Control[10] == 1)
                                        goto Rest;
                                    Thread.Sleep(50);
                                } while (ModbusTCP_PLC.thisModbus.PLC_Control[3] != 1 && ModbusTCP_PLC.thisModbus.PLC_Control[7] != 1);//推动采集开始
                                Log.LogVision("四工位测试推动采集触发");

                                stopwatch.Restart();
                                do
                                {
                                    if (MainWindow.realtime[4] > 0)
                                        OnePushRoute.Add(MainWindow.realtime[4]);
                                    if (MainWindow.realtime[5] > 0)
                                        TwoPushRoute.Add(MainWindow.realtime[5]);
                                    if (MainWindow.realtime[6] > 0)
                                        OnePullForce.Add(MainWindow.realtime[6]);
                                    if (MainWindow.realtime[7] > 0)
                                        TwoPullForce.Add(MainWindow.realtime[7]);
                                    Thread.Sleep(30);
                                } while (stopwatch.ElapsedMilliseconds <= model.ArgumentsData[9].Value);
                                stopwatch.Stop();
                                ModbusTCP_PLC.thisModbus.Write("206", 0);
                                ModbusTCP_PLC.thisModbus.Write("214", 0);
                                if (OnePushRoute.Count > 0 && TwoPushRoute.Count > 0 && OnePullForce.Count > 0 && TwoPullForce.Count > 0)
                                {
                                    measuredvalue[0, 0] = Geometry.SampleMode(OnePushRoute.ToArray()) - criterion[0];
                                    measuredvalue[0, 1] = Geometry.SampleMode(OnePullForce.ToArray());
                                    measuredvalue[1, 0] = Geometry.SampleMode(TwoPushRoute.ToArray()) - criterion[1];
                                    measuredvalue[1, 1] = Geometry.SampleMode(TwoPullForce.ToArray());
                                    no4.PushOneMove = Math.Round(measuredvalue[0, 0], 3);
                                    no4.PushTwoMove = Math.Round(measuredvalue[1, 0], 3);
                                    no4.PushOneForce = Math.Round(measuredvalue[0, 1], 3);
                                    no4.PushTwoForce = Math.Round(measuredvalue[1, 1], 3);
                                    ModbusTCP_PLC.thisModbus.Write("144", no4.PushOneMove);
                                    ModbusTCP_PLC.thisModbus.Write("146", no4.PushTwoMove);
                                    ModbusTCP_PLC.thisModbus.Write("156", no4.PushOneForce);
                                    ModbusTCP_PLC.thisModbus.Write("158", no4.PushTwoForce);
                                    ModbusTCP_PLC.thisModbus.Write("122", 1);
                                    ModbusTCP_PLC.thisModbus.Write("136", 1);
                                    Log.LogVision("四工位测试推动采集数据完成");

                                    if (measuredvalue[0, 0] >= model.ArgumentsData[13].Value && measuredvalue[0, 0] <= model.ArgumentsData[11].Value)
                                    {
                                        no4.PushOneMoveResult = "Pass";
                                        bools[0] = true;
                                        //1推位移ok
                                    }
                                    else
                                    {
                                        no4.PushOneMoveResult = "NG";
                                        bools[0] = false;
                                    }
                                    if (measuredvalue[0, 1] >= model.ArgumentsData[20].Value && measuredvalue[0, 1] <= model.ArgumentsData[19].Value)
                                    {
                                        no4.PushOneForceResult = "Pass";
                                        //1推力ok
                                        bools[1] = true;
                                    }
                                    else
                                    {
                                        no4.PushOneForceResult = "NG";
                                        bools[1] = false;
                                    }
                                    if (measuredvalue[1, 0] >= model.ArgumentsData[14].Value && measuredvalue[1, 0] <= model.ArgumentsData[12].Value)
                                    {
                                        no4.PushTwoMoveResult = "Pass";
                                        //2推位移ok
                                        bools[4] = true;
                                    }
                                    else
                                    {
                                        no4.PushTwoMoveResult = "NG";
                                        bools[4] = false;
                                    }
                                    if (measuredvalue[1, 1] >= model.ArgumentsData[24].Value && measuredvalue[1, 1] <= model.ArgumentsData[23].Value)
                                    {
                                        no4.PushTwoForceResult = "Pass";
                                        bools[5] = true;
                                        //2推力ok
                                    }
                                    else
                                    {
                                        no4.PushTwoForceResult = "NG";
                                        bools[5] = false;
                                    }
                                    if (bools[0] && bools[1])
                                    {
                                        ModbusTCP_PLC.thisModbus.Write("126", 1);
                                    }
                                    else
                                    {
                                        ModbusTCP_PLC.thisModbus.Write("126", 2);

                                    }
                                    if (bools[4] && bools[5])
                                    {
                                        ModbusTCP_PLC.thisModbus.Write("128", 1);
                                    }
                                    else
                                    {
                                        ModbusTCP_PLC.thisModbus.Write("128", 2);
                                    }
                                }
                                else
                                {
                                    ModbusTCP_PLC.thisModbus.Write("126", 2);
                                    ModbusTCP_PLC.thisModbus.Write("128", 2);
                                    throw new Exception("推动采集时间过小导致采集数据不足，无法进行运算");
                                }
                                OnePushRoute.Clear();
                                TwoPushRoute.Clear();
                                OnePullForce.Clear();
                                TwoPullForce.Clear();
                                do
                                {
                                    if (ModbusTCP_PLC.thisModbus.PLC_Control[10] == 1)
                                        goto Rest;
                                    Thread.Sleep(50);
                                } while (ModbusTCP_PLC.thisModbus.PLC_Control[5] != 1 && ModbusTCP_PLC.thisModbus.PLC_Control[8] != 1);//拉动采集开始
                                Log.LogVision("四工位测试拉动采集数据开始");
                                Stopwatch stopwatch2 = new Stopwatch();
                                stopwatch2.Start();
                                do
                                {
                                    if (ModbusTCP_PLC.thisModbus.PLC_Control[10] == 1)
                                        goto Rest;
                                    Thread.Sleep(30);
                                    //if (stopwatch2.ElapsedMilliseconds >= 200)
                                    //    break;
                                } while (MainWindow.realtime[6] == 0 && MainWindow.realtime[7] == 0);
                                stopwatch.Restart();
                                {
                                    if (MainWindow.realtime[4] > 0)
                                        OnePushRoute.Add(MainWindow.realtime[4]);
                                    if (MainWindow.realtime[5] > 0)
                                        TwoPushRoute.Add(MainWindow.realtime[5]);
                                    if (MainWindow.realtime[6] != 0)
                                        OnePullForce.Add(MainWindow.realtime[6]);
                                    if (MainWindow.realtime[7] != 0)
                                        TwoPullForce.Add(MainWindow.realtime[7]);
                                    Thread.Sleep(30);
                                } while (stopwatch.ElapsedMilliseconds <= model.ArgumentsData[10].Value) ;
                                stopwatch.Stop();
                                ModbusTCP_PLC.thisModbus.Write("210", 0);
                                ModbusTCP_PLC.thisModbus.Write("216", 0);
                                Log.LogVision("四工位测试拉动采集数据完成");

                                if (OnePushRoute.Count > 0 && TwoPushRoute.Count > 0 && OnePullForce.Count > 0 && TwoPullForce.Count > 0)
                                {
                                    measuredvalue[0, 2] = Geometry.SampleMode(OnePushRoute.ToArray()) - criterion[0];
                                    measuredvalue[0, 3] = Geometry.SampleMode(OnePullForce.ToArray());
                                    measuredvalue[1, 2] = Geometry.SampleMode(TwoPushRoute.ToArray()) - criterion[1];
                                    measuredvalue[1, 3] = Geometry.SampleMode(TwoPullForce.ToArray());
                                    no4.PullOneMove = Math.Round(measuredvalue[0, 2], 3);
                                    no4.PullTwoMove = Math.Round(measuredvalue[1, 2], 3);
                                    no4.PullOneForce = Math.Round(measuredvalue[0, 3], 3);
                                    no4.PullTwoForce = Math.Round(measuredvalue[1, 3], 3);
                                    no4.One_bunch_movement = Math.Round(no4.PushOneMove - no4.PullOneMove, 3);
                                    no4.Two_bunch_movement = Math.Round(no4.PushTwoMove - no4.PullTwoMove, 3);
                                    ModbusTCP_PLC.thisModbus.Write("148", no4.PullOneMove);
                                    ModbusTCP_PLC.thisModbus.Write("150", no4.PullTwoMove);
                                    ModbusTCP_PLC.thisModbus.Write("160", no4.PullOneForce);
                                    ModbusTCP_PLC.thisModbus.Write("162", no4.PullTwoForce);
                                    ModbusTCP_PLC.thisModbus.Write("152", no4.One_bunch_movement);
                                    ModbusTCP_PLC.thisModbus.Write("154", no4.Two_bunch_movement);
                                    ModbusTCP_PLC.thisModbus.Write("124", 1);
                                    ModbusTCP_PLC.thisModbus.Write("138", 1);
                                    if (measuredvalue[0, 2] >= model.ArgumentsData[17].Value && measuredvalue[0, 2] <= model.ArgumentsData[15].Value)
                                    {
                                        no4.PullOneMoveResult = "Pass";
                                        bools[2] = true;
                                        //1拉位移ok
                                    }
                                    else
                                    {
                                        no4.PullOneMoveResult = "NG";
                                        bools[2] = false;
                                    }
                                    if (measuredvalue[0, 3] >= model.ArgumentsData[22].Value && measuredvalue[0, 3] <= model.ArgumentsData[21].Value)
                                    {
                                        no4.PullOneForceResult = "Pass";
                                        bools[3] = true;
                                        //1拉力ok
                                    }
                                    else
                                    {
                                        no4.PullOneForceResult = "NG";
                                        bools[3] = false;
                                    }
                                    if (measuredvalue[1, 2] >= model.ArgumentsData[18].Value && measuredvalue[1, 2] <= model.ArgumentsData[16].Value)
                                    {
                                        no4.PullTwoMoveResult = "Pass";
                                        bools[6] = true;
                                        //2拉位移ok
                                    }
                                    else
                                    {
                                        no4.PullTwoMoveResult = "NG";
                                        bools[6] = false;
                                    }
                                    if (measuredvalue[1, 3] >= model.ArgumentsData[26].Value && measuredvalue[1, 3] <= model.ArgumentsData[25].Value)
                                    {
                                        no4.PullTwoForceResult = "Pass";
                                        bools[7] = true;
                                        //2拉力ok
                                    }
                                    else
                                    {
                                        no4.PullTwoForceResult = "NG";
                                        bools[7] = false;
                                    }
                                    if (bools[2] && bools[3])
                                    {
                                        ModbusTCP_PLC.thisModbus.Write("130", 1);
                                    }
                                    else
                                    {
                                        ModbusTCP_PLC.thisModbus.Write("130", 2);

                                    }
                                    if (bools[6] && bools[7])
                                    {
                                        ModbusTCP_PLC.thisModbus.Write("132", 1);
                                    }
                                    else
                                    {
                                        ModbusTCP_PLC.thisModbus.Write("132", 2);
                                    }
                                    for (int i = 0; i < bools.Length / 2; i++)
                                    {
                                        if (!bools[i])
                                        {
                                            no4.OneResult = "NG";
                                            break;
                                        }
                                        else
                                        {
                                            no4.OneResult = "Pass";

                                        }
                                    }
                                    for (int i = 4; i < bools.Length; i++)
                                    {
                                        if (!bools[i])
                                        {
                                            no4.TwoResult = "NG";
                                            break;
                                        }
                                        else
                                        {
                                            no4.TwoResult = "Pass";
                                        }
                                    }
                                    if (ModbusTCP_PLC.thisModbus.PLC_Control[11] == 2)
                                        ExcelTool.SaveDataN4(no4);
                                    else if ((ModbusTCP_PLC.thisModbus.PLC_Control[11] == 1))
                                    {
                                        //model.MessageLog("四工位数据缓存");
                                        lock (On4_Lock)
                                        {
                                            no4s.Enqueue(no4);
                                        }

                                    }
                                    Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                                    {
                                        if (model.No4TestResult.Count > 100)
                                        {
                                            model.No4TestResult.RemoveAt(model.No4TestResult.Count - 1);
                                            model.No4TestResult.Insert(0, no4);
                                        }
                                        else
                                        {
                                            model.No4TestResult.Insert(0, no4);
                                        }
                                    });
                                    stopwatch1.Stop();
                                    model.Auto4CT = stopwatch1.ElapsedMilliseconds.ToString();
                                }
                                else
                                {
                                    ModbusTCP_PLC.thisModbus.Write("130", 2);
                                    ModbusTCP_PLC.thisModbus.Write("132", 2);
                                    no4.OneResult = "NULL";
                                    no4.TwoResult = "NULL";
                                    if (ModbusTCP_PLC.thisModbus.PLC_Control[11] == 2)
                                        ExcelTool.SaveDataN4(no4);
                                    else if ((ModbusTCP_PLC.thisModbus.PLC_Control[11] == 1))
                                    {
                                        //model.MessageLog("四工位数据缓存");
                                        lock (On4_Lock)
                                        {
                                            no4s.Enqueue(no4);
                                        }

                                    }
                                    Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                                    {
                                        if (model.No4TestResult.Count > 100)
                                        {
                                            model.No4TestResult.RemoveAt(model.No4TestResult.Count - 1);
                                            model.No4TestResult.Insert(0, no4);
                                        }
                                        else
                                        {
                                            model.No4TestResult.Insert(0, no4);
                                        }
                                    });
                                    // throw new Exception("拉动采集时间过小导致采集数据不足，无法进行运算");
                                }
                            }
                            else
                            {
                                no4.OneResult = "NULL";
                                no4.TwoResult = "NULL";
                                if (ModbusTCP_PLC.thisModbus.PLC_Control[11] == 2)
                                    ExcelTool.SaveDataN4(no4);
                                else if ((ModbusTCP_PLC.thisModbus.PLC_Control[11] == 1))
                                {
                                    //model.MessageLog("四工位数据缓存");
                                    lock (On4_Lock)
                                    {
                                        no4s.Enqueue(no4);
                                    }

                                }
                                Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                                {
                                    if (model.No4TestResult.Count > 100)
                                    {
                                        model.No4TestResult.RemoveAt(model.No4TestResult.Count - 1);
                                        model.No4TestResult.Insert(0, no4);
                                    }
                                    else
                                    {
                                        model.No4TestResult.Insert(0, no4);
                                    }
                                });
                            }
                        }
                        else if (one && !two)
                        {
                            if (SpOne.Count > 0)
                            {
                                criterion[0] = Geometry.SampleMode(SpOne.ToArray());
                                ModbusTCP_PLC.thisModbus.Write("194", criterion[0]);
                                ModbusTCP_PLC.thisModbus.Write("120", 1);
                                do
                                {
                                    if (ModbusTCP_PLC.thisModbus.PLC_Control[10] == 1)
                                        goto Rest;
                                    Thread.Sleep(100);
                                } while (ModbusTCP_PLC.thisModbus.PLC_Control[3] != 1);//推动采集开始
                                Log.LogVision("四工位测试推动采集数据触发");

                                stopwatch.Restart();
                                do
                                {
                                    if (MainWindow.realtime[4] != 0)
                                        OnePushRoute.Add(MainWindow.realtime[4]);

                                    if (MainWindow.realtime[6] != 0)
                                        OnePullForce.Add(MainWindow.realtime[6]);

                                    Thread.Sleep(30);
                                } while (stopwatch.ElapsedMilliseconds <= model.ArgumentsData[9].Value);

                                stopwatch.Stop();
                                ModbusTCP_PLC.thisModbus.Write("206", 0);
                                if (OnePushRoute.Count > 0 && OnePullForce.Count > 0)
                                {
                                    measuredvalue[0, 0] = Geometry.SampleMode(OnePushRoute.ToArray()) - criterion[0];
                                    measuredvalue[0, 1] = Geometry.SampleMode(OnePullForce.ToArray());
                                    no4.PushOneMove = Math.Round(measuredvalue[0, 0], 3);
                                    no4.PushOneForce = Math.Round(measuredvalue[0, 1], 3);
                                    ModbusTCP_PLC.thisModbus.Write("144", no4.PushOneMove);
                                    ModbusTCP_PLC.thisModbus.Write("156", no4.PushOneForce);
                                    ModbusTCP_PLC.thisModbus.Write("122", 1);
                                    Log.LogVision("四工位测试推动采集数据完成");

                                    if (measuredvalue[0, 0] >= model.ArgumentsData[13].Value && measuredvalue[0, 0] <= model.ArgumentsData[11].Value)
                                    {
                                        no4.PushOneMoveResult = "Pass";
                                        bools[0] = true;
                                        //1推位移ok
                                    }
                                    else
                                    {
                                        no4.PushOneMoveResult = "NG";
                                        bools[0] = false;
                                    }
                                    if (measuredvalue[0, 1] >= model.ArgumentsData[20].Value && measuredvalue[0, 1] <= model.ArgumentsData[19].Value)
                                    {
                                        no4.PushOneForceResult = "Pass";
                                        //1推力ok
                                        bools[1] = true;
                                    }
                                    else
                                    {
                                        no4.PushOneForceResult = "NG";
                                        bools[1] = false;
                                    }
                                    if (bools[0] && bools[1])
                                    {
                                        ModbusTCP_PLC.thisModbus.Write("126", 1);
                                    }
                                    else
                                    {
                                        ModbusTCP_PLC.thisModbus.Write("126", 2);

                                    }
                                }
                                else
                                {
                                    ModbusTCP_PLC.thisModbus.Write("126", 2);
                                    throw new Exception("推动采集时间过小导致采集数据不足，无法进行运算");
                                }
                                OnePushRoute.Clear();
                                TwoPushRoute.Clear();
                                OnePullForce.Clear();
                                TwoPullForce.Clear();
                                do
                                {
                                    if (ModbusTCP_PLC.thisModbus.PLC_Control[10] == 1)
                                        goto Rest;
                                    Thread.Sleep(100);
                                } while (ModbusTCP_PLC.thisModbus.PLC_Control[5] != 1);//拉动采集开始
                                Log.LogVision("四工位测试拉动采集数据触发");

                                do
                                {
                                    if (ModbusTCP_PLC.thisModbus.PLC_Control[10] == 1)
                                        goto Rest;
                                    Thread.Sleep(30);
                                } while (MainWindow.realtime[6] == 0);
                                stopwatch.Restart();
                                do
                                {
                                    if (MainWindow.realtime[4] != 0)
                                        OnePushRoute.Add(MainWindow.realtime[4]);
                                    if (MainWindow.realtime[6] < 0)
                                        OnePullForce.Add(MainWindow.realtime[6]);
                                    Thread.Sleep(30);
                                } while (stopwatch.ElapsedMilliseconds <= model.ArgumentsData[10].Value);
                                Log.LogVision("四工位测试拉动采集数据完成");

                                stopwatch.Stop();
                                ModbusTCP_PLC.thisModbus.Write("210", 0);
                                if (OnePushRoute.Count > 0 && OnePullForce.Count > 0)
                                {
                                    measuredvalue[0, 2] = Geometry.SampleMode(OnePushRoute.ToArray()) - criterion[0];
                                    measuredvalue[0, 3] = Geometry.SampleMode(OnePullForce.ToArray());
                                    no4.PullOneMove = Math.Round(measuredvalue[0, 2], 3);
                                    no4.PullOneForce = Math.Round(measuredvalue[0, 3], 3);
                                    no4.One_bunch_movement = Math.Round(no4.PushOneMove - no4.PullOneMove, 3);


                                    ModbusTCP_PLC.thisModbus.Write("148", no4.PullOneMove);
                                    ModbusTCP_PLC.thisModbus.Write("160", no4.PullOneForce);
                                    ModbusTCP_PLC.thisModbus.Write("152", no4.PushOneMove - no4.PullOneMove);

                                    ModbusTCP_PLC.thisModbus.Write("124", 1);
                                    if (measuredvalue[0, 2] >= model.ArgumentsData[17].Value && measuredvalue[0, 2] <= model.ArgumentsData[15].Value)
                                    {
                                        no4.PullOneMoveResult = "Pass";
                                        bools[2] = true;
                                        //1拉位移ok
                                    }
                                    else
                                    {
                                        no4.PullOneMoveResult = "NG";
                                        bools[2] = false;
                                    }
                                    if (measuredvalue[0, 3] >= model.ArgumentsData[22].Value && measuredvalue[0, 3] <= model.ArgumentsData[21].Value)
                                    {
                                        bools[3] = true;
                                        //1拉力ok
                                    }
                                    else
                                    {
                                        no4.PullOneForceResult = "NG";
                                        bools[3] = false;
                                    }
                                    if (bools[2] && bools[3])
                                    {
                                        ModbusTCP_PLC.thisModbus.Write("130", 1);
                                    }
                                    else
                                    {
                                        ModbusTCP_PLC.thisModbus.Write("130", 2);

                                    }
                                    for (int i = 0; i < bools.Length / 2; i++)
                                    {
                                        if (!bools[i])
                                        {
                                            no4.OneResult = "NG";
                                            break;
                                        }
                                        else
                                        {
                                            no4.OneResult = "Pass";

                                        }
                                    }
                                    no4.TwoResult = "NULL";
                                    if (ModbusTCP_PLC.thisModbus.PLC_Control[11] == 2)
                                        ExcelTool.SaveDataN4(no4);
                                    else if ((ModbusTCP_PLC.thisModbus.PLC_Control[11] == 1))
                                    {
                                        //model.MessageLog("四工位数据缓存");
                                        lock (On4_Lock)
                                        {
                                            no4s.Enqueue(no4);
                                        }
                                    }
                                    Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                                    {
                                        if (model.No4TestResult.Count > 100)
                                        {
                                            model.No4TestResult.RemoveAt(model.No4TestResult.Count - 1);
                                            model.No4TestResult.Insert(0, no4);
                                        }
                                        else
                                        {
                                            model.No4TestResult.Insert(0, no4);
                                        }
                                    });
                                    stopwatch1.Stop();
                                    model.Auto4CT = stopwatch1.ElapsedMilliseconds.ToString();
                                }
                                else
                                {
                                    ModbusTCP_PLC.thisModbus.Write("124", 1);
                                    ModbusTCP_PLC.thisModbus.Write("130", 2);
                                    ModbusTCP_PLC.thisModbus.Write("132", 2);
                                    throw new Exception("拉动采集时间过小导致采集数据不足，无法进行运算");
                                }
                            }
                            else
                            {
                                throw new Exception("基准点采集时间过小导致采集数据不足，无法进行运算");
                            }
                        }
                        else if (!one && two)
                        {
                            if (SpTwo.Count > 0)
                            {
                                criterion[1] = Geometry.SampleMode(SpTwo.ToArray());
                                ModbusTCP_PLC.thisModbus.Write("196", criterion[1]);
                                ModbusTCP_PLC.thisModbus.Write("134", 1);
                                do
                                {
                                    if (ModbusTCP_PLC.thisModbus.PLC_Control[10] == 1)
                                        goto Rest;
                                    Thread.Sleep(100);
                                } while (ModbusTCP_PLC.thisModbus.PLC_Control[7] != 1);//推动采集开始
                                Log.LogVision("四工位测试推动采集数据触发");
                                stopwatch.Restart();
                                do
                                {

                                    if (MainWindow.realtime[5] != 0)
                                        TwoPushRoute.Add(MainWindow.realtime[5]);
                                    if (MainWindow.realtime[7] != 0)
                                        TwoPullForce.Add(MainWindow.realtime[7]);
                                    Thread.Sleep(30);
                                } while (stopwatch.ElapsedMilliseconds <= model.ArgumentsData[9].Value);
                                stopwatch.Stop();
                                ModbusTCP_PLC.thisModbus.Write("214", 0);
                                if (TwoPushRoute.Count > 0 && TwoPullForce.Count > 0)
                                {
                                    measuredvalue[1, 0] = Geometry.SampleMode(TwoPushRoute.ToArray()) - criterion[1];
                                    measuredvalue[1, 1] = Geometry.SampleMode(TwoPullForce.ToArray());
                                    no4.PushTwoMove = Math.Round(measuredvalue[1, 0], 3);
                                    no4.PushTwoForce = Math.Round(measuredvalue[1, 1], 3);
                                    ModbusTCP_PLC.thisModbus.Write("146", no4.PushTwoMove);
                                    ModbusTCP_PLC.thisModbus.Write("158", no4.PushTwoForce);
                                    ModbusTCP_PLC.thisModbus.Write("136", 1);
                                    Log.LogVision("四工位测试推动采集数据完成");
                                    if (measuredvalue[1, 0] >= model.ArgumentsData[14].Value && measuredvalue[1, 0] <= model.ArgumentsData[12].Value)
                                    {
                                        no4.PushTwoMoveResult = "Pass";
                                        //2推位移ok
                                        bools[0] = true;
                                    }
                                    else
                                    {
                                        no4.PushTwoMoveResult = "NG";
                                        bools[0] = false;
                                    }
                                    if (measuredvalue[1, 1] >= model.ArgumentsData[24].Value && measuredvalue[1, 1] <= model.ArgumentsData[23].Value)
                                    {
                                        no4.PushTwoForceResult = "Pass";
                                        bools[1] = true;
                                        //2推力ok
                                    }
                                    else
                                    {
                                        no4.PushTwoForceResult = "NG";
                                        bools[1] = false;
                                    }
                                    if (bools[0] && bools[1])
                                    {
                                        ModbusTCP_PLC.thisModbus.Write("128", 1);
                                    }
                                    else
                                    {
                                        ModbusTCP_PLC.thisModbus.Write("128", 2);

                                    }
                                }
                                else
                                {
                                    ModbusTCP_PLC.thisModbus.Write("128", 2);
                                    throw new Exception("推动采集时间过小导致采集数据不足，无法进行运算");
                                }
                                OnePushRoute.Clear();
                                TwoPushRoute.Clear();
                                OnePullForce.Clear();
                                TwoPullForce.Clear();
                                do
                                {
                                    if (ModbusTCP_PLC.thisModbus.PLC_Control[10] == 1)
                                        goto Rest;
                                    Thread.Sleep(100);
                                } while (ModbusTCP_PLC.thisModbus.PLC_Control[8] != 1);//拉动采集开始
                                Log.LogVision("四工位测试拉动采集数据触发");

                                do
                                {
                                    if (ModbusTCP_PLC.thisModbus.PLC_Control[10] == 1)
                                        goto Rest;
                                    Thread.Sleep(30);
                                } while (MainWindow.realtime[7] == 0);
                                stopwatch.Restart();
                                do
                                {

                                    if (MainWindow.realtime[5] != 0)
                                        TwoPushRoute.Add(MainWindow.realtime[5]);
                                    if (MainWindow.realtime[7] < 0)
                                        TwoPullForce.Add(MainWindow.realtime[7]);
                                    Thread.Sleep(30);
                                } while (stopwatch.ElapsedMilliseconds <= model.ArgumentsData[10].Value);
                                stopwatch.Stop();
                                ModbusTCP_PLC.thisModbus.Write("216", 0);
                                if (TwoPushRoute.Count > 0 && TwoPullForce.Count > 0)
                                {
                                    measuredvalue[1, 2] = Geometry.SampleMode(TwoPushRoute.ToArray()) - criterion[1];
                                    measuredvalue[1, 3] = Geometry.SampleMode(TwoPullForce.ToArray());
                                    no4.PullTwoMove = Math.Round(measuredvalue[1, 2], 3);
                                    no4.PullTwoForce = Math.Round(measuredvalue[1, 3], 3);
                                    no4.Two_bunch_movement = Math.Round(no4.PushTwoMove - no4.PullTwoMove, 3);

                                    ModbusTCP_PLC.thisModbus.Write("150", no4.PullTwoMove);
                                    ModbusTCP_PLC.thisModbus.Write("162", no4.PullTwoForce);
                                    ModbusTCP_PLC.thisModbus.Write("154", no4.PushTwoMove - no4.PullTwoMove);
                                    ModbusTCP_PLC.thisModbus.Write("138", 1);
                                    Log.LogVision("四工位测试拉动采集数据完成");
                                    if (measuredvalue[1, 2] >= model.ArgumentsData[18].Value && measuredvalue[1, 2] <= model.ArgumentsData[16].Value)
                                    {
                                        no4.PullTwoMoveResult = "NG";
                                        bools[2] = true;
                                        //2拉位移ok
                                    }
                                    else
                                    {
                                        no4.PullTwoMoveResult = "NG";
                                        bools[2] = false;
                                    }
                                    if (measuredvalue[1, 3] >= model.ArgumentsData[26].Value && measuredvalue[1, 3] <= model.ArgumentsData[25].Value)
                                    {
                                        no4.PullTwoForceResult = "Pass";
                                        bools[3] = true;
                                        //2拉力ok
                                    }
                                    else
                                    {
                                        no4.PullTwoForceResult = "NG";
                                        bools[3] = false;
                                    }
                                    if (bools[2] && bools[3])
                                    {
                                        ModbusTCP_PLC.thisModbus.Write("132", 1);
                                    }
                                    else
                                    {
                                        ModbusTCP_PLC.thisModbus.Write("132", 2);
                                    }
                                    for (int i = 0; i < bools.Length / 2; i++)
                                    {
                                        if (!bools[i])
                                        {
                                            no4.TwoResult = "NG";
                                            break;
                                        }
                                        else
                                        {
                                            no4.TwoResult = "Pass";

                                        }
                                    }
                                    no4.OneResult = "NULL";
                                    if (ModbusTCP_PLC.thisModbus.PLC_Control[11] == 2)
                                        ExcelTool.SaveDataN4(no4);
                                    else if ((ModbusTCP_PLC.thisModbus.PLC_Control[11] == 1))
                                    {
                                        lock (On4_Lock)
                                        {
                                            no4s.Enqueue(no4);
                                        }
                                        // model.MessageLog("四工位数据缓存");
                                    }
                                    Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                                    {
                                        if (model.No4TestResult.Count > 100)
                                        {
                                            model.No4TestResult.RemoveAt(model.No4TestResult.Count - 1);
                                            model.No4TestResult.Insert(0, no4);
                                        }
                                        else
                                        {
                                            model.No4TestResult.Insert(0, no4);
                                        }
                                    });
                                    stopwatch1.Stop();
                                    model.Auto4CT = stopwatch1.ElapsedMilliseconds.ToString();
                                }
                                else
                                {
                                    ModbusTCP_PLC.thisModbus.Write("132", 2);
                                    //throw new Exception("拉动采集时间过小导致采集数据不足，无法进行运算");
                                }
                            }
                            else
                            {
                                //throw new Exception("基准点采集时间过小导致采集数据不足，无法进行运算");
                            }
                        }
                        continue;
                    Rest:
                        Log.LogVision("四工位程序复位");
                        ModbusTCP_PLC.thisModbus.Write("204", 0);
                        ModbusTCP_PLC.thisModbus.Write("206", 0);
                        ModbusTCP_PLC.thisModbus.Write("208", 0);
                        ModbusTCP_PLC.thisModbus.Write("210", 0);
                        ModbusTCP_PLC.thisModbus.Write("212", 0);
                        ModbusTCP_PLC.thisModbus.Write("214", 0);
                        ModbusTCP_PLC.thisModbus.Write("216", 0);
                        ModbusTCP_PLC.thisModbus.Write("218", 0);
                        ModbusTCP_PLC.thisModbus.Write("220", 0);
                        do
                        {
                            Thread.Sleep(50);
                            ModbusTCP_PLC.thisModbus.Write("220", 0);
                        } while (ModbusTCP_PLC.thisModbus.PLC_Control[10] == 1);
                        Log.LogVision("四工位程序复位完成");
                    }
                }
                catch (Exception ex)
                {
                    Log.LogVision("四工位异常" + "--" + ex.Message);
                    ModbusTCP_PLC.thisModbus.Write("204", 0);
                    ModbusTCP_PLC.thisModbus.Write("206", 0);
                    ModbusTCP_PLC.thisModbus.Write("210", 0);
                    ModbusTCP_PLC.thisModbus.Write("212", 0);
                    ModbusTCP_PLC.thisModbus.Write("214", 0);
                    ModbusTCP_PLC.thisModbus.Write("216", 0);
                    Log.Error(ex.StackTrace, ex);
                }
            }
        }

        float OneEValue = 0;
        float TwoEValue = 0;

        /// <summary>
        /// 五工位自动流程
        /// </summary>
        public void NO5_Auto_1()
        {
            ModbusTCP_PLC.thisModbus.Write("208", 0);
            ModbusTCP_PLC.thisModbus.Write("218", 0);
            Stopwatch stopwatch;
            while (true)
            {
                Thread.Sleep(100);
                if (ModbusTCP_PLC.thisModbus.IsConnet)
                {
                    try
                    {
                        do
                        {
                            ModbusTCP_PLC.thisModbus.Write("208", 0);
                            Thread.Sleep(100);
                        } while (ModbusTCP_PLC.thisModbus.PLC_Control[4] == 1);
                        do
                        {
                            Thread.Sleep(100);
                        } while (ModbusTCP_PLC.thisModbus.PLC_Control[4] != 1);
                        do
                        {
                            Thread.Sleep(100);
                        } while (ModbusTCP_PLC.thisModbus.PLC_Control[9] == 0);
                        Log.LogVision("五工位测试触发");
                        bool one = false;
                        bool two = false;
                        No5 no5 = new No5();
                        No4 no4 = new No4();
                        if (ModbusTCP_PLC.thisModbus.PLC_Control[9] == 1)
                        {
                            Log.LogVision("五工位测试模式两边OK");

                            one = true;
                            two = true;
                        }
                        else if (ModbusTCP_PLC.thisModbus.PLC_Control[9] == 2)
                        {
                            Log.LogVision("五工位测试模式一号OK");

                            one = true;
                            two = false;
                        }
                        else if (ModbusTCP_PLC.thisModbus.PLC_Control[9] == 3)
                        {
                            Log.LogVision("五工位测试模式二号OK");

                            one = false;
                            two = true;
                        }
                        stopwatch = new Stopwatch();
                        stopwatch.Start();
                        ControlPower.Set_Out(Convert.ToDouble(model.ArgumentsData[37].Value), 4);
                        ModbusTCP_PLC.thisModbus.Write("176", model.ArgumentsData[37].Value);
                        ModbusTCP_PLC.thisModbus.Write("178", model.ArgumentsData[37].Value);
                        ControlPower.Output(true);
                        double[] power = new double[3];
                        Stopwatch stopwatch1 = new Stopwatch();
                        stopwatch1.Start();
                        Log.LogVision("五工位测试电机通电");
                        do
                        {
                            //power = ControlPower.Get_Out();
                            Thread.Sleep(50);
                            if (stopwatch1.ElapsedMilliseconds > 3000)
                                throw new Exception($"五工位等待电机启动电流超时！");
                        } while (model.OneElectric < model.ArgumentsData[38].Value && model.TwoElectric < model.ArgumentsData[38].Value);
                        stopwatch1.Stop();
                        ModbusTCP_PLC.thisModbus.Write("208", 0);
                        ModbusTCP_PLC.thisModbus.Write("218", 0);
                        stopwatch1.Restart();
                        do
                        {
                            Thread.Sleep(100);
                        } while (stopwatch1.ElapsedMilliseconds <= model.ArgumentsData[36].Value);
                        if (one && two)//全部采集
                        {
                            var test_rest = MainWindow.Await_Get_Sampling((int)model.ArgumentsData[35].Value, 0);
                            OneEValue = OneAmmeter.thiselectricity.OneA;
                            TwoEValue = TwoAmmeter.thiselectricity.TwoA;
                            model.OneDifferenceValue = test_rest.OneReturn;
                            model.TwoDifferenceValue = test_rest.TwoReturn;
                            model.ThreeDifferenceValue = test_rest.ThreeReturn;
                            model.FourDifferenceValue = test_rest.FourReturn;
                            ModbusTCP_PLC.thisModbus.Write("164", model.OneDifferenceValue);
                            ModbusTCP_PLC.thisModbus.Write("166", model.ThreeDifferenceValue);
                            ModbusTCP_PLC.thisModbus.Write("168", model.TwoDifferenceValue);
                            ModbusTCP_PLC.thisModbus.Write("170", model.FourDifferenceValue);
                            ModbusTCP_PLC.thisModbus.Write("172", OneEValue);
                            ModbusTCP_PLC.thisModbus.Write("174", TwoEValue);
                        }
                        else if (one && !two)//采集1号
                        {
                            var test_rest = MainWindow.Await_Get_Sampling((int)model.ArgumentsData[35].Value, 1);
                            OneEValue = OneAmmeter.thiselectricity.OneA;
                            model.OneDifferenceValue = test_rest.OneReturn;
                            model.TwoDifferenceValue = test_rest.TwoReturn;
                            ModbusTCP_PLC.thisModbus.Write("164", model.OneDifferenceValue);
                            ModbusTCP_PLC.thisModbus.Write("168", model.TwoDifferenceValue);
                            ModbusTCP_PLC.thisModbus.Write("172", OneEValue);
                        }
                        else if (!one && two)//采集2号
                        {
                            var test_rest = MainWindow.Await_Get_Sampling((int)model.ArgumentsData[35].Value, 2);
                            TwoEValue = TwoAmmeter.thiselectricity.TwoA;
                            model.ThreeDifferenceValue = test_rest.ThreeReturn;
                            model.FourDifferenceValue = test_rest.FourReturn;
                            ModbusTCP_PLC.thisModbus.Write("166", model.ThreeDifferenceValue);
                            ModbusTCP_PLC.thisModbus.Write("170", model.FourDifferenceValue);
                            ModbusTCP_PLC.thisModbus.Write("174", TwoEValue);
                        }
                        ControlPower.Output(false);
                        bool[] retbool = new bool[6];

                        if (one && two)//全部采集
                        {
                            no5 = new No5() { Time = DateTime.Now.ToString(), OneValue = model.OneDifferenceValue, TwoValue = model.TwoDifferenceValue, ThreeValue = model.ThreeDifferenceValue, FourValue = model.FourDifferenceValue, OneElE = OneEValue, TwoElE = TwoEValue };
                            if (model.OneDifferenceValue <= model.ArgumentsData[41].Value)
                            {
                                no5.OneValueResult = "Pass";
                                retbool[0] = true;
                                ModbusTCP_PLC.thisModbus.Write("108", 1);
                            }
                            else
                            {
                                no5.OneValueResult = "NG";
                                retbool[0] = false;
                                ModbusTCP_PLC.thisModbus.Write("108", 2);
                            }
                            if (model.TwoDifferenceValue <= model.ArgumentsData[42].Value)
                            {
                                no5.TwoValueResult = "Pass";
                                retbool[1] = true;
                                ModbusTCP_PLC.thisModbus.Write("110", 1);
                            }
                            else
                            {
                                no5.TwoValueResult = "NG";
                                retbool[1] = false;
                                ModbusTCP_PLC.thisModbus.Write("110", 2);
                            }
                            if (model.ThreeDifferenceValue <= model.ArgumentsData[43].Value)
                            {
                                no5.ThreeValueResult = "Pass";
                                retbool[2] = true;
                                ModbusTCP_PLC.thisModbus.Write("112", 1);
                            }
                            else
                            {
                                no5.ThreeValueResult = "NG";
                                retbool[2] = false;
                                ModbusTCP_PLC.thisModbus.Write("112", 2);
                            }
                            if (model.FourDifferenceValue <= model.ArgumentsData[44].Value)
                            {
                                no5.FourValueResult = "Pass";
                                retbool[3] = true;
                                ModbusTCP_PLC.thisModbus.Write("114", 1);
                            }
                            else
                            {
                                no5.FourValueResult = "NG";
                                retbool[3] = false;
                                ModbusTCP_PLC.thisModbus.Write("114", 2);

                            }
                            if (OneEValue >= model.ArgumentsData[40].Value && OneEValue <= model.ArgumentsData[39].Value)
                            {
                                no5.OneElEResult = "Pass";
                                retbool[4] = true;
                                ModbusTCP_PLC.thisModbus.Write("198", (short)1);
                            }
                            else
                            {
                                no5.OneElEResult = "NG";
                                retbool[4] = false;
                                ModbusTCP_PLC.thisModbus.Write("198", (short)2);
                            }
                            if (TwoEValue >= model.ArgumentsData[40].Value && TwoEValue <= model.ArgumentsData[39].Value)
                            {
                                no5.TwoElEResult = "Pass";
                                retbool[5] = true;
                                ModbusTCP_PLC.thisModbus.Write("199", (short)1);
                            }
                            else
                            {
                                no5.TwoElEResult = "NG";
                                retbool[5] = false;
                                ModbusTCP_PLC.thisModbus.Write("199", (short)2);
                            }
                            if (retbool[0] && retbool[1] && retbool[4])
                                no5.OneResult = "Pass";
                            else
                                no5.OneResult = "NG";
                            if (retbool[2] && retbool[3] && retbool[5])
                                no5.TwoResult = "Pass";
                            else
                                no5.TwoResult = "NG";
                        }
                        else if (one && !two)//采集1号
                        {
                            no5 = new No5() { Time = DateTime.Now.ToString(), OneValue = model.OneDifferenceValue, TwoValue = model.TwoDifferenceValue };
                            if (model.OneDifferenceValue <= model.ArgumentsData[41].Value)
                            {
                                no5.OneValueResult = "Pass";
                                retbool[0] = true;
                                ModbusTCP_PLC.thisModbus.Write("108", 1);
                            }
                            else
                            {
                                no5.OneValueResult = "NG";
                                retbool[0] = false;
                                ModbusTCP_PLC.thisModbus.Write("108", 2);
                            }
                            if (model.TwoDifferenceValue <= model.ArgumentsData[42].Value)
                            {
                                no5.TwoValueResult = "Pass";
                                retbool[1] = true;
                                ModbusTCP_PLC.thisModbus.Write("110", 1);
                            }
                            else
                            {
                                no5.TwoValueResult = "NG";
                                retbool[1] = false;
                                ModbusTCP_PLC.thisModbus.Write("110", 2);
                            }
                            if (OneEValue >= model.ArgumentsData[40].Value && OneEValue <= model.ArgumentsData[39].Value)
                            {
                                no5.OneElEResult = "Pass";
                                retbool[2] = true;
                                ModbusTCP_PLC.thisModbus.Write("198", (short)1);
                            }
                            else
                            {
                                no5.OneElEResult = "NG";
                                retbool[2] = false;
                                ModbusTCP_PLC.thisModbus.Write("198", (short)2);
                            }

                            no5.TwoResult = "NULL";
                            if (retbool[0] && retbool[1] && retbool[2])
                                no5.OneResult = "Pass";
                            else
                                no5.OneResult = "NG";
                        }
                        else if (!one && two)//采集2号
                        {
                            no5 = new No5() { Time = DateTime.Now.ToString(), ThreeValue = model.ThreeDifferenceValue, FourValue = model.FourDifferenceValue };
                            if (model.ThreeDifferenceValue <= model.ArgumentsData[43].Value)
                            {
                                no5.ThreeValueResult = "Pass";
                                retbool[0] = true;
                                ModbusTCP_PLC.thisModbus.Write("112", 1);
                            }
                            else
                            {
                                no5.ThreeValueResult = "NG";
                                retbool[0] = false;
                                ModbusTCP_PLC.thisModbus.Write("112", 2);
                            }
                            if (model.FourDifferenceValue <= model.ArgumentsData[44].Value)
                            {
                                no5.FourValueResult = "Pass";
                                retbool[1] = true;
                                ModbusTCP_PLC.thisModbus.Write("114", 1);
                            }
                            else
                            {
                                no5.FourValueResult = "NG";
                                retbool[1] = false;
                                ModbusTCP_PLC.thisModbus.Write("114", 2);

                            }
                            if (TwoEValue >= model.ArgumentsData[40].Value && TwoEValue <= model.ArgumentsData[39].Value)
                            {
                                no5.TwoElEResult = "Pass";
                                retbool[2] = true;
                                ModbusTCP_PLC.thisModbus.Write("199", (short)1);
                            }
                            else
                            {
                                no5.TwoElEResult = "NG";
                                retbool[2] = false;
                                ModbusTCP_PLC.thisModbus.Write("199", (short)2);
                            }
                            no5.OneResult = "NULL";
                            if (retbool[0] && retbool[1] && retbool[2])
                                no5.TwoResult = "Pass";
                            else
                                no5.TwoResult = "NG";
                        }
                        Log.LogVision("五工位测试采集完成");
                        if (ModbusTCP_PLC.thisModbus.PLC_Control[11] == 2)
                            ExcelTool.SaveDataN5(no5);
                        Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                        {
                            if (model.No5TestResult.Count > 100)
                            {
                                model.No5TestResult.RemoveAt(model.No5TestResult.Count - 1);
                                model.No5TestResult.Insert(0, no5);
                            }
                            else
                            {
                                model.No5TestResult.Insert(0, no5);

                            }
                        });
                        stopwatch.Stop();
                        model.Auto5CT = stopwatch.ElapsedMilliseconds.ToString();
                        continue;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "序列不包含任何元素")
                        {

                        }
                        Log.LogVision("五工位异常" + "--" + ex.Message);

                        if (ex.Message == "五工位等待电机启动电流超时！")
                        {
                            ModbusTCP_PLC.thisModbus.Write("108", 2);
                            ModbusTCP_PLC.thisModbus.Write("110", 2);
                            ModbusTCP_PLC.thisModbus.Write("112", 2);
                            ModbusTCP_PLC.thisModbus.Write("114", 2);
                        }
                        if (ex.Message == "等待UDP(模拟量数据)返回数据超时！")
                        {
                            ModbusTCP_PLC.thisModbus.Write("108", 2);
                            ModbusTCP_PLC.thisModbus.Write("110", 2);
                            ModbusTCP_PLC.thisModbus.Write("112", 2);
                            ModbusTCP_PLC.thisModbus.Write("114", 2);
                        }
                        ControlPower.Set_Out(Convert.ToDouble(model.Set_V), Convert.ToDouble(model.Set_A));
                        ControlPower.Output(false);
                        MainWindowViewModel.thisModel.MessageLog(ex.Message, MainWindowViewModel.thisModel.Abnormal, 5);
                        Log.Error(ex.Source, ex);
                        No5 no5 = new No5() { Time = DateTime.Now.ToString(), OneResult = "NG", TwoResult = "NG" };
                        Application.Current.Dispatcher.BeginInvoke((Action)delegate ()
                        {
                            if (model.No5TestResult.Count > 100)
                            {
                                model.No5TestResult.RemoveAt(model.No5TestResult.Count - 1);
                                model.No5TestResult.Insert(0, no5);
                            }
                            else
                            {
                                model.No5TestResult.Insert(0, no5);
                            }
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 6工位1号流程
        /// </summary>
        public void N06_1()
        {
            ModbusTCP_PLC.thisModbus.Write("224", 0);
            ModbusTCP_PLC.thisModbus.Write("226", 0);
            float[] floats = new float[2];
            try
            {
                while (true)
                {
                    do
                    {
                        ModbusTCP_PLC.thisModbus.Write("224", 0);
                        Thread.Sleep(50);
                    } while (ModbusTCP_PLC.thisModbus.PLC_Control[12] == 1);
                    do
                    {
                        Thread.Sleep(50);
                    } while (ModbusTCP_PLC.thisModbus.PLC_Control[12] != 1);//注油一触发
                    Log.LogVision("六工位一号注油触发");
                    try
                    {
                        floats[0] = Dispense232.Get_Weight();
                        floats[1] = Dispense232.Get_FlowVelocity();
                        ModbusTCP_PLC.thisModbus.Write("184", floats[0]);
                        ModbusTCP_PLC.thisModbus.Write("188", floats[1]);
                    }
                    catch (Exception ex)
                    {
                        floats[0] = -1;
                        floats[1] = -1;
                        model.MessageLog("注油机通讯异常", model.Abnormal);
                        Log.LogVision("注油机通讯异常");
                        Log.Error(ex.Message, ex);
                    }
                    ModbusTCP_PLC.thisModbus.Write("224", 0);
                    ModbusTCP_PLC.thisModbus.Write("226", 0);
                    if (floats[0] <= model.ArgumentsData[49].Value && floats[0] >= model.ArgumentsData[50].Value)
                    {
                        if (floats[1] <= model.ArgumentsData[51].Value && floats[0] >= model.ArgumentsData[52].Value)
                            ModbusTCP_PLC.thisModbus.Write("140", 1);
                        else
                            ModbusTCP_PLC.thisModbus.Write("140", 2);
                    }
                    else
                        ModbusTCP_PLC.thisModbus.Write("140", 2);
                    Log.LogVision("六工位一号注油完成");
                }

            }
            catch (Exception ex)
            {

                Log.Error(ex.Message, ex);
            }
        }

        public void N06_2()
        {
            ModbusTCP_PLC.thisModbus.Write("228", 0);
            ModbusTCP_PLC.thisModbus.Write("230", 0);
            float[] floats = new float[2];
            try
            {
                while (true)
                {
                    do
                    {
                        ModbusTCP_PLC.thisModbus.Write("228", 0);
                        Thread.Sleep(50);
                    } while (ModbusTCP_PLC.thisModbus.PLC_Control[14] == 1);
                    do
                    {
                        //if (stopwatch.ElapsedMilliseconds >= 5000)
                        //    break;
                        Thread.Sleep(50);
                    } while (ModbusTCP_PLC.thisModbus.PLC_Control[14] != 1);
                    Log.LogVision("六工位二号注油触发");
                    try
                    {
                        floats[0] = Dispense232.Get_Weight();
                        floats[1] = Dispense232.Get_FlowVelocity();
                        ModbusTCP_PLC.thisModbus.Write("186", floats[0]);
                        ModbusTCP_PLC.thisModbus.Write("190", floats[1]);
                    }
                    catch (Exception ex)
                    {
                        floats[0] = -1;
                        floats[1] = -1;
                        model.MessageLog("注油机通讯异常", model.Abnormal);
                        Log.LogVision("注油机通讯异常");
                        Log.Error(ex.Message, ex);
                    }
                    ModbusTCP_PLC.thisModbus.Write("228", 0);
                    ModbusTCP_PLC.thisModbus.Write("230", 0);
                    if (floats[0] <= model.ArgumentsData[49].Value && floats[0] >= model.ArgumentsData[50].Value)
                    {
                        if (floats[1] <= model.ArgumentsData[51].Value && floats[1] >= model.ArgumentsData[52].Value)
                            ModbusTCP_PLC.thisModbus.Write("142", 1);
                        else
                            ModbusTCP_PLC.thisModbus.Write("142", 2);
                    }
                    else
                        ModbusTCP_PLC.thisModbus.Write("142", 2);
                    Log.LogVision("六工位二号注油完成");
                }

            }
            catch (Exception ex)
            {

                Log.Error(ex.Message, ex);
            }
        }

    }
}

