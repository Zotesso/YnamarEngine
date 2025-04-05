using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Network
{
    internal class ClientTCP
    {
        public int Index;
        public string IP;
        public TcpClient Socket;
        public NetworkStream myStream;
        private ServerHandleDataTCP handleServerData;
        public bool Closing;
        public byte[] readBuff;

        public void Start()
        {
            handleServerData = new ServerHandleDataTCP();

            Socket.SendBufferSize = 4096;
            Socket.ReceiveBufferSize = 4096;
            myStream = Socket.GetStream();

            Array.Resize(ref readBuff, Socket.ReceiveBufferSize);
            myStream.BeginRead(readBuff, 0, Socket.ReceiveBufferSize, OnReceiveData, null);
        }

        private void OnReceiveData(IAsyncResult ar)
        {
            try
            {
                int readBytes = myStream.EndRead(ar);
                if (readBytes <= 0)
                {
                    CloseSocket(Index);
                    return;
                }

                byte[] newBytes = null;
                Array.Resize(ref newBytes, readBytes);
                Buffer.BlockCopy(readBuff, 0, newBytes, 0, readBytes);

                handleServerData.HandleNetworkMessages(Index, newBytes);
                myStream.BeginRead(readBuff, 0, Socket.ReceiveBufferSize, OnReceiveData, null);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                CloseSocket(Index);
            }
        }

        private void CloseSocket(int index)
        {
            Console.WriteLine("Connection from " + IP + "has been ended.");
            Socket.Close();
            Socket = null;
        }
    }
}
