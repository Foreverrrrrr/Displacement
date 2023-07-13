using Displacement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace Displacement.FunctionCall
{
    public class ControlPower
    {
        private static SerialPort serialPortseria;

        private static bool _isopen;
        public static bool IsOpen
        {
            get { return _isopen; }
            set
            {
                _isopen = value;
            }
        }


        public static ControlPower ThisPower;

        public ControlPower(string com, int baudrate)
        {
            ThisPower = this;
            serialPortseria = new SerialPort();
            serialPortseria.PortName = com;
            serialPortseria.BaudRate = baudrate;
            serialPortseria.Parity = Parity.None;
            serialPortseria.StopBits = StopBits.One;
            //serialPortseria.ReceivedBytesThreshold = 9;
            serialPortseria.DataReceived += SerialPort_DataReceived;
            OpenPorts();
        }

        private static string Read_String = "";

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            List<byte> buffer = new List<byte>();
            byte[] data = new byte[1024];
            while (true)
            {
                System.Threading.Thread.Sleep(20);
                if (serialPortseria.BytesToRead < 1)
                {
                    break;
                }
                int recCount = serialPortseria.Read(data, 0, Math.Min(serialPortseria.BytesToRead, data.Length));

                byte[] buffer2 = new byte[recCount];
                Array.Copy(data, 0, buffer2, 0, recCount);
                buffer.AddRange(buffer2);
            }
            if (buffer.Count == 0 && buffer.Count != 19)
                return;
            var t = buffer.ToArray();
            Read_String = System.Text.Encoding.Default.GetString(t).Remove(buffer.Count - 1);
        }

        private void OpenPorts()
        {
            try
            {
                if (!serialPortseria.IsOpen)
                {
                    serialPortseria.Open();
                    serialPortseria.ReadTimeout = 5000;
                    if (serialPortseria.IsOpen)
                    {
                        IsOpen = true;
                        Log.LogVision($"打开{serialPortseria.PortName}串口成功！", Log.State.Normal);
                        Log.Info($"打开{serialPortseria.PortName}串口成功！");
                    }
                    else
                    {
                        IsOpen = false;
                        Log.LogVision($"打开{serialPortseria.PortName}串口失败！", Log.State.Error);
                        Log.Error($"打开{serialPortseria.PortName}串口失败！");
                    }
                }
            }
            catch (Exception ex)
            {
                IsOpen = false;
                MainWindowViewModel.thisModel.MessageLog($"打开压力传感器通信{serialPortseria.PortName}串口失败！", MainWindowViewModel.thisModel.Abnormal, 5);
                Log.Error($"打开{serialPortseria.PortName}串口失败！", ex);
            }
        }

        /// <summary>
        ///设置电压电流
        /// </summary>
        /// <param name="v">电压值</param>
        /// <param name="a">电流值</param>
        public static void Set_Out(double v, double a)
        {
            try
            {
                if (IsOpen)
                {
                    string message = $":APPLy CH1,{v},{a}";
                    serialPortseria.Write(message + "\r\n");
                    //Thread.Sleep(20);
                    //serialPortseria.Write(":OUTPut:OCP CH1,ON" + "\r\n");//打开过流保护
                    //Thread.Sleep(20);
                    //serialPortseria.Write(":OUTPut:OVP CH1,ON" + "\r\n");//打开过压保护

                }
            }
            catch (Exception ex)
            {
                IsOpen = false;
                Log.Error($"{serialPortseria.PortName}串口发送失败！", ex);
            }
        }

        /// <summary>
        /// 控制输出
        /// </summary>
        /// <param name="on_off"></param>
        public static void Output(bool on_off)
        {
            try
            {
                if (IsOpen)
                {
                    Thread.Sleep(100);
                    if (on_off)
                        serialPortseria.Write(":OUTPut:STATe CH1,ON" + "\r\n");
                    else
                        serialPortseria.Write(":OUTPut:STATe CH1,OFF" + "\r\n");
                }
            }
            catch (Exception ex)
            {
                IsOpen = false;
                Log.Error($"{serialPortseria.PortName}串口发送失败！", ex);
            }
        }

        /// <summary>
        /// 获取电源输出数据
        /// </summary>
        /// <returns>[0]==电压 [1]==电流  [2]==功率</returns>
        public static double[] Get_Out()
        {
            double[] doubles = new double[3];
            try
            {
                if (IsOpen)
                {
                    Read_String = "";
                    string message = ":MEASure:ALL? CH1";
                    serialPortseria.Write(message + "\r\n");
                    Thread.Sleep(100);
                    var re = Read_String.Split(new char[] { ',' });
                    if (Read_String != "")
                    {
                        for (int i = 0; i < doubles.Length; i++)
                        {
                            doubles[i] = Convert.ToDouble(re[i]);
                        }
                    }
                    return doubles;
                }
                return doubles;
            }
            catch (Exception ex)
            {
                //IsOpen = false;
                Log.Error($"{serialPortseria.PortName}串口发送失败！ -- {Read_String}", ex);
                return doubles;
            }
        }
    }
}
