using FluentMigrator;
using System.Reflection;

namespace DatabaseMigrateExt
{
    public static class Utils
    {
        public static void ExecuteSqlStructure(this Migration migration, string scriptFileName)
        {
            var appContext = (MigrateDatabaseItem)migration.ApplicationContext;

            var assembly = Assembly.GetExecutingAssembly();
            var embeddedScriptNamespace = $"{appContext.SqlArchitectureRefScriptNamespace}.{scriptFileName.Trim()}";
            migration.Execute.EmbeddedScript(embeddedScriptNamespace);
        }

        public static void ExecuteStoredProcedure(this Migration migration, string scriptFileName)
        {
            var appContext = (MigrateDatabaseItem)migration.ApplicationContext;

            var assembly = Assembly.GetExecutingAssembly();
            var embeddedScriptNamespace = $"{appContext.SqlStoredRefScriptNamespace}.{scriptFileName.Trim()}";
            migration.Execute.EmbeddedScript(embeddedScriptNamespace);

            //using (Stream stream = assembly.GetManifestResourceStream(embeddedScriptNamespace))
            //using (StreamReader reader = new StreamReader(stream))
            //{
            //    var content = reader.ReadToEnd();
                
            //}
        }

        public static void ExecuteFunction(this Migration migration, string scriptFileName)
        {
            var appContext = (MigrateDatabaseItem)migration.ApplicationContext;

            var assembly = Assembly.GetExecutingAssembly();
            var embeddedScriptNamespace = $"{appContext.SqlFunctionRefScriptNamespace}.{scriptFileName.Trim()}";
            migration.Execute.EmbeddedScript(embeddedScriptNamespace);
        }
    }
}
