using DatabaseMigrateExt;
using DatabaseMigrateExt.Models;
using System;
using System.Configuration;
using System.Linq;

namespace DatabaseMigrateRunner
{
    class Program
    {
        private static void Main()
        {
            try
            {
                var databaseKeys = ConfigurationManager.AppSettings["mgr:DatabaseKeys"].Split(',').Select(p => p.Trim()).ToList();
                var setting = new MigrationSetting(databaseKeys);
                
                MigrationManager.Run(setting);
                Console.WriteLine("Completed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something happenned: " + ex.Message);
            }

            Console.ReadKey();
        }
    }
}
