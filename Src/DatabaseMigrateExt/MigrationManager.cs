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

namespace DatabaseMigrateExt
{
    public class MigrationManager
    {
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
            var migrationDatabaseItems = setting.DatabaseKeys.Select(dbKey => new MigrateDatabaseItem(dbKey)).ToList();

            foreach (var item in migrationDatabaseItems)
            {
                Run(setting, item);
            }            
        }

        /// <summary>
        /// Run migration for a specific database
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="item"></param>
        public void Run(MigrationSetting setting, MigrateDatabaseItem item)
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
        }

        private static void ValidateAndRunMigrations(MigrationRunner runner, SortedList<long, IMigrationInfo> migrations, DatabaseScriptType scriptType)
        {
            foreach (var script in migrations)
            {
                var attrs = script.Value.Migration.GetType().GetCustomAttributes(typeof(Attributes.ExtMigrationAttribute), false);
                if (attrs.Length <= 0) continue;

                var migrateAttr = (Attributes.ExtMigrationAttribute)attrs[0];
                if (migrateAttr.ScriptType == scriptType)
                {
                    runner.ApplyMigrationUp(script.Value, true);
                }
            }
        }

        private MigrationRunner GetMigrationRunner(Assembly migrationAssembly, StringWriter sw, MigrateDatabaseItem dbItem)
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
