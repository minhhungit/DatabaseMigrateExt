using DatabaseMigrateExt.Models;
using FluentMigrator;

namespace DatabaseMigrateExt.Utils
{
    public static class MigrationUtils
    {
        public static void ExecuteSqlStructure(this Migration migration, string scriptFileName)
        {
            var appContext = (MigrateDatabaseContext)migration.ApplicationContext;
            var embeddedScriptNamespace = $"{appContext.SqlArchitectureRefScriptNamespace}.{scriptFileName.Trim()}";
            migration.Execute.EmbeddedScript(embeddedScriptNamespace);
        }

        public static void ExecuteStoredProcedure(this Migration migration, string scriptFileName)
        {
            var appContext = (MigrateDatabaseContext)migration.ApplicationContext;
            var embeddedScriptNamespace = $"{appContext.SqlStoredRefScriptNamespace}.{scriptFileName.Trim()}";
            migration.Execute.EmbeddedScript(embeddedScriptNamespace);
        }

        public static void ExecuteFunction(this Migration migration, string scriptFileName)
        {
            var appContext = (MigrateDatabaseContext)migration.ApplicationContext;
            var embeddedScriptNamespace = $"{appContext.SqlFunctionRefScriptNamespace}.{scriptFileName.Trim()}";
            migration.Execute.EmbeddedScript(embeddedScriptNamespace);
        }
    }
}
