using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Serilog;

namespace DeliveryFinder;

public class DatabaseOperations
{
    private readonly ILogger _logger;

    public DatabaseOperations(ILogger logger)
    {
        _logger = logger;
    }
    public IEnumerable<DTO> ReadDb(string city = "A0", DateTime? date = null)
    {
        _logger.Information("Searching...");
        
        var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
        {
            Delimiter = ";",
            Quote = '"'
        };
        using var stream = File.OpenRead(Path.Join(Environment.CurrentDirectory, "Data.csv"));
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, csvConfig);
        if(city == "A0" || date is null)
            return csv.GetRecords<DTO>().ToList();
        
        return csv.GetRecords<DTO>()
            .Where(w => w.OrderDistrict == city 
                        && w.DateTime >= date.Value.Add(new TimeSpan(0, 5, 0)))
            .ToList();
    }
}