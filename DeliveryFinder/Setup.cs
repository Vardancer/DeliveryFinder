using System.CommandLine;
using System.Reflection;
using Serilog;

namespace DeliveryFinder;

internal class Setup
{
    private readonly ILogger _logger;
    private readonly DatabaseOperations _db;

    public Setup(ILogger logger, DatabaseOperations db)
    {
        _logger = logger;
        _db = db;
    }
    public RootCommand SetupOptions()
    {
        var cityOption = new Option<string>(
            aliases: ["-c", "--city"],
            description: "City to search"){IsRequired = true};
        
        var dateOption = new Option<DateTime>(
            aliases: ["-d", "--date"],
            getDefaultValue: () => DateTime.Now,
            description: "Date");
        
        var resultDst = new Option<string>(
            aliases: ["-r", "--result"],
            description: "путь к файлу с результатом выборки");
        
        var rootCommand = new RootCommand("Delivery Finder\nBy default application will list nearest deliveries");
        rootCommand.AddOption(cityOption);
        rootCommand.AddOption(dateOption);
        rootCommand.AddOption(resultDst);
        
        _logger.Information("Program started");
        
        rootCommand.SetHandler((city, date) =>
        {
            var dtos = _db.ReadDb(city, date);
            Console.WriteLine(dtos);
        }, cityOption, dateOption);
        
        return rootCommand;
    }
}