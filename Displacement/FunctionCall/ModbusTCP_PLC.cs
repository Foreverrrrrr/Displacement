using Displacement.ViewModels;
using HslCommunication.ModBus;
using System;
using System.Threading;
using System.Timers;

namespace Displacement.FunctionCall
{
    public class ModbusTCP_PLC
    {
        ModbusTcpNet modbus;

        private bool _isconnet;
        /// <summary>
        /// 连接标志
        /// </summary>
        public bool IsConnet
        {
            get { return _isconnet; }
            set { _isconnet = value; }
        }

        public int[] PLC_Control { get; set; } = new int[18];

        public static ModbusTCP_PLC thisModbus { get; set; }

        public ModbusTCP_PLC(string ip, int port)
        {
            thisModbus = this;
            Connector(ip, port);
        }

        public async void Connector(string ip, int port)
        {
            modbus = new ModbusTcpNet(ip, port);
            modbus.DataFormat = HslCommunication.Core.DataFormat.CDAB;
            var isconnect = await modbus.ConnectServerAsync();
            if (isconnect.IsSuccess)
            {
                IsConnet = true;
                Thread thread = new Thread(Read);
                thread.IsBackground = true;
                thread.Start();
                System.Timers.Timer aTimer = new System.Timers.Timer(500);
                // 设置事件
                aTimer.Elapsed += MyMethod;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
                MainWindowViewModel.thisModel.MessageLog($"连接（{ip}--{port}）ModbusTCP服务器成功！", MainWindowViewModel.thisModel.Consequence);
                Log.Info($"连接（{ip}--{port}）ModbusTCP服务器成功！");
            }
            else
            {
                IsConnet = false;
                MainWindowViewModel.thisModel.MessageLog($"{ip}--{port}ModbusTCP服务器连接超时！", MainWindowViewModel.thisModel.Abnormal);
                Log.Error($"{ip}--{port}ModbusTCP服务器连接超时！");
            }
        }

        bool dog;
        private void MyMethod(object sender, ElapsedEventArgs e)
        {
            if (dog)
            {
                dog = false;
                Write("100", 1);
            }
            else
            {
                dog = true;
                Write("100", 0);
            }
        }

        private void Read()
        {
            while (true)
            {
                Thread.Sleep(100);
                HslCommunication.OperateResult<byte[]> read = modbus.Read("200", 128);
                if (read.IsSuccess)
                {
                    if (!IsConnet)
                        IsConnet = true;
                    PLC_Control[0] = modbus.ByteTransform.TransInt16(read.Content, 0);//d200生存信号
                    PLC_Control[1] = modbus.ByteTransform.TransInt16(read.Content, 4);//d202当前型号
                    PLC_Control[2] = modbus.ByteTransform.TransInt16(read.Content, 8);//d204串动采集零点校准1
                    PLC_Control[3] = modbus.ByteTransform.TransInt16(read.Content, 12);//d206串动推位移采集开始1
                    PLC_Control[4] = modbus.ByteTransform.TransInt16(read.Content, 16);//d208轴跳采集开始1
                    PLC_Control[5] = modbus.ByteTransform.TransInt16(read.Content, 20);//d210串动拉位移采集开始1
                    PLC_Control[6] = modbus.ByteTransform.TransInt16(read.Content, 24);//d212串动采集零点校准2
                    PLC_Control[7] = modbus.ByteTransform.TransInt16(read.Content, 28);//d214串动推位移采集开始2
                    PLC_Control[8] = modbus.ByteTransform.TransInt16(read.Content, 32);//d216串动拉位移采集开始2
                    PLC_Control[9] = modbus.ByteTransform.TransInt16(read.Content, 36);//d218轴跳采集开始2
                    PLC_Control[10] = modbus.ByteTransform.TransInt16(read.Content, 40);//d220复位
                    PLC_Control[11] = modbus.ByteTransform.TransInt16(read.Content, 44);//d222手自动
                    PLC_Control[12] = modbus.ByteTransform.TransInt16(read.Content, 48);//d224注油1触发
                    PLC_Control[13] = modbus.ByteTransform.TransInt16(read.Content, 52);//d226注油1结果
                    PLC_Control[14] = modbus.ByteTransform.TransInt16(read.Content, 56);//d228注油2触发
                    PLC_Control[15] = modbus.ByteTransform.TransInt16(read.Content, 60);//d230注油2结果
                    PLC_Control[16] = modbus.ByteTransform.TransInt16(read.Content, 64);//d232设备总复位
                    PLC_Control[17] = modbus.ByteTransform.TransInt16(read.Content, 68);//d234设备总复位
                }
                else
                {
                    if (IsConnet)
                        IsConnet = false;
                }
            }
        }

        public void Colse()
        {
            modbus?.Dispose();
        }

        public bool Write(string size, int value)
        {
            var a = modbus.Write(size, value);
            return a.IsSuccess;
        }

        public bool Write(string size, bool value)
        {
            var a = modbus.Write(size, value);
            return a.IsSuccess;
        }

        public bool Write(string size, float value)
        {
            var a = modbus.Write(size, value);
            return a.IsSuccess;
        }
        public bool Write(string size, double value)
        {
            var a = modbus.Write(size, Convert.ToSingle(value));
            return a.IsSuccess;
        }

        public float[] ReadSave()
        {
            float[] floats = new float[50];
            HslCommunication.OperateResult<byte[]> read = modbus.Read("236", 192);
            if (read.IsSuccess)
            {
                if (!IsConnet)
                    IsConnet = true;
                for (int i = 0; i < floats.Length; i++)
                {
                    if (i < 8 || i == 20 || i == 21 || i == 30 || i == 31 || i == 36 || i == 37)
                    {
                        floats[i] = (float)modbus.ByteTransform.TransInt32(read.Content, i * 4);
                    }
                    else
                    {
                        floats[i] = modbus.ByteTransform.TransSingle(read.Content, i * 4);
                    }
                }
                //if (floats[0] == 1 && floats[2] == 1 && floats[4] == 1 && floats[6] == 1 && floats[20] == 1 && floats[30] == 1 && floats[48] == 1)
                //    floats[38] = 1;
                //else
                //    floats[38] = 2;
            }
            return floats;
        }
    }
}
