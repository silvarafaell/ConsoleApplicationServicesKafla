using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var build = new ConfigurationBuilder();

            build.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(build.Build())
                .WriteTo.Console()
                .CreateLogger();

            //Log.Logger.Information("Information");
            //Log.Logger.Warning("Warning");
            //Log.Logger.Error("Error");
            //Log.Logger.Fatal("Fatal");

            //Console.ReadLine();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(opts =>
                {
                    //Log.Logger.Information($"{opts.host}");
                    //Log.Logger.Information($"{opts.requisicoes}");

                    var app = Host.CreateDefaultBuilder()
                    .ConfigureServices((context, services) =>
                    {
                        services.AddHostedService<Worker>();
                        services.AddTransient<IProcesso, Processo>();
                    })
                    .UseSerilog()
                    .Build();

                    app.Run();
                });

            //Log.Logger.Information("Fim da aplicação");
        }
    }
}
