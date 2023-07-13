using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Displacement.FunctionCall
{
    /// <summary>
    /// 串动
    /// </summary>
    public class AsyncTcpServerGetPressure : ClientSession
    {

        /// <summary>
        /// TCP端口数据接收事件
        /// </summary>
        public event Action<DateTime, IPEndPoint, byte[]> OnTCPReadEvent;

        public event Action<DateTime, Exception> DisconnectionEvent;
        /// <summary>
        /// 客户端连接事件
        /// </summary>
        public event Action<DateTime, IPEndPoint> SuccessfuConnectEvent;
        private object lockObject = new object();
        private Socket socketCore = null;
        private byte[] buffer = new byte[2048];
        private List<ClientSession> sockets = new List<ClientSession>();

        private bool _isconnet;

        public bool IsCommet
        {
            get { return _isconnet; }
            set
            {
                _isconnet = value;
            }
        }
        /// <summary>
        /// 是否接收数据
        /// </summary>
        public bool IF_Read { get; set; } = true;
        public AsyncTcpServerGetPressure(string ip, int port)
        {
            IPAddress pcip = IPAddress.Parse(ip);
            socketCore = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socketCore.Bind(new IPEndPoint(pcip, port));
            //socketCore.Listen(1024);
            socketCore.BeginAccept(new AsyncCallback(AsyncAcceptCallback), socketCore);
        }

        /// <summary>
        /// 异步传入的连接申请请求
        /// </summary>
        /// <param name="iar">异步对象</param>
        protected void AsyncAcceptCallback(IAsyncResult iar)
        {
            if (iar.AsyncState is Socket server_socket)
            {
                Socket client = null;
                ClientSession session = new ClientSession();
                try
                {
                    client = server_socket.EndAccept(iar);
                    session.Socket = client;
                    session.EndPoint = (IPEndPoint)client.RemoteEndPoint;
                    IsCommet = true;
                    client.BeginReceive(buffer, 0, 2048, SocketFlags.None, new AsyncCallback(ReceiveCallBack), session);
                    lock (session)
                    {
                        sockets.Add(session);
                    }
                    SuccessfuConnectEvent?.BeginInvoke(DateTime.Now, new IPEndPoint(session.EndPoint.Address, session.EndPoint.Port), null, null);
                }
                catch (ObjectDisposedException)//Server Close
                {
                    IsCommet = false;
                    lock (lockObject)
                    {
                        sockets.Remove(session);
                    }
                    return;
                }
                catch (Exception ex)
                {
                    DisconnectionEvent?.BeginInvoke(DateTime.Now, ex, null, null);
                    IsCommet = false;
                    lock (lockObject)
                    {
                        sockets.Remove(session);
                    }
                    client?.Close();
                }
                server_socket.BeginAccept(new AsyncCallback(AsyncAcceptCallback), server_socket);
            }
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            if (ar.AsyncState is ClientSession client)
            {
                string msg = "";
                try
                {
                    int length = client.Socket.EndReceive(ar);
                    if (length == 0)
                    {
                        client.Socket.Close();
                        lock (lockObject)
                        {
                            sockets.Remove(client);
                        }
                        return;
                    };
                    client.Socket.BeginReceive(buffer, 0, 2048, SocketFlags.None, new AsyncCallback(ReceiveCallBack), client);
                    if (IF_Read)
                    {
                        byte[] data = new byte[length];
                        if (CRC(buffer))
                        {
                            Array.Copy(buffer, 10, data, 0, 1200);
                            //msg = Encoding.ASCII.GetString(data, 0, length); //接收数据
                            OnTCPReadEvent?.BeginInvoke(DateTime.Now, new IPEndPoint(client.EndPoint.Address, client.EndPoint.Port), data, null, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DisconnectionEvent?.BeginInvoke(DateTime.Now, ex, null, null);
                    lock (lockObject)
                    {
                        if (ex.Message == "远程主机强迫关闭了一个现有的连接。")
                        {
                            IsCommet = false;
                            sockets.Remove(client);
                        }
                    }
                }
            }
        }

        private bool CRC(byte[] data)
        {
            if (data[0] == 0xff && data[data.Length - 1] == 0xfe)
            {
                if (data.Length == 1220 && data[6] == 0x04 && data[7] == 0xc4)
                {
                    return true;
                }
            }
            return false;
        }

        public void AsyncWrite(int port, string meg)
        {
            if (IsCommet)
            {
                var emp = sockets.Find(e => e.EndPoint.Port == port);
                if (emp != null)
                {
                    byte[] msgBytes = Encoding.ASCII.GetBytes(meg);
                    emp.Socket.BeginSend(msgBytes, 0, msgBytes.Length, SocketFlags.None, null, emp.Socket);
                }
            }
        }

        public void AsyncWrite(string meg)
        {
            if (IsCommet)
            {
                var emp = sockets[0];
                if (emp != null)
                {
                    byte[] msgBytes = Encoding.ASCII.GetBytes(meg);
                    emp.Socket.BeginSend(msgBytes, 0, msgBytes.Length, SocketFlags.None, null, emp.Socket);
                }
            }
        }

        public void AsyncWrite(string ip, int port, string meg)
        {
            if (IsCommet)
            {
                var emp = sockets.Find(e => e.EndPoint.Address.ToString() == ip && e.EndPoint.Port == port);
                if (emp != null)
                {
                    byte[] msgBytes = Encoding.ASCII.GetBytes(meg);
                    emp.Socket.BeginSend(msgBytes, 0, msgBytes.Length, SocketFlags.None, null, emp.Socket);
                }
            }
        }

        public void CloseTCPServer()
        {
            foreach (var socket in sockets)
            {
                socket?.Socket?.Close();
            }
            IsCommet = false;
            sockets.Clear();
            socketCore.Dispose();
        }
    }

    
}
