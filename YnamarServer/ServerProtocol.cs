using ENet;
/// <summary>
/// Summary description for Class1
/// </summary>
public class ServerProtocol
{
    public void initializeNetwork()
    {
        ENet.Library.Initialize();
        using Host server = new();
        Address address = new()
        {
            Port = 8081
        };
        server.Create(address, 100);

        Event netEvent;

        while (!Console.KeyAvailable)
        {
            bool polled = false;

            while (!polled)
            {
                if (server.CheckEvents(out netEvent) <= 0)
                {
                    if (server.Service(15, out netEvent) <= 0)
                        break;

                    polled = true;
                }

                switch (netEvent.Type)
                {
                    case EventType.None:
                        break;

                    case EventType.Connect:
                        Console.WriteLine("Client connected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                        break;

                    case EventType.Disconnect:
                        Console.WriteLine("Client disconnected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                        break;

                    case EventType.Timeout:
                        Console.WriteLine("Client timeout - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                        break;

                    case EventType.Receive:
                        Console.WriteLine("Packet received from - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP + ", Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length);
                        netEvent.Packet.Dispose();
                        break;
                }
            }
        }

        server.Flush();

    }
}
