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
                ExtMigrationRunner
                    .Initialize()
                    .Process();

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
