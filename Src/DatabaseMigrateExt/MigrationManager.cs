﻿using FluentMigrator.Infrastructure;
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
        private static Assembly _callingAssembly;
        /// <summary>
        ///  Run migration for all database and default settings
        /// </summary>
        public static void Run()
        {
            _callingAssembly = Assembly.GetCallingAssembly();
            Run(new MigrationSetting());
        }

        /// <summary>
        /// Run migration for all database and a specific migration direction (Up or Down)
        /// </summary>
        /// <param name="setting"></param>
        public static void Run(MigrationSetting setting)
        {
            Logger.InfoFormat($"Start...{Environment.NewLine}");

            var migrationDatabases = setting.DatabaseKeys.Select(dbKey => new MigrateDatabaseContext(dbKey)).ToList();

            foreach (var dbContext in migrationDatabases)
            {
                Logger.InfoFormat($"DATEBASE: {dbContext.DatabaseKey} ({dbContext.DatabaseName})");
                Run(setting, dbContext);
            }

            Logger.InfoFormat("All done!");
        }

        /// <summary>
        /// Run migration for a specific database and a specific migration direction
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="dbContext"></param>
        public static void Run(MigrationSetting setting, MigrateDatabaseContext dbContext)
        {
            try
            {
                using (var sw = new StringWriter())
                {
                    var assemblyLocation = setting.MigrationAssembly;
                    if (assemblyLocation.GetName().Name == "DatabaseMigrateExt")
                    {
                        assemblyLocation = _callingAssembly;
                    }

                    if (assemblyLocation == null)
                    {
                        throw new ArgumentOutOfRangeException(nameof(assemblyLocation), assemblyLocation, null);
                    }

                    var runner = GetMigrationRunner(assemblyLocation, sw, dbContext);
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
