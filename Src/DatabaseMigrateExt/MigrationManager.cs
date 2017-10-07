using FluentMigrator.Infrastructure;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Logging;
using DatabaseMigrateExt.Models;
using DatabaseMigrateExt.Utils;

namespace DatabaseMigrateExt
{
    public class MigrationManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MigrationManager));
        private static readonly Assembly DefaultMigrationAssembly = GetDefaultMigrationAssembly();
        public MigrationSetting Settings { get; set; }

        public MigrationManager()
        {
            Settings = new MigrationSetting();
        }

        public MigrationManager(MigrationSetting setting)
        {
            Settings = setting;
        }

        /// <summary>
        /// Run migration for all database
        /// </summary>
        public void Run()
        {
            Logger.InfoFormat($"Start...{Environment.NewLine}");

            var migrationDatabases = Settings.DatabaseKeys.Select(dbKey => new MigrateDatabaseContext(Settings.RootNamespace, dbKey)).ToList();

            foreach (var dbContext in migrationDatabases)
            {
                Logger.InfoFormat($"DATEBASE: {dbContext.DatabaseKey} ({dbContext.DatabaseName})");
                Run(dbContext);
            }

            Logger.InfoFormat("All done!");
        }

        /// <summary>
        /// Run migration for specific database
        /// </summary>
        /// <param name="dbContext"></param>
        public void Run(MigrateDatabaseContext dbContext)
        {
            try
            {
                using (var sw = new StringWriter())
                {
                    var runner = GetMigrationRunner(sw, dbContext);
                    var migrations = runner.MigrationLoader.LoadMigrations();

                    foreach (var scriptType in Settings.AvailableLevels)
                    {
                        ValidateAndRunMigrations(runner, migrations, scriptType);
                    }
                }
                Logger.InfoFormat("   -> done");
                Logger.InfoFormat(Environment.NewLine);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(string.Empty, ex);
                throw;
            }
        }

        private void ValidateAndRunMigrations(MigrationRunner runner, SortedList<long, IMigrationInfo> migrations, DatabaseScriptType scriptType)
        {
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

                if (migrateAttr == null  ||runner.VersionLoader.VersionInfo.HasAppliedMigration(script.Value.Version))
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

                Logger.InfoFormat($"   - Ver: {script.Value.Version} - {script.Value.Migration.GetType().Name} {(!((BaseExtMgrAttribute)migrateAttr).UseTransaction ? " -noTrans" : string.Empty)}");

                runner.ApplyMigrationUp(script.Value, ((BaseExtMgrAttribute)migrateAttr).UseTransaction);
            }
        }

        private MigrationRunner GetMigrationRunner(StringWriter sw, MigrateDatabaseContext dbItem)
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
                return new MigrationRunner(
                    Settings.MigrationAssembly != null
                        ? Settings.MigrationAssembly
                        : DefaultMigrationAssembly
                    , runnerCtx, processor);
            }
        }

        private static Assembly GetDefaultMigrationAssembly()
        {
            var usedAttrAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assy => assy != typeof(MigrationManager).Assembly)
                .FirstOrDefault(assy => assy.GetTypes()
                    .Select(type => Attribute.IsDefined(type, typeof(BaseExtMgrAttribute)))
                    .Any(x => x));

            if (usedAttrAssemblies != null)
            {
                return usedAttrAssemblies;
            }
            throw new TypeLoadException("Can not load migration assembly");
        }
    }
}
