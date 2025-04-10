﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database.Models;
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
            SendCharacterPackage(index, accChar);
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
    }
}
