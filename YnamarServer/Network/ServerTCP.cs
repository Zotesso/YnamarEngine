using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static YnamarServer.Network.NetworkPackets;
using YnamarServer.Database;

namespace YnamarServer.Network
{
    internal class ServerTCP
    {
        public static ClientTCP[] Clients = new ClientTCP[Constants.MAX_PLAYERS];
        public TcpListener ServerSocket;
        public static NetworkStream clientStream;
        public void InitializeNetwork()
        {
            ServerSocket = new TcpListener(IPAddress.Any, 5555);
            ServerSocket.Start();
            ServerSocket.BeginAcceptTcpClient(OnClientConnect, null);
        }

        private void OnClientConnect(IAsyncResult ar)
        {
            TcpClient client = ServerSocket.EndAcceptTcpClient(ar);
            client.NoDelay = false;
            ServerSocket.BeginAcceptTcpClient(OnClientConnect, null);

            for (int i = 0; i <= Constants.MAX_PLAYERS; i++)
            {
                if (Clients[i].Socket == null)
                {
                    Clients[i].Socket = client;
                    Clients[i].Index = i;
                    Clients[i].IP = client.Client.RemoteEndPoint.ToString();
                    Clients[i].Start();

                    Console.WriteLine("Connection received from " + Clients[i].IP);
                    return;
                }
            }
        }

        public bool isConnected(int index)
        {
            if (Clients[index].Socket != null)
            {
                return Clients[index].Socket.Connected;
            }
            return false;
        }

        public bool isPlaying(int index)
        {
            return InMemoryDatabase.Player[index] != null;
        }

        public void SendData(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            clientStream = Clients[index].Socket.GetStream();
            clientStream.Write(buffer.ToArray(), 0, buffer.ToArray().Length);
            buffer.Dispose();
        }

        public void SendDataToMap(int mapNum, byte[] data)
        {
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if (isConnected(i) && isPlaying(i))
                {
                    if (InMemoryDatabase.Player[i].Map == mapNum)
                    {
                        SendData(i, data);
                    }
                }
            }
        }

        public void SendDataToMapBut(int index, int mapNum, byte[] data)
        {
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if (isConnected(i))
                {
                    if (InMemoryDatabase.Player[i].Map == mapNum && i != index)
                    {
                        SendData(i, data);
                    }
                }
            }
        }
    }
}
