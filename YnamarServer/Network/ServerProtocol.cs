using ENet;
using System;
using System.Text;
using System.Threading.Channels;
using System.Xml;
using YnamarServer.Network;
using static YnamarServer.Network.NetworkPackets;

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
                        XmlDocument doc = new XmlDocument();

                        //doc.Load("C:\\Users\\sirio\\source\\repos\\YnamarEngine\\YnamarServer\\bin\\Debug\\net8.0\\playerData.xml");
                        doc.Load("playerData.xml");
                        XmlNode node = doc.DocumentElement.FirstChild;

                        string playerName = node.InnerText;

                       // byte[] data = Encoding.ASCII.GetBytes(playerName);

                        PacketBuffer buffer = new PacketBuffer();
                        buffer.AddInteger((int)ServerPackets.SJoinGame);
                        buffer.AddInteger(0); // indice do player
                        buffer.AddString(playerName);

                        SendData(0, buffer.ToArray(), netEvent);
                        buffer.Dispose();
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

    public void SendData(int index, byte[] data, Event netEvent)
    {
        Packet packet = default(Packet);
        packet.Create(data);
        netEvent.Peer.Send(netEvent.ChannelID, ref packet);
    }
}
