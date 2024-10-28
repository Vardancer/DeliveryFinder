using System.Globalization;
using Serilog;

namespace DeliveryFinder.Tests;

public class DataTests
{
    ILogger _logger;

    public DataTests()
    {
        _logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
    }
    
    [Fact]
    public void DataIsNotEmpty_Test()
    {
        var db = new DatabaseOperations(logger: _logger);
        var readDb = db.ReadDb().ToList();
        Assert.NotEmpty(readDb);
    }

    [Fact]
    public void DataIsNotNull_Test()
    {
        var db = new DatabaseOperations(logger: _logger);
        var readDb = db.ReadDb().ToList();
        Assert.NotNull(readDb);
    }

    [Fact]
    public void DataIsNotNullWhenFilter_Test()
    {
        var db = new DatabaseOperations(logger: _logger);
        var readDb = db.ReadDb(city: "A1", date: DateTime.ParseExact("2024-06-22 16:59:30", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).ToList();
        Assert.Null(readDb);
    }
    
    [Fact]
    public void DataIsEmptyWhenFilter_Test()
    {
        var db = new DatabaseOperations(logger: _logger);
        var readDb = db.ReadDb(city: "A3", date: DateTime.ParseExact("2024-06-22 16:59:30", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        Assert.Empty(readDb.ToList());
    }
}