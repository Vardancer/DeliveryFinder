using System.CommandLine;
using Autofac;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace DeliveryFinder;

internal class Program
{
    private static IContainer Container { get; set; }
    static int Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appConfig.json")
            .Build();
        
        var builder = new ContainerBuilder();
        
        builder.RegisterType<CommandLine>().AsSelf();
        builder.RegisterType<DatabaseOperations>().AsSelf();

        builder.Register<ILogger>((context, parameters) => 
            new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger());
        
        Container = builder.Build();
        var app = StartApp(args);
        return app;
    }

    static int StartApp(string[] args)
    {
        using (var scope = Container.BeginLifetimeScope())
        {
            var cc = scope.Resolve<CommandLine>();
            var rootCommand = cc.SetupOptions();
            return rootCommand.Invoke(args);
        }
    }
}