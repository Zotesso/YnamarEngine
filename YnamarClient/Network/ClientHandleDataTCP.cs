using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static YnamarClient.Network.NetworkPackets;
using YnamarClient.GUI;

namespace YnamarClient.Network
{
    internal class ClientHandleDataTCP
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

            Globals.playerIndex = buffer.GetInteger();
            Types.Player[Globals.playerIndex].Name = buffer.GetString();
            Types.Player[Globals.playerIndex].Sprite = buffer.GetInteger();
            Types.Player[Globals.playerIndex].Level = buffer.GetInteger();
            Types.Player[Globals.playerIndex].EXP = buffer.GetInteger();
            Types.Player[Globals.playerIndex].Map = buffer.GetInteger();
            Types.Player[Globals.playerIndex].X = buffer.GetInteger();
            Types.Player[Globals.playerIndex].Y = buffer.GetInteger();
            Types.Player[Globals.playerIndex].Dir = buffer.GetByte();
            Types.Player[Globals.playerIndex].XOffset = buffer.GetInteger();
            Types.Player[Globals.playerIndex].YOffset = buffer.GetInteger();
            Types.Player[Globals.playerIndex].Access = buffer.GetByte();

            MenuManager.ChangeMenu(MenuManager.Menu.InGame, Game1.desktop);
            GameLogic.InGame();
        }
    }
}
