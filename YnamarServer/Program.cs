using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YnamarServer.Services;

internal class Program
{
    private static General? general;
    private static Thread? consoleThread;
    private static Thread? tcpServerThread;

    private static YnamarServer.Database.Database database;
    public static AccountService accountService;
    private static void Main(string[] args)
    {
        database = new YnamarServer.Database.Database();
        var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json");
                //config.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) => database.ConfigureDatabase(context.Configuration, services))
            .Build();

        var serviceScopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
        accountService = new AccountService(serviceScopeFactory);

        Console.WriteLine("Initializing Server!");
        general = new General();
        consoleThread = new Thread(new ThreadStart(ConsoleThread));

        tcpServerThread = new Thread(new ThreadStart(general.initializeTCPServer));

        consoleThread.Start();
        tcpServerThread.Start();
        general.initializeServer();
    }

    private static void ConsoleThread()
    {
        var cmnd = Console.ReadLine();

        if (cmnd == "SetAccess")
        {
            Console.WriteLine("--------Nome do jogador-------");
            _ = Console.ReadLine();
            Console.WriteLine("--------Tier do Acesso 0 = sem acesso, 10 = acesso maximo-------");
            byte accessToBeSeted = Convert.ToByte(Console.ReadLine());
        }

        ConsoleThread();
    }
}