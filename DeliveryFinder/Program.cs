using System.CommandLine;
using System.Globalization;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;
using Serilog;

namespace DeliveryFinder;

internal class Program
{
    // static 
    static int Main(string[] args)
    {
        // Setup.SetupOptions(args);

        var cityOption = new Option<string>(
            aliases: ["-c", "--city"],
            description: "City to search"){IsRequired = true};
        
        var dateOption = new Option<DateTime>(
            aliases: ["-d", "--date"],
            getDefaultValue: () => DateTime.Now,
            description: "Date");
        
        var logFileOption = new Option<string>(
            name: "--log",
            getDefaultValue: () => Path.Join(Environment.CurrentDirectory, "DeliveryLog.txt"),
            description: "Log file");

        var arg1 = new Argument<string>("city");
        var arg2 = new Argument<DateTime>("date");

        var rootCommand = new RootCommand("Sample Application");
        rootCommand.AddOption(cityOption);
        rootCommand.AddOption(dateOption);
        rootCommand.AddOption(logFileOption);
        
        var com1 = new Command("test1", "Test 1");
        com1.Add(arg1);
        com1.Add(arg2);
        com1.SetHandler((arg1, arg2) => { Console.WriteLine($"{arg1}--{arg2}"); }, arg1, arg2);
        
        rootCommand.AddCommand(com1);
        
        rootCommand.SetHandler((city, date, log) =>
        {
            Setup.SetupLogger(log);
            ReadDb(city, date);
        }, cityOption, dateOption, logFileOption);
        return rootCommand.Invoke(args);
        
    }

    static void ReadDb(string? city = null, DateTime? date = null)
    {
        var logger = Log.Logger;
        logger.Information("Starting Delivery Finder");
        
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            Quote = '"'
        };
        List<DTO> records;
        using var stream = File.OpenRead(Path.Join(Environment.CurrentDirectory, "Data.csv"));
        using var reader = new StreamReader(stream);
        using (var csv = new CsvReader(reader, csvConfig))
        {
            records = csv.GetRecords<DTO>().ToList();
        }


        foreach (var record in records)
        {
            Console.WriteLine(record.ToString());
        }
    }
}