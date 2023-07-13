using Displacement.ViewModels;
using HslCommunication.ModBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Displacement.FunctionCall
{
    public class Ammeter
    {
        private ModbusRtu busRtuClient;

        private float _a;

        public float A
        {
            get { return (_a * 10); }
            set { _a = value; }
        }

        public string ComName { get; set; }

        public int Port { get; set; }

        private static bool _isopen;

        public static bool IsOpen
        {
            get { return _isopen; }
            set
            {
                _isopen = value;
                //if (_isopen)
                //    MainWindowViewModel.thisModel.AState = MainWindowViewModel.thisModel.Consequence;
                //else
                //    MainWindowViewModel.thisModel.AState = MainWindowViewModel.thisModel.Abnormal;
                //;
            }
        }

        public static Ammeter thiselectricity { get; set; }

        public Ammeter(string com, int baudrate)
        {
            thiselectricity = this;
            ComName = com;
            Port = baudrate;
            OpenPower(ComName, Port);
        }

        public bool OpenPower(string com, int baudrate)
        {
            try
            {
                busRtuClient = new ModbusRtu();
                busRtuClient.SerialPortInni(sp =>
                {
                    sp.PortName = com;
                    sp.BaudRate = baudrate;
                    sp.DataBits = 8;
                    sp.StopBits = System.IO.Ports.StopBits.One;
                    sp.Parity = System.IO.Ports.Parity.None;
                });
                busRtuClient.DataFormat = HslCommunication.Core.DataFormat.ABCD;
                busRtuClient.Open(); // 打开
                IsOpen = true;
                Thread thread = new Thread(ReadPower);
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                IsOpen = false;
                Log.Error(ex.Message, ex);
            }
            return IsOpen;
        }

        private void ReadPower()
        {
            while (true)
            {
                Thread.Sleep(50);
                HslCommunication.OperateResult<byte[]> read = busRtuClient.Read("42", 4);
                if (read.IsSuccess)
                {
                    A = busRtuClient.ByteTransform.TransSingle(read.Content, 0);
                    if (!IsOpen)
                        IsOpen = true;
                }
                else
                {
                    IsOpen = false;
                }
            }
        }
    }
}
