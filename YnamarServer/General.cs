
using YnamarServer.Network;

public class General
{
    private ServerProtocol? serverProtocol;
    private ServerHandleData handleServerData;


    public void initializeServer()
    {
        serverProtocol = new ServerProtocol();
        handleServerData = new ServerHandleData();

        handleServerData.InitializeMessages();
        serverProtocol.initializeNetwork();
    }
}