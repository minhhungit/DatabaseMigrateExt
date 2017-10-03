using DatabaseMigrateExt;
using System;
using DatabaseMigrateExt.Models;

namespace DatabaseMigrateRunner
{
    class Program
    {
        private static void Main()
        {
            try
            {
                var setting = new MigrationSetting();
                MigrationManager.Run();
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
