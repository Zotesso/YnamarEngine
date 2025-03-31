
using YnamarServer;
using YnamarServer.Network;

public class General
{
    private ServerProtocol? serverProtocol;
    private ServerHandleData handleServerData;

    private ServerTCP stcp;
    private ServerHandleDataTCP handleServerDataTcp;

    public void initializeServer()
    {
        serverProtocol = new ServerProtocol();
        handleServerData = new ServerHandleData();
        handleServerData.InitializeMessages();

        serverProtocol.initializeNetwork();
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