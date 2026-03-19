using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarClient.GUI;
using YnamarClient.Database.Models;
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

            Packets.Add((int)ServerUdpPackets.UdpSNpcAttacked, HandleNpcAttacked);
            Packets.Add((int)ServerUdpPackets.UdpSNpcMove, HandleNpcMove);
            Packets.Add((int)ServerUdpPackets.UdpSPlayerAttacking, HandlePlayerAttacking);
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

        private void HandleNpcAttacked(int index, byte[] data)
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

        private void HandlePlayerAttacking(int index, byte[] data)
        {
            if (!Globals.InGame) return;

            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            int playerIndex = buffer.GetInteger();
            byte playerDir = buffer.GetByte();
            Types.Players[playerIndex].Dir = playerDir;
            Types.Players[playerIndex].Attacking = true;

            if (Types.Players[playerIndex].EquippedItems is not null && Types.Players[playerIndex].EquippedItems.ElementAt(0).Item.AnimationClip is not null)
            {
                Types.Players[playerIndex].WeaponAnim.Play(Types.Players[playerIndex].EquippedItems.ElementAt(0).Item.AnimationClip);
            }

            buffer.Dispose();
        }
    }
}
