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

        /// <summary>
        /// Run migration for all database for migration Up direction
        /// </summary>
        /// <param name="setting"></param>
        public static void Run(MigrationSetting setting)
        {
            Run(setting, MigrationDirection.Up);
        }

        /// <summary>
        /// Run migration for all database and a specific migration direction (Up or Down)
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="migrationDirection"></param>
        public static void Run(MigrationSetting setting, MigrationDirection migrationDirection)
        {
            Logger.InfoFormat($"Start...{Environment.NewLine}");

            var migrationDatabases = setting.DatabaseKeys.Select(dbKey => new MigrateDatabaseContext(dbKey)).ToList();

            foreach (var dbContext in migrationDatabases)
            {
                Logger.InfoFormat($"DATEBASE: {dbContext.DatabaseKey} ({dbContext.DatabaseName})");
                Run(setting, dbContext, migrationDirection);
            }

            Logger.InfoFormat("All done!");
        }

        /// <summary>
        /// Run migration for a specific database for migration Up direction
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="dbContext"></param>
        public static void Run(MigrationSetting setting, MigrateDatabaseContext dbContext)
        {
            Run(setting, dbContext, MigrationDirection.Up);
        }

        /// <summary>
        /// Run migration for a specific database and a specific migration direction
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="dbContext"></param>
        /// <param name="migrationDirection">Migration Direction (Up or Down)</param>
        public static void Run(MigrationSetting setting, MigrateDatabaseContext dbContext, MigrationDirection migrationDirection)
        {
            try
            {
                using (var sw = new StringWriter())
                {
                    var runner = GetMigrationRunner(setting.MigrationAssembly, sw, dbContext);
                    var migrations = runner.MigrationLoader.LoadMigrations();

                    foreach (var scriptType in setting.AvailableLevels)
                    {
                        ValidateAndRunMigrations(runner, migrations, scriptType, migrationDirection);
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

        private static void ValidateAndRunMigrations(MigrationRunner runner, SortedList<long, IMigrationInfo> migrations, DatabaseScriptType scriptType, MigrationDirection migrationDirection)
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

                if (migrationDirection == MigrationDirection.Up)
                {
                    runner.ApplyMigrationUp(script.Value, migrateAttr.UseTransaction);
                }
                else
                {
                    runner.ApplyMigrationDown(script.Value, migrateAttr.UseTransaction);
                }
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
