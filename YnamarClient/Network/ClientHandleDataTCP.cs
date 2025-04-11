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
            Packets.Add((int)ServerPackets.SPlayerData, HandlePlayerData);
            Packets.Add((int)ServerPackets.SPlayerMove, HandlePlayerMove);
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

        private void HandlePlayerData(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            int targetIndex = buffer.GetInteger();
            Types.Player[targetIndex].Name = buffer.GetString();
            Types.Player[targetIndex].Sprite = buffer.GetInteger();
            Types.Player[targetIndex].Level = buffer.GetInteger();
            Types.Player[targetIndex].EXP = buffer.GetInteger();
            Types.Player[targetIndex].Map = buffer.GetInteger();
            Types.Player[targetIndex].X = buffer.GetInteger();
            Types.Player[targetIndex].Y = buffer.GetInteger();
            Types.Player[targetIndex].Dir = buffer.GetByte();
            Types.Player[targetIndex].XOffset = buffer.GetInteger();
            Types.Player[targetIndex].YOffset = buffer.GetInteger();
            Types.Player[targetIndex].Access = buffer.GetByte();
        }

        private void HandlePlayerMove(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            int targetIndex = buffer.GetInteger();
            int targetX = buffer.GetInteger();
            int targetY = buffer.GetInteger();
            byte targetDirection = buffer.GetByte();
            int targetMoving = buffer.GetInteger();

            Types.Player[targetIndex].X = targetX;
            Types.Player[targetIndex].Y = targetY;
            Types.Player[targetIndex].Dir = targetDirection;

            Types.Player[targetIndex].XOffset = 0;
            Types.Player[targetIndex].YOffset = 0;
            Types.Player[targetIndex].Moving = targetMoving;

            switch (Types.Player[targetIndex].Dir)
            {
                case Constants.DIR_UP:
                    Types.Player[targetIndex].YOffset = 32;
                    Types.Player[targetIndex].Y -= 1;
                    break;
                case Constants.DIR_DOWN:
                    Types.Player[targetIndex].YOffset = 32 * -1;
                    Types.Player[targetIndex].Y += 1;
                    break;
                case Constants.DIR_LEFT:
                    Types.Player[targetIndex].XOffset = 32;
                    Types.Player[targetIndex].X -= 1;
                    break;
                case Constants.DIR_RIGHT:
                    Types.Player[targetIndex].XOffset = 32 * -1;
                    Types.Player[targetIndex].X += 1;
                    break;
            }

            GameLogic.ProcessMovement(targetIndex);
        }
    }
}
