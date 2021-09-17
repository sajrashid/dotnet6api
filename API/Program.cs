using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Debugging;

namespace API
{
    public class Program
    {
        private const string OutputTemplate =
            "{Timestamp:dd-MM-yyyy HH:mm:ss} [{Level:u3}] [{ThreadId}] {Message}{NewLine}{Exception}";
        public static void Main(string[] args)
        {
            SelfLog.Enable(Console.Error);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((ctx, cfg) => cfg.ClearProviders())
                .UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
             
    }
}
