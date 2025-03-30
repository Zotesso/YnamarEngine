using ENet;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Text;

namespace YnamarClient.Network
{
    internal class ClientUDP
    {
        private ClientHandleData clientDataHandle;

        public void ConnectToServer()
        {
            clientDataHandle = new ClientHandleData();
            Library.Initialize();
            
            using (Host client = new Host())
            {
                Address address = new Address();

                address.SetHost("127.0.0.1");
                address.Port = 8081;
                client.Create();

                Peer peer = client.Connect(address);

                Event netEvent;

                while (true)
                {
                    bool polled = false;

                    while (!polled)
                    {
                        if (client.CheckEvents(out netEvent) <= 0)
                        {
                            if (client.Service(15, out netEvent) <= 0)
                                break;

                            polled = true;
                        }

                        switch (netEvent.Type)
                        {
                            case EventType.None:
                                break;

                            case EventType.Connect:
                                Console.WriteLine("Client connected to server");
                                break;

                            case EventType.Disconnect:
                                Console.WriteLine("Client disconnected from server");
                                break;

                            case EventType.Timeout:
                                Console.WriteLine("Client connection timeout");
                                break;

                            case EventType.Receive:
                                Console.WriteLine("Packet received from server - Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length);
                                byte[] buffer = new byte[1024];
                                netEvent.Packet.CopyTo(buffer);
                                buffer = TrimEnd(buffer);

                                clientDataHandle.HandleNetworkMessages(0, buffer);
                                netEvent.Packet.Dispose();
                                break;
                        }
                    }
                }

                client.Flush();
            }

        }
        public static byte[] TrimEnd(byte[] array)
        {
            int lastIndex = Array.FindLastIndex(array, b => b != 0);

            Array.Resize(ref array, lastIndex + 1);

            return array;
        }
    }
}
