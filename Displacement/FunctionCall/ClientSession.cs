using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Displacement.FunctionCall
{
    public class ClientSession
    {
        public Socket Socket { get; set; }

        public IPEndPoint EndPoint { get; set; }

        public override string ToString() => EndPoint.ToString();
    }
}
