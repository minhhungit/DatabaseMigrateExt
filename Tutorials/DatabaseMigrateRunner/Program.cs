using DatabaseMigrateExt;
using System;

namespace DatabaseMigrateRunner
{
    class Program
    {
        private static void Main()
        {
            try
            {
                var migrator = new MigrationManager();
                // migrator.Settings = [...]

                migrator.Run();
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
