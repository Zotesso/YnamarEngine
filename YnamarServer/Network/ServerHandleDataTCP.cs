using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database;
using YnamarServer.Database.Models;
using YnamarServer.GameLogic;
using YnamarServer.Services;
using static YnamarServer.Network.NetworkPackets;

namespace YnamarServer.Network
{
    internal class ServerHandleDataTCP
    {
        private delegate void Packet(int index, byte[] data);
        private static Dictionary<int, Packet> Packets;
        private static ServerTCP stcp = new ServerTCP();

        public void InitializeMessages()
        {
            Packets = new Dictionary<int, Packet>();

            Console.WriteLine("Initializing Packets");

            //Packets
            Packets.Add((int)ClientTcpPackets.CLogin, HandleLoginAsync);
            Packets.Add((int)ClientTcpPackets.CRegister, HandleRegister);
            Packets.Add((int)ClientTcpPackets.CPlayerMove, HandlePlayerMovement);
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

        private async void HandleLoginAsync(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            string username = buffer.GetString();
            string password = buffer.GetString();

            var myService = Program.accountService;
            int userId = await myService.Login(username, password);
            Console.WriteLine("Player " + index + " Has logged in");

            Character accChar = await myService.GetCharacterAsync(userId);
            InMemoryDatabase.Player[index] = accChar;
            SendCharacterPackage(index, accChar);
            SendJoinMap(index);
            SendCharacterPackageToMap(index, accChar);
        }

        private void SendCharacterPackage(int index, Character accChar)
        {
            PacketBuffer bufferSend = new PacketBuffer();
            bufferSend.AddInteger((int)ServerPackets.SJoinGame);
            bufferSend.AddInteger(index);
            bufferSend.AddString(accChar.Name);
            bufferSend.AddInteger(accChar.Sprite);
            bufferSend.AddInteger(accChar.Level);
            bufferSend.AddInteger(accChar.EXP);
            bufferSend.AddInteger(accChar.Map);

            bufferSend.AddInteger(accChar.X);
            bufferSend.AddInteger(accChar.Y);
            bufferSend.AddByte(accChar.Dir);

            bufferSend.AddInteger(accChar.XOffset);
            bufferSend.AddInteger(accChar.YOffset);
            bufferSend.AddByte(accChar.Access);
            
            stcp.SendData(index, bufferSend.ToArray());

            bufferSend.Dispose();
        }
        private void SendCharacterPackageToMap(int index, Character accChar)
        {
            PacketBuffer bufferSend = new PacketBuffer();
            bufferSend.AddInteger((int)ServerPackets.SPlayerData);
            bufferSend.AddInteger(index);
            bufferSend.AddString(accChar.Name);
            bufferSend.AddInteger(accChar.Sprite);
            bufferSend.AddInteger(accChar.Level);
            bufferSend.AddInteger(accChar.EXP);
            bufferSend.AddInteger(accChar.Map);

            bufferSend.AddInteger(accChar.X);
            bufferSend.AddInteger(accChar.Y);
            bufferSend.AddByte(accChar.Dir);

            bufferSend.AddInteger(accChar.XOffset);
            bufferSend.AddInteger(accChar.YOffset);
            bufferSend.AddByte(accChar.Access);

            stcp.SendDataToMap(accChar.Map, bufferSend.ToArray());

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

                    buffer.AddString(InMemoryDatabase.Player[i].Name);
                    buffer.AddInteger(InMemoryDatabase.Player[i].Sprite);
                    buffer.AddInteger(InMemoryDatabase.Player[i].Level);
                    buffer.AddInteger(InMemoryDatabase.Player[i].EXP);
                    buffer.AddInteger(InMemoryDatabase.Player[i].Map);

                    buffer.AddInteger(InMemoryDatabase.Player[i].X);
                    buffer.AddInteger(InMemoryDatabase.Player[i].Y);
                    buffer.AddByte(InMemoryDatabase.Player[i].Dir);

                    buffer.AddInteger(InMemoryDatabase.Player[i].XOffset);
                    buffer.AddInteger(InMemoryDatabase.Player[i].YOffset);
                    buffer.AddByte(InMemoryDatabase.Player[i].Access);

                    Console.WriteLine(i.ToString());
                    Console.WriteLine(InMemoryDatabase.Player[0].X.ToString());

                    stcp.SendData(index, buffer.ToArray());
                    buffer.Dispose();
                }
            }
        }

        private void HandleRegister(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            string username = buffer.GetString();
            string password = buffer.GetString();
            var myService = Program.accountService;
            myService.RegisterUserAsync(username, password);
        }

        private void HandlePlayerMovement(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddByteArray(data);
            buffer.GetInteger();

            byte dir = buffer.GetByte();
            int moving = buffer.GetInteger();

            GameLogicHandler.PlayerMove(index, dir, moving);
        }
    }
}
