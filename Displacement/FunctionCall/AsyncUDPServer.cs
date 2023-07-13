using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Displacement.FunctionCall
{
    /// <summary>
    /// UdpClient 实现异步UDP服务器
    /// </summary>
    public class AsyncUDPServer
    {

        /// <summary>
        /// TCP端口数据接收事件
        /// </summary>
        public event Action<DateTime, IPEndPoint, byte[]> OnTCPReadEvent;

        public event Action<DateTime, IPEndPoint, byte[]> BeginInvokeOnTCPReadEvent;

        public event Action<DateTime, Exception> DisconnectionEvent;
        /// <summary>
        /// 客户端连接事件
        /// </summary>
        public event Action<DateTime, IPEndPoint> SuccessfuConnectEvent;
        #region Fields
        /// <summary>
        /// 服务器程序允许的最大客户端连接数
        /// </summary>
        private int _maxClient;

        /// <summary>
        /// 当前的连接的客户端数
        /// </summary>
        //private int _clientCount;

        /// <summary>
        /// 服务器使用的异步UdpClient
        /// </summary>
        private UdpClient _server;

        /// <summary>
        /// 客户端会话列表
        /// </summary>
        //private List<AsyncUDPState> _clients;

        private bool disposed = false;

        /// <summary>
        /// 数据接受缓冲区
        /// </summary>
        private byte[] _recvBuffer;
        #endregion
        #region Properties

        /// <summary>
        /// 服务器是否正在运行
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// 监听的IP地址
        /// </summary>
        public IPAddress Address { get; private set; }

        /// <summary>
        /// 监听的端口
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// 通信使用的编码
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// 是否接收数据
        /// </summary>
        public static bool IF_Read { get; set; } = false;

        /// <summary>
        /// 在线数量
        /// </summary>
        public static int OnLineNumer { get; set; } = 0;
        #endregion

        #region 构造函数

        /// <summary>
        /// 异步UdpClient UDP服务器
        /// </summary>
        /// <param name="listenPort">监听的端口</param>
        public AsyncUDPServer(int listenPort)
            : this(IPAddress.Any, listenPort, 1024)
        {

        }

        /// <summary>
        /// 异步UdpClient UDP服务器
        /// </summary>
        /// <param name="localEP">监听的终结点</param>
        public AsyncUDPServer(IPEndPoint localEP)
            : this(localEP.Address, localEP.Port, 1024)
        {

        }

        /// <summary>
        /// 异步UdpClient UDP服务器
        /// </summary>
        /// <param name="localIPAddress">监听的IP地址</param>
        /// <param name="listenPort">监听的端口</param>
        /// <param name="maxClient">最大客户端数量</param>
        public AsyncUDPServer(IPAddress localIPAddress, int listenPort, int maxClient)
        {
            for (int i = 0; i < 8; i++)
            {
                bytes[i] = new byte[150];
            }
            this.Address = localIPAddress;
            this.Port = listenPort;
            this.Encoding = Encoding.Default;
            _maxClient = maxClient;
            //_clients = new List<AsyncUDPSocketState>(); this.Address
            _server = new UdpClient(new IPEndPoint(IPAddress.Any, this.Port));
            //_server.Connect(new IPEndPoint(this.Address, this.Port));
            _recvBuffer = new byte[_server.Client.ReceiveBufferSize];
            Start();
        }
        #endregion



        #region Method
        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <returns>异步TCP服务器</returns>
        public void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                _server.EnableBroadcast = true;
                _server.BeginReceive(ReceiveDataAsync, null);
            }
        }


        /// <summary>
        /// 停止服务器
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                _server.Close();
                //TODO 关闭对所有客户端的连接
            }
        }

        public byte[][] bytes = new byte[8][]; //二维 byte 数组
        /// <summary>
        /// 接收数据的方法
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveDataAsync(IAsyncResult ar)
        {
            IPEndPoint remote = null;
            byte[] buffer = null;
            try
            {
                buffer = _server.EndReceive(ar, ref remote);
                OnLineNumer = _server.Available;
                if (buffer.Length == 1220)
                {
                    byte[] data = new byte[buffer.Length - 20];
                    if (CRC(buffer))
                    {
                        Array.Copy(buffer, 10, data, 0, 1200);
                        //for (int i = 0; i < 8; i++) //外层循环遍历行
                        //{
                        //    for (int j = 0; j < 150; j++) //内层循环遍历列
                        //    {
                        //        bytes[i][j] = data[i * 150 + j];
                        //    }
                        //}
                        OnTCPReadEvent?.Invoke(DateTime.Now, remote, data);
                       // BeginInvokeOnTCPReadEvent?.BeginInvoke(DateTime.Now, remote, data, null,null);
                    }
                }

            }
            catch (Exception)
            {   
                //TODO 处理异常
                //RaiseOtherException(null);
            }
            finally
            {
                if (IsRunning && _server != null)
                    _server.BeginReceive(ReceiveDataAsync, null);
            }
        }

        private bool CRC(byte[] data)
        {
            if (data[0] == 0xff && data[data.Length - 1] == 0xfe)
            {
                if (data.Length == 1220)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="remote"></param>
        public void Send(byte[] msg)
        {
            byte[] data = msg;
            try
            {
                //RaisePrepareSend(null);
                _server.BeginSend(data, data.Length, "192.168.90.190", 1190, new AsyncCallback(SendCallback), null);


            }
            catch (Exception)
            {
                //TODO 异常处理
                //RaiseOtherException(null);
            }
        }



        private void SendCallback(IAsyncResult ar)
        {
            if (ar.IsCompleted)
            {
                try
                {
                    _server.EndSend(ar);
                    //消息发送完毕事件
                    // RaiseCompletedSend(null);
                }
                catch (Exception)
                {
                    //TODO 数据发送失败事件
                    //RaiseOtherException(null);
                }
            }
        }
        #endregion

        #region 释放

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release 
        /// both managed and unmanaged resources; <c>false</c> 
        /// to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    try
                    {
                        Stop();
                        if (_server != null)
                            _server = null;
                    }
                    catch (SocketException)
                    {
                        //TODO
                        //RaiseOtherException(null);
                    }
                }
                disposed = true;
            }
        }
        #endregion
    }

}
