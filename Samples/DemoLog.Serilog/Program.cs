using Common.Logging;
using Common.Logging.Configuration;
using DatabaseMigrateExt;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace DemoLog.Serilog
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            // Common logging
            LogConfiguration logConfiguration = new LogConfiguration();
            config.GetSection("LogConfiguration").Bind(logConfiguration);
            LogManager.Configure(logConfiguration);

            // Serilog
            var log = new LoggerConfiguration()
                 .ReadFrom.Configuration(config) // Serilog.Settings.Configuration
                .WriteTo.ColoredConsole()
                //.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day) // reading from appsetting.json
                .CreateLogger();

            Log.Logger = log;
            
            ExtMigrationRunner.Initialize().Process();

            log.Error("Test error message");
            log.Information("test information message");

            log.Information("===================================================================");
        }
    }
}
