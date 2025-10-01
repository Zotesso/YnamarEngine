using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static YnamarClient.Network.NetworkPackets;

namespace YnamarClient.Network
{
    internal class ClientTCP
    {
        public TcpClient PlayerSocket;
        private static NetworkStream myStream;
        private ClientHandleDataTCP clientDataHandle;
        private byte[] asyncBuff;
        private bool connecting;
        private bool connected;

        public static Types.PlayerStruct[] Player = new Types.PlayerStruct[Constants.MAX_PLAYERS];

        public void ConnectToServer()
        {
            if (PlayerSocket != null)
            {
                if (PlayerSocket.Connected || connected)
                {
                    return;
                }
                PlayerSocket.Close();
                PlayerSocket = null;
            }

            PlayerSocket = new TcpClient();
            clientDataHandle = new ClientHandleDataTCP();
            PlayerSocket.ReceiveBufferSize = 100000;
            PlayerSocket.SendBufferSize = 100000;
            PlayerSocket.NoDelay = false;

            Array.Resize(ref asyncBuff, 100000);

            PlayerSocket.BeginConnect("127.0.0.1", 5555, new AsyncCallback(ConnectCallback), PlayerSocket);
            connecting = true;
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            PlayerSocket.EndConnect(ar);
            if (PlayerSocket.Connected == false)
            {
                connecting = false;
                connected = false;
                return;
            }
            else
            {
                PlayerSocket.NoDelay = true;
                myStream = PlayerSocket.GetStream();
                myStream.BeginRead(asyncBuff, 0, 100000, OnReceive, null);
                connected = true;
                connecting = false;
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            int byteAmt = myStream.EndRead(ar);
            byte[] myBytes = null;
            Array.Resize(ref myBytes, byteAmt);
            Buffer.BlockCopy(asyncBuff, 0, myBytes, 0, byteAmt);

            if (byteAmt == 0)
            {
                return;
            }

            clientDataHandle.HandleNetworkMessages(0, myBytes);
            myStream.BeginRead(asyncBuff, 0, 100000, OnReceive, null);
        }

        public static bool IsPlaying(int index)
        {

            if (Types.Player[index].Name != null)
            {
                return true;
            }

            return false;
        }

        public void SendData(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            myStream.Write(buffer.ToArray(), 0, buffer.ToArray().Length);
            buffer.Dispose();
        }

        public void SendLogin(string username, string password)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInteger((int)ClientTcpPackets.CLogin);
            buffer.AddString(username);
            buffer.AddString(password);

            SendData(buffer.ToArray());
            buffer.Dispose();
        }

        public void SendRegister()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInteger((int)ClientTcpPackets.CRegister);
            buffer.AddString(Globals.regUsername);
            buffer.AddString(Globals.regPassword);
            buffer.AddString(Globals.regRepeatPassword);

            SendData(buffer.ToArray());
            buffer.Dispose();
        }

        public void SendPlayerMove()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInteger((int)ClientTcpPackets.CPlayerMove);

            buffer.AddByte(Types.Player[Globals.playerIndex].Dir);
            buffer.AddInteger(Types.Player[Globals.playerIndex].Moving);

            SendData(buffer.ToArray());
            buffer.Dispose();
        }

        public void SendLoadMap()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInteger((int)ClientTcpPackets.CLoadMap);
            buffer.AddInteger(Types.Player[Globals.playerIndex].Map);

            SendData(buffer.ToArray());
            buffer.Dispose();
        }
    }
}
