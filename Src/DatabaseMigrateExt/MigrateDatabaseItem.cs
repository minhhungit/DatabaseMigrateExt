using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;

namespace DatabaseMigrateExt
{
    public class MigrateDatabaseItem
    {
        public static MigrateDatabaseItem CreateDatabaseItem(Assembly migrationAssembly, string databaseKey)
        {
            return new MigrateDatabaseItem
            {
                DatabaseKey = databaseKey,
                MigrationAssembly = migrationAssembly
            };
        }

        public string DatabaseKey { get; set; }
        public string RootNamespace => ConfigurationManager.AppSettings["mgr:RootNamespace"];
        public string ConnectionString => ConfigurationManager.AppSettings[$"mgr:{DatabaseKey}_ConnString"];
        public Assembly MigrationAssembly { get; set; }

        public int ConnectionTimeout
        {
            get
            {
                var builder = new SqlConnectionStringBuilder(ConnectionString);
                return builder.ConnectTimeout;
            }
        }

        public string SqlArchitectureRefScriptNamespace     => $"{RootNamespace}.{DatabaseKey}.SqlDataAndStructure._RefScript";
        public string SqlArchitectureChangeScriptNamespace  => $"{RootNamespace}.{DatabaseKey}.SqlDataAndStructure";

        public string SqlFunctionRefScriptNamespace         => $"{RootNamespace}.{DatabaseKey}.SqlFunction._RefScript";
        public string SqlFunctionChangeScriptNamespace      => $"{RootNamespace}.{DatabaseKey}.SqlFunction";

        public string SqlStoredRefScriptNamespace           => $"{RootNamespace}.{DatabaseKey}.SqlStored._RefScript";
        public string SqlStoredChangeScriptNamespace        => $"{RootNamespace}.{DatabaseKey}.SqlStored";
        
    }
}
