using ENet;
using System;

namespace YnamarClient
{
    internal class ClientUDP
    {
        public void ConnectToServer() {
            ENet.Library.Initialize();

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
                                netEvent.Packet.Dispose();
                                break;
                        }
                    }
                }

                client.Flush();
            }

        }
    }
}
