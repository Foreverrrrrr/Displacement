using Displacement.ViewModels;
using HslCommunication.ModBus;
using System;
using System.IO.Ports;
using System.Threading;

namespace Displacement.FunctionCall
{
    public class Dispense232
    {
        private static SerialPort serialPortseria;
        static ModbusRtu busRtuClient;
        private static bool _isopen;
        public static bool IsOpen
        {
            get { return _isopen; }
            set
            {
                _isopen = value;
            }
        }
        public static float Value = 0;
        public static Dispense232 ThisPort;
        public Dispense232(string conname, int baudrate)
        {
            ThisPort = this;
            try
            {
                busRtuClient = new ModbusRtu();
                busRtuClient.SerialPortInni(sp =>
                {
                    sp.PortName = conname;
                    sp.BaudRate = baudrate;
                    sp.DataBits = 8;
                    sp.StopBits = System.IO.Ports.StopBits.One;
                    sp.Parity = System.IO.Ports.Parity.Even;
                });
                busRtuClient.DataFormat = HslCommunication.Core.DataFormat.CDAB;
                var t = busRtuClient.Open(); // 打开
                if (t.IsSuccess)
                {
                    IsOpen = true;
                    Log.LogVision($"打开{busRtuClient.PortName}串口成功！", Log.State.Normal);
                    Log.Info($"打开{busRtuClient.PortName}串口成功！");
                    Thread thread = new Thread(GetDispensing);
                    thread.IsBackground = true;
                    thread.Start();
                }
                else
                {
                    IsOpen = false;
                    Log.LogVision($"打开{busRtuClient.PortName}串口失败！", Log.State.Error);
                    Log.Error($"打开{busRtuClient.PortName}串口失败！");
                }
            }
            catch (Exception ex)
            {
                IsOpen = false;
                Log.LogVision($"打开{busRtuClient.PortName}串口失败！", Log.State.Error);
                Log.Error($"打开{busRtuClient.PortName}串口失败！");
            }
        }
        public static bool[] bools = new bool[3];
        private void GetDispensing()
        {
            while (true)
            {
                if (IsOpen)
                {
                    Thread.Sleep(500);
                    HslCommunication.OperateResult<bool[]> read = busRtuClient.ReadBool("20494", 2);
                    if (read.IsSuccess)
                    {
                        ModbusTCP_PLC.thisModbus.Write("180", read.Content[0]);
                        ModbusTCP_PLC.thisModbus.Write("182", read.Content[1]);
                        var y12 = busRtuClient.ReadUInt16("24588").Content == 0 ? false : true;
                        ModbusTCP_PLC.thisModbus.Write("181", y12);
                    }
                }
            }
        }

        public static float Get_Weight()
        {
            //float[] floats= new float[2];   
            //HslCommunication.OperateResult<byte[]> read = busRtuClient.Read("41090", 8);
            //{
            //    if (read.IsSuccess)
            //    {
            //        floats[0] = busRtuClient.ByteTransform.TransSingle(read.Content, 0);
            //        floats[1] = busRtuClient.ByteTransform.TransSingle(read.Content, 4);

            //    }
            //}
            MainWindowViewModel.thisModel.OilValue = busRtuClient.ReadFloat("41090").Content;
            return MainWindowViewModel.thisModel.OilValue;

        }
        public static float Get_FlowVelocity()
        {
            MainWindowViewModel.thisModel.OilSpeed = busRtuClient.ReadFloat("41092").Content;
            return MainWindowViewModel.thisModel.OilSpeed;
        }
    }
}
