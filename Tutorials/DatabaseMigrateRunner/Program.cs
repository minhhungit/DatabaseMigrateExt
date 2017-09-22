using DatabaseMigrateExt;
using System;
using System.Collections.Generic;

namespace DatabaseMigrateRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var databaseKeys = new List<string> { "MovieStore", "InventoryDb" };

            // OR load values from AppSetting
            // databaseKeys = ConfigurationManager.AppSettings["mgr:DatabaseKeys"].Split(',').Select(p => p.Trim()).ToList();

            var setting = new MigrationSetting
            {
                DatabaseKeys = databaseKeys
            };

            Console.WriteLine("Start...");
            MigrationManager.Instance.Run(setting);
            Console.WriteLine("Completed!");

            Console.ReadKey();
        }
    }
}
