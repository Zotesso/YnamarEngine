using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using YnamarServer.Admin.Controllers;
using YnamarServer.Admin.Services;
using YnamarServer.GameLogic;
using YnamarServer.Services;

internal class Program
{
    private static Stopwatch _stopwatch = Stopwatch.StartNew();
    public static long CurrentTick { get; private set; }

    private static General? general;
    private static Thread? consoleThread;
    private static Thread? tcpServerThread;
    private static Thread? udpServerThread;
    private static Thread? gameLoopThread;
    
    private static YnamarServer.Database.Database database;
    public static AccountService accountService;
    public static MapService mapService;
    public static MapEditorService mapEditorService;
    public static NpcEditorService npcEditorService;
    public static ItemEditorService itemEditorService;
    public static NpcService npcService;

    private static async Task Main(string[] args)
    {
        database = new YnamarServer.Database.Database();

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json");
                //config.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                database.ConfigureDatabase(context.Configuration, services);
                services.AddControllers().AddProtoBufNet()
                .AddApplicationPart(typeof(MapEditorController).Assembly)
                .AddApplicationPart(typeof(NpcEditorController).Assembly)
                .AddApplicationPart(typeof(ItemEditorController).Assembly);
            })
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseKestrel(option =>
                {   
                    option.ListenAnyIP(8080);
                });

                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                });

            })
            .Build();

        var routeEndpoints = host.Services.GetRequiredService<EndpointDataSource>().Endpoints;
        foreach (var ep in routeEndpoints)
            Console.WriteLine(ep.DisplayName);

        var serviceScopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
        accountService = new AccountService(serviceScopeFactory);
        mapService = new MapService(serviceScopeFactory);
        mapEditorService = new MapEditorService(serviceScopeFactory);
        npcEditorService = new NpcEditorService(serviceScopeFactory);
        itemEditorService = new ItemEditorService(serviceScopeFactory);
        npcService = new NpcService(serviceScopeFactory);

        Console.WriteLine("Initializing Server!");
        general = new General();
        consoleThread = new Thread(new ThreadStart(ConsoleThread));

        tcpServerThread = new Thread(new ThreadStart(general.initializeTCPServer));
        udpServerThread = new Thread(new ThreadStart(general.initializeServer));
        gameLoopThread = new Thread(new ThreadStart(GameLoopThread));

        await general.LoadInMemoryResources();

        consoleThread.Start();
        tcpServerThread.Start();
        gameLoopThread.Start();
        udpServerThread.Start();

        await host.RunAsync();
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

    private static void GameLoopThread()
    {
        long tick, elapsedTime, frameTime = 0;
        long tmr25 = 0, tmr500 = 0, tmr1000 = 0;
        long lastUpdateSavePlayers = 0, lastUpdateMapSpawnItems = 0, lastUpdatePlayerVitals = 0;

        bool serverOnline = true;

        while (serverOnline)
        {
            tick = _stopwatch.ElapsedMilliseconds;
            elapsedTime = tick - frameTime;
            frameTime = tick;

            CurrentTick = tick;

            if (tick >= tmr500)
            {
                // Run your 500ms logic
                MapLogicHandler.UpdateAllMaps();
                tmr500 = tick + 500;
            }
        }
    }
}