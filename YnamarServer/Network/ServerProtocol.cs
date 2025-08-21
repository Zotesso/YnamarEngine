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
    private ServerHandleData handleServerData;

    public void initializeNetwork()
    {
        ENet.Library.Initialize();
        handleServerData = new ServerHandleData();

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

    // Resolver essa questão aqui, ainda não é possivel mandar os dados pro cliente por que o peer esta somento no escopo ali da conexão
    // Também ver como será possivel via UDP enviar o dado pra todos que estiverem no mapa.
    public void SendData(int index, byte[] data)
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
    public void SendData(int index, byte[] data, Event netEvent)
    {
        Packet packet = default(Packet);
        packet.Create(data);
        netEvent.Peer.Send(netEvent.ChannelID, ref packet);
    }

    public void SendDataToMap(int mapNum, byte[] data)
    {
        for (int i = 0; i < Constants.MAX_PLAYERS; i++)
        {
            if (isConnected(i) && isPlaying(i))
            {
                if (InMemoryDatabase.Player[i].Map == mapNum)
                {
                    SendData(i, data);
                }
            }
        }
    }
}
