using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database;
using YnamarServer.Database.Models;
using YnamarServer.GameLogic;
using static YnamarServer.Network.NetworkPackets;

namespace YnamarServer.Network
{
    internal class ServerHandleData
    {
        private delegate void Packet(int index, byte[] data);
        private static Dictionary<int, Packet> Packets;

        public void InitializeMessages()
        {
            Packets = new Dictionary<int, Packet>();

            Console.WriteLine("Initializing Packets");

            //Packets
            Packets.Add((int)ClientUdpPackets.UdpCAttack, HandlePlayerAttack);
        }

        public void HandleNetworkMessages(int index, byte[] data)
        {
            int packetNum;
            PacketBuffer buffer;
            buffer = new PacketBuffer();

            buffer.AddByteArray(data);
            packetNum = buffer.GetInteger();
            buffer.Dispose();

            if (Packets.TryGetValue(packetNum, out Packet Packet))
            {
                Packet.Invoke(index, data);
            }
        }
        

        public void HandlePlayerAttack(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            byte dir = buffer.GetByte();

            SendPlayerAttackToMap(index, dir);
            GameLogicHandler.PlayerAttack(index, dir);
        }

        private void SendPlayerAttackToMap(int index, byte dir)
        {
            PacketBuffer bufferSend = new PacketBuffer();
            bufferSend.AddInteger((int)ServerUdpPackets.UdpSPlayerAttacking);
            bufferSend.AddInteger(index);
            bufferSend.AddByte(dir);

            NetworkManager.ServerUdp.SendDataToMap(InMemoryDatabase.Player[index].Map, bufferSend.ToArray());

            bufferSend.Dispose();
        }
    }
}
