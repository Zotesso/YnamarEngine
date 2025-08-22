﻿
using YnamarServer;
using YnamarServer.Database;
using YnamarServer.Database.Models;
using YnamarServer.Network;
using YnamarServer.Services;

public class General
{
    private ServerProtocol? serverProtocol;
    private ServerHandleData handleServerData;

    private ServerTCP stcp;
    private ServerHandleDataTCP handleServerDataTcp;

    public async Task LoadInMemoryResources()
    {
        MapService mapService = Program.mapService;
        InMemoryDatabase.Maps = (await mapService.LoadAllMaps()).ToArray();
    }

    public void initializeServer()
    {
        handleServerData = new ServerHandleData();
        handleServerData.InitializeMessages();

        NetworkManager.ServerUdp.initializeNetwork();
    }

    public void initializeTCPServer()
    {
        stcp = new ServerTCP();
        handleServerDataTcp = new ServerHandleDataTCP();
        handleServerDataTcp.InitializeMessages();

        for (int i = 0; i < Constants.MAX_PLAYERS; i++)
        {
            ServerTCP.Clients[i] = new ClientTCP();
        }


        stcp.InitializeNetwork();
    }
}