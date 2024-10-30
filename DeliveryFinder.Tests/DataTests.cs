using System.Globalization;
using Serilog;

namespace DeliveryFinder.Tests;

public class DataTests
{
    private readonly DatabaseOperations _db;

    public DataTests()
    {
        ILogger logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
        _db = new DatabaseOperations(logger);
    }
    
    [Fact]
    public void DataIsNotEmpty_Test()
    {
        var readDb = _db.ReadDb().ToList();
        Assert.NotEmpty(readDb);
    }

    [Fact]
    public void DataIsNotNull_Test()
    {
        var readDb = _db.ReadDb().ToList();
        Assert.NotNull(readDb);
    }

    [Fact]
    public void DataIsNotNullWhenFilter_Test()
    {
        var readDb = _db.ReadDb(city: "A1", date: DateTime.ParseExact("2024-06-22 16:59:30", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)).ToList();
        Assert.NotNull(readDb);
    }
    
    [Fact]
    public void DataIsEmptyWhenFilter_Test()
    {
        var readDb = _db.ReadDb(city: "A3", date: DateTime.ParseExact("2024-06-22 16:59:30", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        Assert.Empty(readDb.ToList());
    }
}