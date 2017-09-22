using DatabaseMigrateExt;
using System;
using System.Configuration;
using System.Linq;

namespace DatabaseMigrateRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var databaseKeys = ConfigurationManager.AppSettings["mgr:DatabaseKeys"].Split(',').Select(p => p.Trim()).ToList();

            var setting = new MigrationSetting
            {
                DatabaseKeys = databaseKeys
            };

            // Start
            MigrationManager.Instance.Run(setting);

            //Completed!
            Console.ReadKey();
        }
    }
}
