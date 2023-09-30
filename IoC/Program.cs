// See https://aka.ms/new-console-template for more information
using Console1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, DI!");

var host = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
{
    services.AddSingleton<DependA>();
    services.AddSingleton<IDependB, DependB>();
    services.AddHostedService<HostedService>();
}).Build();


await host.RunAsync();