using FluentMigrator;

namespace DatabaseMigrateExt
{
    public static class MigrationUtils
    {
        public static void ExecuteSqlStructure(this Migration migration, string scriptFileName)
        {
            var appContext = (MigrateDatabaseItem)migration.ApplicationContext;
            var embeddedScriptNamespace = $"{appContext.SqlArchitectureRefScriptNamespace}.{scriptFileName.Trim()}";
            migration.Execute.EmbeddedScript(embeddedScriptNamespace);
        }

        public static void ExecuteStoredProcedure(this Migration migration, string scriptFileName)
        {
            var appContext = (MigrateDatabaseItem)migration.ApplicationContext;
            var embeddedScriptNamespace = $"{appContext.SqlStoredRefScriptNamespace}.{scriptFileName.Trim()}";
            migration.Execute.EmbeddedScript(embeddedScriptNamespace);
        }

        public static void ExecuteFunction(this Migration migration, string scriptFileName)
        {
            var appContext = (MigrateDatabaseItem)migration.ApplicationContext;
            var embeddedScriptNamespace = $"{appContext.SqlFunctionRefScriptNamespace}.{scriptFileName.Trim()}";
            migration.Execute.EmbeddedScript(embeddedScriptNamespace);
        }
    }
}
