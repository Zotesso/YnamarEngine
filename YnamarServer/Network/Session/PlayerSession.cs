using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Network.Session
{
    public class PlayerSession
    {
        public int Index { get; set; }
        public int PlayerId { get; set; }
        public TcpClient? Tcp { get; init; }
        public IPEndPoint? UdpEndpoint { get; set; }

        public long UdpToken { get; init; }

        public DateTime LastHeartbeat { get; set; } = DateTime.UtcNow;

        public bool IsConnected => Tcp?.Connected ?? false;

    }
}
