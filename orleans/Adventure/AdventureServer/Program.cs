using System.Reflection;
using AdventureSetup;
using Grace.DependencyInjection;
using Grace.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
var mapFileName = Path.Combine(path, "AdventureMap.json");

switch (args.Length)
{
    default:
        Console.WriteLine("*** Invalid command line arguments.");
        return -1;
    case 0:
        break;
    case 1:
        mapFileName = args[0];
        break;
}

if (!File.Exists(mapFileName))
{
    Console.WriteLine("*** File not found: {0}", mapFileName);
    return -2;
}

// Configure the host
using var host = Host.CreateDefaultBuilder(args)
    .UseGrace(new InjectionScopeConfiguration
    {
        //Behaviors = {
        //    AllowInstanceAndFactoryToReturnNull = true,

        //},
        //AutoRegisterUnknown = true,

    })
    .ConfigureServices(s => s.AddKeyedSingleton<NamedService<IHeroesIndex>>("hots", (sp, k) => new() { Name = "howtz", Value = new HeroesIndex() }))
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering();
    })
    .Build();

// Start the host
var hotsIndex = host.Services.GetRequiredKeyedService<NamedService<IHeroesIndex>>("hots");

await host.StartAsync();

Console.WriteLine("Map file name is '{0}'.", mapFileName);
Console.WriteLine("Setting up Adventure, please wait ...");

// Initialize the game world
var client = host.Services.GetRequiredService<IGrainFactory>();
var adventure = new AdventureGame(client);
await adventure.Configure(mapFileName);

Console.WriteLine("Setup completed.");
Console.WriteLine("Now you can launch the client.");

// Exit when any key is pressed
Console.WriteLine("Press any key to exit.");
Console.ReadKey();
await host.StopAsync();

return 0;



public class NamedService<T>
{
    public string Name { get; set; }
    public T Value { get; set; }
}

public interface IHeroesIndex
{
}

public class HeroesIndex : IHeroesIndex
{

}
public class StrategyIndex
{

}
