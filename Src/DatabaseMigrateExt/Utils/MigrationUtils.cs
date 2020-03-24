using FluentMigrator;

namespace DatabaseMigrateExt.Utils
{
    public static class MigrationUtils
    {
        public static void ExecuteDataStructureRefScript(this Migration migration, string scriptFileName)
        {
            var appContext = (MigrateDatabaseContext)migration.ApplicationContext;
            var embeddedScriptNamespace = $"{appContext.SqlDataStructureRefScriptNamespace}.{scriptFileName.Trim()}";
            migration.Execute.EmbeddedScript(embeddedScriptNamespace);
        }

        public static void ExecuteFunctionRefScript(this Migration migration, string scriptFileName)
        {
            var appContext = (MigrateDatabaseContext)migration.ApplicationContext;
            var embeddedScriptNamespace = $"{appContext.SqlFunctionRefScriptNamespace}.{scriptFileName.Trim()}";
            migration.Execute.EmbeddedScript(embeddedScriptNamespace);
        }

        public static void ExecuteStoredProcedureRefScript(this Migration migration, string scriptFileName)
        {
            var appContext = (MigrateDatabaseContext)migration.ApplicationContext;
            var embeddedScriptNamespace = $"{appContext.SqlStoredRefScriptNamespace}.{scriptFileName.Trim()}";
            migration.Execute.EmbeddedScript(embeddedScriptNamespace);
        }

        public static void ExecuteGeneralRefScript(this Migration migration, string scriptFileName)
        {
            var appContext = (MigrateDatabaseContext)migration.ApplicationContext;
            var embeddedScriptNamespace = $"{appContext.SqlGeneralScriptRefScriptNamespace}.{scriptFileName.Trim()}";
            migration.Execute.EmbeddedScript(embeddedScriptNamespace);
        }
    }
}
