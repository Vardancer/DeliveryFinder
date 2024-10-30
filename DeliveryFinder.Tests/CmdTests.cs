using System.CommandLine;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.Diagnostics;
using System.Text;
using Serilog;

namespace DeliveryFinder.Tests;

public class CmdTests
{
    ILogger logger;
    private DatabaseOperations db;
    private IConsole console = new TestConsole();
    
    public CmdTests()
    {
        logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
        db = new DatabaseOperations(logger);
    }
    
    [Fact]
    public void Cmd_Test()
    {
        var c = new CommandLine(logger, db);
        var command = c.SetupOptions();
        var result = command.Invoke("--help", console);
        Assert.True(result >= 1);
    }
}