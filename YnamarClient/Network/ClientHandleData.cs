using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static YnamarClient.Network.NetworkPackets;

namespace YnamarClient.Network
{
    internal class ClientHandleData
    {
        public PacketBuffer Buffer = new PacketBuffer();
        private delegate void Packet(int index, byte[] data);
        private static Dictionary<int, Packet> Packets;

        public void InitializeMessages()
        {
            Packets = new Dictionary<int, Packet>();

            Packets.Add((int)ServerPackets.SJoinGame, HandleJoinGame);
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

        private void HandleJoinGame(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            int playerIndex = buffer.GetInteger();
            string targetName = buffer.GetString();

            Types.Player[0].Name = targetName;

            //Globals.playerIndex = buffer.GetInteger();
            //Types.Player[Globals.playerIndex].Name = buffer.GetString();
            //int playerMap = buffer.GetInteger();
        }
    }
}
