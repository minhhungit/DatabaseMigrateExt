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
                //var runner = new ExtMigrationRunner();
                //runner.ForDatabases(new SortedList<int, string> {{2, "MovieStore" }, {1, "InventoryDb" } });
                //runner.ForRootNamespace("DatabaseMigrateRunner.Migrations");
                //runner.ForDatabaseLayers(new SortedList<int, DatabaseScriptType>
                //{
                //    {2, DatabaseScriptType.SqlFunction},
                //    {3, DatabaseScriptType.SqlStoredProcedure},
                //    {1, DatabaseScriptType.SqlDataAndStructure}
                //});
                //runner.ForMigrationAssembly(typeof(Program).Assembly);
                //runner.Process();

                ExtMigrationRunner.Initialize().Process();

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