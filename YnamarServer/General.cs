
using YnamarServer;
using YnamarServer.Database;
using YnamarServer.Database.Models;
using YnamarServer.Network;
using YnamarServer.Services;

public class General
{
    private ServerProtocol? serverProtocol;
    private ServerHandleData handleServerData;

    private ServerHandleDataTCP handleServerDataTcp;

    public async Task LoadInMemoryResources()
    {
        MapService mapService = Program.mapService;
        NpcService npcService = Program.npcService;

        var maps = await mapService.LoadAllMaps();

        foreach (var map in maps)
        {
            InMemoryDatabase.Maps[map.Id] = map;
        }

        InMemoryDatabase.Npcs = (await npcService.LoadAllNpcs()).ToArray();
        InMemoryDatabase.Items = (await Program.itemService.LoadAllItems()).ToArray();
    }

    public void initializeServer()
    {
        handleServerData = new ServerHandleData();
        handleServerData.InitializeMessages();

        NetworkManager.ServerUdp.initializeNetwork();
    }

    public void initializeTCPServer()
    {
        var stcp = ServerTCP.Instance;

        handleServerDataTcp = new ServerHandleDataTCP();
        handleServerDataTcp.InitializeMessages();

        stcp.InitializeNetwork();
    }
}