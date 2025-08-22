using ENet;
using System;
using System.Text;
using System.Threading.Channels;
using System.Xml;
using YnamarServer.Network;
using YnamarServer.Database;
using static YnamarServer.Network.NetworkPackets;

/// <summary>
/// Summary description for Class1
/// </summary>
public class ServerProtocol
{
    private ServerHandleData handleServerData;
    private Host server;

    private Dictionary<uint, Peer> connectedClients = new();

    public void initializeNetwork()
    {
        ENet.Library.Initialize();
        handleServerData = new ServerHandleData();

        server = new Host();
        Address address = new()
        {
            Port = 8081
        };
        server.Create(address, 100);

        Event netEvent;

        while (true)
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
                        connectedClients[netEvent.Peer.ID] = netEvent.Peer;
                        break;

                    case EventType.Disconnect:
                        Console.WriteLine("Client disconnected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                        connectedClients.Remove(netEvent.Peer.ID);
                        break;

                    case EventType.Timeout:
                        Console.WriteLine("Client timeout - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                        connectedClients.Remove(netEvent.Peer.ID);
                        break;

                    case EventType.Receive:
                        Console.WriteLine("Packet received from - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP + ", Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length);
                        byte[] buffer = new byte[netEvent.Packet.Length];
                        netEvent.Packet.CopyTo(buffer);

                        handleServerData.HandleNetworkMessages(netEvent.ChannelID, buffer);
                        netEvent.Packet.Dispose();
                        break;
                }
            }
        }

        server.Flush();
    }

    public void SendData(uint clientId, byte[] data, byte channel = 0)
    {
        if (connectedClients.TryGetValue(clientId, out Peer peer))
        {
            Packet packet = default;
            packet.Create(data, PacketFlags.Reliable);
            peer.Send(channel, ref packet);
            Console.WriteLine($"Sent {data.Length} bytes to client {clientId}");
        }
        else
        {
            Console.WriteLine($"Client {clientId} not found.");
        }
    }


    public void Broadcast(byte channel, byte[] data)
    {
        Packet packet = default;
        packet.Create(data, PacketFlags.Reliable);

        foreach (var peer in connectedClients.Values)
        {
            peer.Send(channel, ref packet);
        }

        Console.WriteLine($"Broadcasted {data.Length} bytes to {connectedClients.Count} clients.");
    }

    public void SendDataToMap(int mapNum, byte[] data)
    {
        for (int i = 0; i < YnamarServer.Constants.MAX_PLAYERS; i++)
        {
            if (isConnected(i) && isPlaying(i))
            {
                if (InMemoryDatabase.Player[i].Map == mapNum)
                {
                    SendData((uint)i, data);
                }
            }
        }
    }

    public bool isConnected(int index)
    {
        return connectedClients.TryGetValue((uint)index, out Peer peer);
    }

    public bool isPlaying(int index)
    {
        return InMemoryDatabase.Player[index] != null;
    }

}
