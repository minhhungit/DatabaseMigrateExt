using Common.Logging;
using DatabaseMigrateExt;
using System;
using System.Collections.Generic;

namespace DatabaseMigrateRunner
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ExtMigrationRunner));

        private static void Main()
        {
            Logger.Info(@"
  ____        _        _                    __  __ _                 _             
 |  _ \  __ _| |_ __ _| |__   __ _ ___  ___|  \/  (_) __ _ _ __ __ _| |_ ___  _ __ 
 | | | |/ _` | __/ _` | '_ \ / _` / __|/ _ \ |\/| | |/ _` | '__/ _` | __/ _ \| '__|
 | |_| | (_| | || (_| | |_) | (_| \__ \  __/ |  | | | (_| | | | (_| | || (_) | |   
 |____/ \__,_|\__\__,_|_.__/ \__,_|___/\___|_|  |_|_|\__, |_|  \__,_|\__\___/|_|   
                                                     |___/                         ");
            try
            {
                #region Run migrate with default values

                //ExtMigrationRunner.Initialize().Process();
                //Console.ReadKey();

                #endregion

                #region Run migrate with custom values

                //var runner = new ExtMigrationRunner();
                //runner.ForDatabases(new SortedList<int, string> { { 2, "MovieStore" }, { 1, "InventoryDb" } });
                //runner.ForRootNamespace("DatabaseMigrateRunner.Migrations");
                //runner.ForDatabaseLayers(new SortedList<int, DatabaseScriptType>
                //{
                //    {2, DatabaseScriptType.SqlFunction},
                //    {3, DatabaseScriptType.SqlStoredProcedure},
                //    {1, DatabaseScriptType.SqlDataAndStructure}
                //});
                //runner.ForMigrationAssembly(typeof(Program).Assembly);
                //runner.Process();
                //Console.ReadKey();

                #endregion

                #region Run migrate with some tricks

                var runner = ExtMigrationRunner.Initialize();

                while (true)
                {
                    Logger.Info("Do you really want to run <type 'yes' to run>: ");
                    var result = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        Logger.Info("Your answer: " + result);
                        Logger.Info("");
                        Logger.Info("");
                        switch (result.ToLower().Trim())
                        {
                            case "ok":
                            case "yes":
                                runner.Process(true);
                                Logger.Info("Completed!");
                                break;
                            case "exit":
                            case "quit":
                            case "close":
                                Logger.Info("\n---------------------------------");
                                Logger.Info("System is closing, please wait...");
                                return;
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                Logger.Info("Something happenned: " + ex.Message);
                Console.ReadKey();
            }
        }
    }
}