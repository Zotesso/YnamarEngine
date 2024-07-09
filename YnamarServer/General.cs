
public class General
{
    private ServerProtocol? serverProtocol;


    public void initializeServer()
    {
        serverProtocol = new ServerProtocol();
        serverProtocol.initializeNetwork();
    }
}