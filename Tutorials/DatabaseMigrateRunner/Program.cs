using Common.Logging;
using DatabaseMigrateExt;
using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;

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

                #endregion

                #region Run migrate with custom values

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

                #endregion

                #region Run migrate with some tricks

                var runner = ExtMigrationRunner.Initialize();
                var runnerCtx = runner.GetRunnerContext();
                ShowValidateScripts(runnerCtx);

                while (true)
                {
                    Logger.Info("Do you really want to run <type 'yes' to run>: ");
                    var result = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        Logger.Info("Your answer: " + result);

                        switch (result.ToLower().Trim())
                        {
                            case "ok":
                            case "yes":
                                runner.Process();
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
            }
            catch (Exception ex)
            {
                Logger.Info("Something happenned: " + ex.Message);
                Console.ReadKey();
            }

            #endregion
        }

        private static void ShowValidateScripts(ExtMigrationRunnerContext runnerCtx)
        {
            // warning invalid scripts
            var invalidScripts = new List<KeyValuePair<Type, string>>();

            var allScriptFiles = runnerCtx.MigrationAssembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(MigrationBase))).ToList();

            var validNamespaces = runnerCtx.DatabaseKeys.Select(x => $"{runnerCtx.RootNamespace}.{x.Value}").ToList();
            var scriptVersions = new List<long>();
            foreach (var script in allScriptFiles)
            {
                if (!script.IsPublic)
                {
                    invalidScripts.Add(new KeyValuePair<Type, string>(script, "[NOT PUBLIC]"));
                    continue;
                }

                var attrs = script.GetCustomAttributes(typeof(BaseExtMgrAttribute), false).ToList();
                var attrFirst = attrs.FirstOrDefault();
                if (attrFirst is BaseExtMgrAttribute)
                {
                    if (scriptVersions.Contains(((BaseExtMgrAttribute)attrFirst).Version))
                    {
                        invalidScripts.Add(new KeyValuePair<Type, string>(script, "[DUPLICATE VERSION]"));
                        continue;
                    }
                    scriptVersions.Add(((BaseExtMgrAttribute)attrFirst).Version);
                }
                else
                {
                    invalidScripts.Add(new KeyValuePair<Type, string>(script, "[INCORRECT ATTRIBUTE]"));
                    continue;
                }

                // check namespace by database
                if (!validNamespaces.Contains(script.Namespace))
                {
                    invalidScripts.Add(new KeyValuePair<Type, string>(script, "[INCORRECT NAMESPACE]"));
                    continue;
                }
            }

            if (invalidScripts.Any())
            {
                Logger.Info($"There are some invalid scripts:");
                foreach (var item in invalidScripts.OrderBy(x => x.Key.Name))
                {
                    Logger.Info($"  > {item.Key.Name} {item.Value}");
                }

                Logger.Info("");
            }
        }
    }
}