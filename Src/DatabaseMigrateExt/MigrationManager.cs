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
        private static readonly Lazy<MigrationManager> Lazy = new Lazy<MigrationManager>(() => new MigrationManager());

        public static MigrationManager Instance => Lazy.Value;

        private MigrationManager()
        {
        }

        /// <summary>
        /// Run migration for all database
        /// </summary>
        /// <param name="setting"></param>
        public void Run(MigrationSetting setting)
        {
            Logger.InfoFormat($"Start...{Environment.NewLine}");

            var migrationDatabases = setting.DatabaseKeys.Select(dbKey => new MigrateDatabaseContext(dbKey)).ToList();

            foreach (var item in migrationDatabases)
            {
                Logger.InfoFormat($"DATEBASE: {item.DatabaseKey} ({item.DatabaseName})");
                Run(setting, item);
            } 
            
            Logger.InfoFormat("All done!");
        }

        /// <summary>
        /// Run migration for a specific database
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="item"></param>
        public void Run(MigrationSetting setting, MigrateDatabaseContext item)
        {
            try
            {
                using (var sw = new StringWriter())
                {
                    var runner = GetMigrationRunner(setting.MigrationAssembly, sw, item);
                    var migrations = runner.MigrationLoader.LoadMigrations();

                    foreach (var scriptType in setting.AvailableLevels)
                    {
                        ValidateAndRunMigrations(runner, migrations, scriptType);
                    }
                }
                Logger.InfoFormat(" -> done");
                Logger.InfoFormat(Environment.NewLine);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat(string.Empty, ex);
                throw;
            }
        }

        private static void ValidateAndRunMigrations(MigrationRunner runner, SortedList<long, IMigrationInfo> migrations, DatabaseScriptType scriptType)
        {
            var printAtStart = false;
            foreach (var script in migrations)
            {
                var migrateAttr =
                    (Attributes.ExtMigrationAttribute)
                    script.Value.Migration.GetType()
                        .GetCustomAttributes(typeof(Attributes.ExtMigrationAttribute), false)
                        .FirstOrDefault();

                if (migrateAttr == null || migrateAttr.ScriptType != scriptType || runner.VersionLoader.VersionInfo.HasAppliedMigration(script.Value.Version))
                {
                    continue;
                }

                if (!printAtStart)
                {
                    Logger.InfoFormat($" > {scriptType.GetEnumDescription()}");
                    printAtStart = true;
                }

                Logger.InfoFormat($"   - Ver: {script.Value.Version} - {script.Value.Migration.GetType().Name} {(!migrateAttr.UseTransaction ? " -noTrans" : string.Empty)}");
                runner.ApplyMigrationUp(script.Value, migrateAttr.UseTransaction);
            }
        }

        private static MigrationRunner GetMigrationRunner(Assembly migrationAssembly, StringWriter sw, MigrateDatabaseContext dbItem)
        {
            Announcer announcer = new TextWriterWithGoAnnouncer(sw) { ShowSql = true };

            var runnerCtx = new RunnerContext(announcer)
            {
                ApplicationContext = dbItem
            };

            if (string.IsNullOrWhiteSpace(dbItem.CurrentDatabsaeNamespace))
            {
                throw new ArgumentOutOfRangeException(nameof(dbItem), dbItem, null);
            }

            runnerCtx.Namespace = dbItem.CurrentDatabsaeNamespace;

            var options = new ProcessorOptions { PreviewOnly = false, Timeout = dbItem.ConnectionTimeout };
            var factory = new FluentMigrator.Runner.Processors.SqlServer.SqlServer2014ProcessorFactory();

            using (var processor = factory.Create(dbItem.ConnectionString, announcer, options))
            {
                return new MigrationRunner(migrationAssembly, runnerCtx, processor);
            }
        }
    }
}
