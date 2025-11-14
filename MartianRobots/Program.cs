// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; // Requires NuGet package

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddTransient<MyService>(); })
    .Build();

var my = host.Services.GetRequiredService<MyService>();
await my.ExecuteAsync();
