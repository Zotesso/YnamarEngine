using System;
using System.Threading;
internal class Program
{
    private static General? general;
    private static Thread? consoleThread;

    private static void Main(string[] args)
    {
        Console.WriteLine("Initializing Server!");
        general = new General();
        consoleThread = new Thread(new ThreadStart(ConsoleThread));
        consoleThread.Start();
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