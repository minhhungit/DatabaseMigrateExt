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
