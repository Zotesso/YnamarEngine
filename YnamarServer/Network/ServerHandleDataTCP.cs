using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database;
using YnamarServer.Database.Models;
using YnamarServer.Database.Protos;
using YnamarServer.GameLogic;
using YnamarServer.GameLogic.Items.Factory;
using YnamarServer.Network.Session;
using YnamarServer.Services;
using static YnamarServer.Network.NetworkPackets;

namespace YnamarServer.Network
{
    internal class ServerHandleDataTCP
    {
        private delegate void Packet(TcpClient client, byte[] data);
        private static Dictionary<int, Packet> Packets;
        private static ServerTCP stcp = ServerTCP.Instance;

        public void InitializeMessages()
        {
            Packets = new Dictionary<int, Packet>();

            Console.WriteLine("Initializing Packets");

            //Packets
            Packets.Add((int)ClientTcpPackets.CLogin, HandleLoginAsync);
            Packets.Add((int)ClientTcpPackets.CRegister, HandleRegister);
            Packets.Add((int)ClientTcpPackets.CPlayerMove, HandlePlayerMovement);
            Packets.Add((int)ClientTcpPackets.CItemUsed, HandleItemUsed);
        }

        public void HandleNetworkMessages(TcpClient client, byte[] data)
        {
            int packetNum;
            PacketBuffer buffer;
            buffer = new PacketBuffer();

            buffer.AddByteArray(data);
            packetNum = buffer.GetInteger();
            buffer.Dispose();

            if (Packets.TryGetValue(packetNum, out Packet Packet))
            {
                Packet.Invoke(client, data);
            }
        }

        private async void HandleLoginAsync(TcpClient client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            string username = buffer.GetString();
            string password = buffer.GetString();

            var myService = Program.accountService;
            int userId = await myService.Login(username, password);

            Character accChar = await myService.GetCharacterAsync(userId);

            var session = Program.SessionManager.CreateSession(userId, accChar.Map, client);
            int index = session.Index;
            InMemoryDatabase.Player[index] = accChar;

            Console.WriteLine("Player " + index + " Has logged in");

            SendUdpHandshakePacket(index, new UdpHandshakePacket { Index = session.Index,  Token = session.UdpToken });
            SendCharacterPackage(index, accChar);
            LoadMap(session, accChar.Map);
            Thread.Sleep(50);
            SendJoinMap(index);
            SendCharacterPackageToMap(index, accChar);
        }

        private void SendUdpHandshakePacket(int index, UdpHandshakePacket udpHandshake)
        {
            PacketBuffer bufferSend = new PacketBuffer();
            bufferSend.AddInteger((int)ServerPackets.SUdpHandshake);
            bufferSend.AddInteger(udpHandshake.Index);
            bufferSend.AddLong(udpHandshake.Token);
            stcp.SendPacket(index, bufferSend);

            bufferSend.Dispose();
        }

        private void SendCharacterPackage(int index, Character accChar)
        {
            PacketBuffer bufferSend = new PacketBuffer();
            bufferSend.AddInteger((int)ServerPackets.SJoinGame);
            bufferSend.AddInteger(index);

            byte[] charProtoBuf = bufferSend.SerializeProto(accChar);
            bufferSend.AddInteger(charProtoBuf.Length);
            bufferSend.AddByteArray(charProtoBuf);

            stcp.SendPacket(index, bufferSend);

            bufferSend.Dispose();
        }
        private void SendCharacterPackageToMap(int index, Character accChar)
        {
            PacketBuffer bufferSend = new PacketBuffer();
            bufferSend.AddInteger((int)ServerPackets.SPlayerData);
            bufferSend.AddInteger(index);

            byte[] charProtoBuf = bufferSend.SerializeProto(accChar);
            bufferSend.AddInteger(charProtoBuf.Length);
            bufferSend.AddByteArray(charProtoBuf);

            stcp.SendPacketToMap(accChar.Map, bufferSend);

            bufferSend.Dispose();
        }

        public void SendJoinMap(int index)
        {
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if (stcp.isConnected(i) && i != index && InMemoryDatabase.Player[i].Map == InMemoryDatabase.Player[index].Map)
                {
                    PacketBuffer buffer = new PacketBuffer();
                    buffer.AddInteger((int)ServerPackets.SPlayerData);
                    buffer.AddInteger(i);

                    byte[] charProtoBuf = buffer.SerializeProto(InMemoryDatabase.Player[i]);
                    buffer.AddInteger(charProtoBuf.Length);
                    buffer.AddByteArray(charProtoBuf);

                    stcp.SendPacket(index, buffer);
                    buffer.Dispose();
                }
            }
        }

        private void HandleRegister(TcpClient client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            string username = buffer.GetString();
            string password = buffer.GetString();
            var myService = Program.accountService;
            myService.RegisterUserAsync(username, password);
        }

        private void HandlePlayerMovement(TcpClient client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            byte dir = buffer.GetByte();
            int moving = buffer.GetInteger();

            var player = Program.SessionManager.GetByTcp(client);

            if (player is null) return;

            GameLogicHandler.PlayerMove(player.Index, dir, moving);
        }
        private async void LoadMap(PlayerSession session, int mapNum)
        {
            MapService mapService = Program.mapService;
            MapMetadata loadedMap = await mapService.LoadMap(mapNum);
            MapLoadDto mapLoadDto = new MapLoadDto
            {
                Id = loadedMap.Id,
                Name = loadedMap.Name,
                Width = loadedMap.MaxMapX,
                Height = loadedMap.MaxMapY,
                ChunkSize = 32,
                TileDefinitions = new MapRebuilder().LoadTileDefinitions(loadedMap.FilePath).Values.ToList(),
            };

            mapService.SendMapToClient(session.Index, mapLoadDto);
        }

        private void HandleItemUsed(TcpClient client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();
            var player = Program.SessionManager.GetByTcp(client);

            if (player is null) return;

            int playerIndex = player.PlayerId;
            int slot = buffer.GetInteger();

            ItemService itemService = Program.itemService;
            itemService.UseItem(playerIndex, slot);
            buffer.Dispose();
        }
    }
}
