using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using Common.Logging;
using DatabaseMigrateExt.Utils;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;

namespace DatabaseMigrateExt
{
    public static class ExtMigrationRunnerExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ExtMigrationRunner));

        public static ExtMigrationRunner ForRootNamespace(this ExtMigrationRunner runner, string rootNamespace)
        {
            if (string.IsNullOrWhiteSpace(rootNamespace))
            {
                throw new ArgumentException("<rootNamespace> should not null or empty");
            }

            runner.RootNamespace = rootNamespace;
            return runner;
        }
        
        public static ExtMigrationRunner ForDatabases(this ExtMigrationRunner runner, SortedList<int, string> databaseKeys)
        {
            if (databaseKeys == null)
            {
                throw new ArgumentException("<databaseKeys> should not null");
            }

            runner.DatabaseKeys = new SortedList<int, string>();

            if (databaseKeys.Any())
            {
                foreach (var item in databaseKeys)
                {
                    runner.DatabaseKeys.Add(item.Key, item.Value);
                }
                
            }

            return runner;
        }


        public static ExtMigrationRunner ForDatabaseLayers(this ExtMigrationRunner runner, SortedList<int, DatabaseScriptType> layers)
        {
            if (layers == null)
            {
                throw new ArgumentException("<layers> should not null");
            }

            runner.DatabaseLayers = new SortedList<int, DatabaseScriptType>();

            if (layers.Any())
            {
                foreach (var item in layers)
                {
                    runner.DatabaseLayers.Add(item.Key, item.Value);
                }
            }
            
            return runner;
        }

        public static ExtMigrationRunner ForMigrationAssembly(this ExtMigrationRunner runner, Assembly migrationAssembly)
        {
            runner.MigrationAssembly = migrationAssembly ?? throw new ArgumentException("<migrationAssembly> should not null");
            return runner;
        }

        /// <summary>
        /// Run migration for all databases
        /// </summary>
        /// <param name="runner"></param>
        public static void Process(this ExtMigrationRunner runner)
        {
            #region Check settings

            if (string.IsNullOrWhiteSpace(runner.RootNamespace))
            {
                throw new ArgumentNullException(nameof(runner.RootNamespace), "Value should not null or empty");
            }

            if (runner.DatabaseKeys == null)
            {
                throw new ArgumentNullException(nameof(runner.DatabaseKeys), "Value should not null");
            }

            #endregion
            
            var migrationDatabases = runner.DatabaseKeys.Select(dbKey => new MigrateDatabaseContext(runner.RootNamespace, dbKey.Value)).ToList();

            foreach (var dbContext in migrationDatabases)
            {
                Process(runner, dbContext);
            }
        }

        /// <summary>
        /// Run migration for specific database
        /// </summary>
        /// <param name="runner"></param>
        /// <param name="dbContext"></param>
        public static void Process(this ExtMigrationRunner runner, MigrateDatabaseContext dbContext)
        {
            Logger.InfoFormat($"DATEBASE: {dbContext.DatabaseKey} ({dbContext.DatabaseName})");
            #region Check settings

            if (runner.DatabaseLayers == null)
            {
                throw new ArgumentNullException(nameof(runner.DatabaseLayers), "Value should not null");
            }

            if (runner.MigrationAssembly == null)
            {
                throw new ArgumentNullException(nameof(runner.MigrationAssembly), "Value should not null");
            }

            #endregion

            try
            {
                foreach (var scriptType in runner.DatabaseLayers)
                {
                    ValidateAndRunMigrations(dbContext, scriptType.Value, runner.MigrationAssembly);
                }

                Logger.InfoFormat($"=> [{dbContext.DatabaseKey}] is done");
                Logger.InfoFormat(Environment.NewLine);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(string.Empty, ex);
                throw;
            }
        }

        private static void ValidateAndRunMigrations(MigrateDatabaseContext dbContext, DatabaseScriptType scriptType, Assembly migrationAssembly)
        {
            using (var sw = new StringWriter())
            {
                var migRunner = GetMigrationRunner(sw, dbContext, migrationAssembly);
                var migrations = migRunner.MigrationLoader.LoadMigrations();

                var printAtStart = false;
                foreach (var script in migrations)
                {
                    if (!script.Value.Version.ToString().StartsWith(((int)scriptType).ToString()))
                    {
                        continue;
                    }

                    var migrateAttr =
                        script.Value.Migration.GetType()
                            .GetCustomAttributes(typeof(BaseExtMgrAttribute), false)
                            .FirstOrDefault();

                    if (migrateAttr == null || migRunner.VersionLoader.VersionInfo.HasAppliedMigration(script.Value.Version))
                    {
                        continue;
                    }

                    switch (scriptType)
                    {
                        case DatabaseScriptType.SqlDataAndStructure:
                            if (!(migrateAttr is ExtMgrDataStructureAttribute))
                            {
                                continue;
                            }
                            break;
                        case DatabaseScriptType.SqlFunction:
                            if (!(migrateAttr is ExtMgrFunctionAttribute))
                            {
                                continue;
                            }
                            break;
                        case DatabaseScriptType.SqlStoredProcedure:
                            if (!(migrateAttr is ExtMgrStoredProcedureAttribute))
                            {
                                continue;
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(scriptType), scriptType, null);
                    }

                    if (!printAtStart)
                    {
                        Logger.InfoFormat($" > {scriptType.GetEnumDescription()}");
                        printAtStart = true;
                    }

                    Logger.InfoFormat($"   - {script.Value.Version} - {script.Value.Migration.GetType().Name} {(!((BaseExtMgrAttribute)migrateAttr).UseTransaction ? " -noTrans" : string.Empty)}");

                    migRunner.ApplyMigrationUp(script.Value, ((BaseExtMgrAttribute)migrateAttr).UseTransaction);
                }
            }
        }

        private static MigrationRunner GetMigrationRunner(StringWriter sw, MigrateDatabaseContext dbItem, Assembly migrationAssembly)
        {
            Announcer announcer = new TextWriterWithGoAnnouncer(sw) { ShowSql = true };

            var runnerCtx = new RunnerContext(announcer)
            {
                ApplicationContext = dbItem
            };

            if (string.IsNullOrWhiteSpace(dbItem.CurrentDatabaseNamespace))
            {
                throw new ArgumentOutOfRangeException(nameof(dbItem), dbItem, null);
            }

            runnerCtx.Namespace = dbItem.CurrentDatabaseNamespace;

            var options = new ProcessorOptions { PreviewOnly = false, Timeout = dbItem.ConnectionTimeout };
            var factory = new FluentMigrator.Runner.Processors.SqlServer.SqlServer2014ProcessorFactory();

            using (var processor = factory.Create(dbItem.ConnectionString, announcer, options))
            {
                return new MigrationRunner(migrationAssembly, runnerCtx, processor);
            }
        }
    }
}
