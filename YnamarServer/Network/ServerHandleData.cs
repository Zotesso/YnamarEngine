using ENet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database;
using YnamarServer.Database.Models;
using YnamarServer.GameLogic;
using YnamarServer.Network.Session;
using YnamarServer.Services;
using static YnamarServer.Network.NetworkPackets;

namespace YnamarServer.Network
{
    internal class ServerHandleData
    {
        private delegate void Packet(Peer peer, byte channel, byte[] data);
        private static Dictionary<int, Packet> Packets;

        public void InitializeMessages()
        {
            Packets = new Dictionary<int, Packet>();

            Console.WriteLine("Initializing Packets");

            //Packets
            Packets.Add((int)ClientUdpPackets.UdpCHandshake, HandleUdpHandshake);
            Packets.Add((int)ClientUdpPackets.UdpCAttack, HandlePlayerAttack);
        }

        public void HandleNetworkMessages(Peer peer, byte channel, byte[] data)
        {
            int packetNum;
            PacketBuffer buffer;
            buffer = new PacketBuffer();

            buffer.AddByteArray(data);
            packetNum = buffer.GetInteger();
            buffer.Dispose();

            if (Packets.TryGetValue(packetNum, out Packet Packet))
            {
                Packet.Invoke(peer, channel, data);
            }
        }
        private void HandleUdpHandshake(Peer peer, byte channel, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            int index = buffer.GetInteger();
            long token = buffer.GetLong();

            bool success = Program.SessionManager.RegisterPeer(peer.ID, index, token);

            if (!success)
            {
                Console.WriteLine($"[SECURITY] Invalid UDP handshake from Peer {peer.ID}");
                peer.Disconnect(0);
                return;
            }

            Console.WriteLine($"UDP authenticated: Peer {peer.ID} -> Player {index}");
            buffer.Dispose();
        }

        public void HandlePlayerAttack(Peer peer, byte channel, byte[] data)
        {
            PlayerSession session = Program.SessionManager.GetByUdpPeer(peer.ID);

            if (session == null)
            {
                Console.WriteLine($"[SECURITY] Unauthenticated UDP packet from {peer.ID}");
                peer.Disconnect(0);
                return;
            }

            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            byte dir = buffer.GetByte();

            SendPlayerAttackToMap(session.Index, dir);
            GameLogicHandler.PlayerAttack(session, dir);
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
