using System.CommandLine;
using System.Reflection;
using Serilog;

namespace DeliveryFinder;

public class CommandLine
{
    private readonly ILogger _logger;
    private readonly DatabaseOperations _db;

    public CommandLine(ILogger logger, DatabaseOperations db)
    {
        _logger = logger;
        _db = db;
    }
    public RootCommand SetupOptions()
    {
        var cityOption = new Option<string>(
            aliases: ["-c", "--city"],
            description: "City to search"){IsRequired = true, Arity = ArgumentArity.ExactlyOne};
        
        var dateOption = new Option<DateTime>(
            aliases: ["-d", "--date"],
            getDefaultValue: () => DateTime.Now,
            description: "Date");
        
        var resultOption = new Option<string>(
            aliases: ["-r", "--result"],
            description: "путь к файлу с результатом выборки");
        
        var rootCommand = new RootCommand("Delivery Finder\nBy default application will list nearest deliveries");
        rootCommand.AddOption(cityOption);
        rootCommand.AddOption(dateOption);
        rootCommand.AddOption(resultOption);
        
        _logger.Information("Program started");

        rootCommand.SetHandler((city, date, resultDst) =>
            {
                Printer(city, date, resultDst);
            },
            cityOption, dateOption, resultOption);
        return rootCommand;
    }

    private void Printer(string city, DateTime date)
    {
        var dtos = _db.ReadDb(city, date);
            
        Console.WriteLine("'Order ID' 'Weight, g' 'Order District' DateTime");
        foreach (var dto in dtos)
        {
            Console.WriteLine(dto.ToString());
        }
    }

    private void Printer(string city, DateTime date, string? resultDst)
    {
        if (string.IsNullOrEmpty(resultDst))
        {
            _logger.Warning("You did not provide a result destination for delivery");
            Printer(city, date);
            return;
        }

        var dtos = _db.ReadDb(city, date);
        using var writer = new StreamWriter(resultDst);
        writer.WriteLine("'Номер заказа' 'Вес, грамм' 'Район' Время доставки");
        foreach (var dto in dtos)
        {
            writer.WriteLine(dto.ToString());
        }
    }
}