using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static YnamarClient.Network.NetworkPackets;
using YnamarClient.GUI;
using YnamarClient.Database.Models;
using YnamarClient.Services;

namespace YnamarClient.Network
{
    internal class ClientHandleDataTCP
    {
        public PacketBuffer Buffer = new PacketBuffer();
        private delegate void Packet(int index, byte[] data);
        private static Dictionary<int, Packet> Packets;
        private static ClientTCP clienttcp = new ClientTCP();
        private static MapService mapService = new MapService();

        public void InitializeMessages()
        {
            Packets = new Dictionary<int, Packet>();

            Packets.Add((int)ServerPackets.SJoinGame, HandleJoinGame);
            Packets.Add((int)ServerPackets.SPlayerData, HandlePlayerData);
            Packets.Add((int)ServerPackets.SPlayerMove, HandlePlayerMove);
            Packets.Add((int)ServerPackets.SLoadMap, HandleLoadMap);
            Packets.Add((int)ServerPackets.SNpcMove, HandleNpcMove);
            Packets.Add((int)ServerPackets.SNpcKilled, HandleNpcKilled);
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
            int bufferLength = buffer.GetInteger();
            byte[] charBuff = buffer.GetByteArray(bufferLength);
            Types.Player[Globals.playerIndex] = buffer.DeserializeProto<Types.PlayerStruct>(charBuff);
            Types.Player[Globals.playerIndex].MaxHP = 500;
            Types.Player[Globals.playerIndex].HP = 423;
            clienttcp.SendLoadMap();
        }

        private void HandlePlayerData(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            int targetIndex = buffer.GetInteger();
            int bufferLength = buffer.GetInteger();
            byte[] charBuff = buffer.GetByteArray(bufferLength);
            Types.Player[targetIndex] = buffer.DeserializeProto<Types.PlayerStruct>(charBuff);
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
            Types.Player[targetIndex].Moving = 1;

            switch (Types.Player[targetIndex].Dir)
            {
                case Constants.DIR_UP:
                    Types.Player[targetIndex].YOffset = 32;
                    break;
                case Constants.DIR_DOWN:
                    Types.Player[targetIndex].YOffset = 32 * -1;
                    break;
                case Constants.DIR_LEFT:
                    Types.Player[targetIndex].XOffset = 32;
                    break;
                case Constants.DIR_RIGHT:
                    Types.Player[targetIndex].XOffset = 32 * -1;
                    break;
            }

            GameLogic.ProcessMovement(targetIndex);
        }

        private void HandleLoadMap(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            int targetIndex = buffer.GetInteger();
            int bufferLength = buffer.GetInteger();
            byte[] mapBuff = buffer.GetByteArray(bufferLength);
            Map deserializedMap = buffer.DeserializeProto<Map>(mapBuff);
            mapService.convertMapPayloadToClientMap(deserializedMap);
            MenuManager.ChangeMenu(MenuManager.Menu.InGame, Game1.desktop);
            GameLogic.InGame();
        }

        private void HandleNpcMove(int index, byte[] data)
        {
            if (!Globals.InGame) return;

            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            int mapNum = buffer.GetInteger();
            int layerNum = buffer.GetInteger();
            int mapNpcNum = buffer.GetInteger();

            int bufferLength = buffer.GetInteger();
            byte[] mapNpcBuff = buffer.GetByteArray(bufferLength);
            MapNpc deserializedMapNpc = buffer.DeserializeProto<MapNpc>(mapNpcBuff);

            Types.Map[mapNum].Layer[layerNum].MapNpc[mapNpcNum] = deserializedMapNpc;
        }

        private void HandleNpcKilled(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            int mapNum = buffer.GetInteger();
            int layerNum = buffer.GetInteger();
            int mapNpcNum = buffer.GetInteger();

            int bufferLength = buffer.GetInteger();
            byte[] mapNpcBuff = buffer.GetByteArray(bufferLength);
            MapNpc deserializedMapNpc = buffer.DeserializeProto<MapNpc>(mapNpcBuff);

            Types.Map[mapNum].Layer[layerNum].MapNpc[mapNpcNum] = deserializedMapNpc;
        }   
    }
}
