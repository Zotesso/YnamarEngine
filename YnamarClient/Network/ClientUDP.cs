using ENet;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Text;

namespace YnamarClient.Network
{
    internal class ClientUDP
    {
        private ClientHandleData clientDataHandle;
        private Host client;
        private Peer serverPeer;

        public void ConnectToServer()
        {
            clientDataHandle = new ClientHandleData();
            Library.Initialize();


            client = new Host();

                Address address = new Address();

                address.SetHost("127.0.0.1");
                address.Port = 8081;
                client.Create();

                client.Connect(address);

                Event netEvent;

                while (true)
                {
                    //bool polled = false;

                if (client.Service(15, out netEvent) > 0)
                {
                    switch (netEvent.Type)
                    {
                        case EventType.Connect:
                            Console.WriteLine("Connected to server!");
                            serverPeer = netEvent.Peer; // <-- assign the real peer here
                            break;

                        case EventType.Receive:
                            Console.WriteLine("Got data: " + netEvent.Packet.Length);
                            netEvent.Packet.Dispose();
                            break;

                        case EventType.Disconnect:
                            Console.WriteLine("Disconnected.");
                            serverPeer = default(Peer);
                            break;
                    }
                }
            }

        }
        public static byte[] TrimEnd(byte[] array)
        {
            int lastIndex = Array.FindLastIndex(array, b => b != 0);

            Array.Resize(ref array, lastIndex + 1);

            return array;
        }

        public void SendData(byte[] data)
        {
            if (serverPeer.IsSet && serverPeer.State == PeerState.Connected)
            {
                Packet packet = default(Packet);
                packet.Create(data);
                serverPeer.Send(0, ref packet);
                client.Flush();
            }
            else
            {
                Console.WriteLine("Not connected yet, cannot send!");
            }
        }

        public void SendPlayerAttack()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.AddInteger((int)NetworkPackets.ClientUdpPackets.UdpCAttack);

            buffer.AddByte(Types.Player[Globals.playerIndex].Dir);
            SendData(buffer.ToArray());
            buffer.Dispose();
        }
    }
}
