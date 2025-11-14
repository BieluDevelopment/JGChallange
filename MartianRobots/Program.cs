// See https://aka.ms/new-console-template for more information


using MartianRobots.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; // Requires NuGet package

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<ConsoleController>();
        services.AddTransient<IConsoleProvider, ConsoleProvider>();
        services.AddTransient<IWorldService, WorldService>();
        services.AddTransient<IRobotService, RobotService>();
        services.AddTransient<IRobotNavigationService, RobotNavigationService>();
    })
    .Build();

var appController = host.Services.GetRequiredService<ConsoleController>();
await appController.ExecuteAsync();
