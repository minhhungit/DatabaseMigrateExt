using DatabaseMigrateExt;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace DatabaseMigrateRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var databaseKeys = ConfigurationManager.AppSettings["mgr:DatabaseKeys"].Split(',').Select(p => p.Trim()).ToList();
            var availableLevels = ConfigurationManager.AppSettings["mgr:AvailableLevels"].Split(',').Select(x => (DatabaseScriptType)int.Parse(x)).ToList();

            var setting = new MigrationSetting
            {
                DatabaseKeys = databaseKeys,
                AvailableLevels = availableLevels,
                MigrationAssembly = Assembly.GetExecutingAssembly()
            };

            Console.WriteLine("Start...");
            MigrationManager.Instance.Run(setting);
            Console.WriteLine("Completed!");

            Console.ReadKey();
        }
    }
}
