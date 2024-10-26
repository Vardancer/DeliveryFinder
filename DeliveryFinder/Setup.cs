using System.Reflection;
using Serilog;

namespace DeliveryFinder;

internal static class Setup
{
    private static string LogFile { get; set; } = Assembly.GetEntryAssembly().GetName().Name + ".log";
    internal static void SetupOptions(string[] args)
    {
        
        if (args.Length == 0 || args.Length < 2)
        {
            Console.WriteLine("Usage: {0} [OPTIONS]", Assembly.GetExecutingAssembly().GetName().Name);
            Console.WriteLine("Options:");
            Console.WriteLine("  -city --cityDistrict <A1> Район доставки");
            Console.WriteLine("  -toDate --firstDeliveryDateTime <2024-06-23 15:25:59> Время первой доставки");
            Environment.Exit(1);
        }

    }

    public static void SetupLogger(string logFile)
    {
        // if(LogFile == null) Console.WriteLine("Log file not specified");
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(logFile, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            .CreateLogger();
        
        Log.Logger.Information("Starting...");
    }
}