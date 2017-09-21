using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using System;
using System.Collections.Generic;
using System.IO;

namespace DatabaseMigrateExt
{
    public class MigrationManager
    {
        private static readonly Lazy<MigrationManager> lazy = new Lazy<MigrationManager>(() => new MigrationManager());

        public static MigrationManager Instance { get { return lazy.Value; } }

        private MigrationManager()
        {
        }

        public void Run(MigrationSetting setting, MigrateDatabaseItem item)
        {
            using (var sw = new StringWriter())
            {
                foreach (var scriptType in setting.AvailableLevels)
                {
                    ValidateAndRunMigrations(sw, item, scriptType);
                }              
            }
        }

        public void Run(MigrationSetting setting)
        {
            var migrationDatabaseItems = new List<MigrateDatabaseItem>();
            foreach (var databaseName in setting.DatabaseKeys)
            {
                migrationDatabaseItems.Add(MigrateDatabaseItem.CreateDatabaseItem(databaseName));
            }

            foreach (var item in migrationDatabaseItems)
            {
                Run(setting, item);
            }            
        }

        private void ValidateAndRunMigrations(StringWriter sw, MigrateDatabaseItem item, DatabaseScriptType scriptType)
        {
            var runner = GetMigrationRunner(sw, item, scriptType);

            foreach (var script in runner.MigrationLoader.LoadMigrations())
            {
                if (!script.Key.ToString().StartsWith(((int)scriptType).ToString()) || script.Key.ToString().Length < 18)
                {
                    throw new Exception($"You used wrong migration attribute or you put migration attribute wrong place: {scriptType}. {Environment.NewLine}{script.Value.ToString()}");
                }
            }
            runner.MigrateUp(true);
        }

        private MigrationRunner GetMigrationRunner(StringWriter sw, MigrateDatabaseItem dbItem, DatabaseScriptType scriptType)
        {
            Announcer announcer = new TextWriterWithGoAnnouncer(sw) { ShowSql = true };

            var runnerCtx = new RunnerContext(announcer)
            {
                ApplicationContext = dbItem
            };

            switch (scriptType)
            {
                case DatabaseScriptType.SqlDataAndStructure:
                    runnerCtx.Namespace = dbItem.SqlArchitectureChangeScriptNamespace;
                    break;                
                case DatabaseScriptType.SqlFunction:
                    runnerCtx.Namespace = dbItem.SqlFunctionChangeScriptNamespace;
                    break;
                case DatabaseScriptType.SqlStoredProcedure:
                    runnerCtx.Namespace = dbItem.SqlStoredChangeScriptNamespace;
                    break;
                default:
                    break;
            }

            var options = new ProcessorOptions { PreviewOnly = false, Timeout = dbItem.ConnectionTimeout };
            var factory = new FluentMigrator.Runner.Processors.SqlServer.SqlServer2014ProcessorFactory();

            using (var processor = factory.Create(dbItem.ConnectionString, announcer, options))
            {
                return new MigrationRunner(System.Reflection.Assembly.GetExecutingAssembly(), runnerCtx, processor);
            }
        }
    }
}
