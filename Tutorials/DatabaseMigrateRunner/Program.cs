using DatabaseMigrateExt;
using System;
using System.Configuration;
using System.Linq;

namespace DatabaseMigrateRunner
{
    class Program
    {
        private static void Main()
        {
            var databaseKeys = ConfigurationManager.AppSettings["mgr:DatabaseKeys"].Split(',').Select(p => p.Trim()).ToList();

            var setting = new MigrationSetting(databaseKeys);

            Console.WriteLine("Start...");
            MigrationManager.Instance.Run(setting);

            Console.WriteLine("Completed!");
            Console.ReadKey();
        }
    }
}
