using System.Configuration;
using System.Data.SqlClient;

namespace DatabaseMigrateExt.Models
{
    public class MigrateDatabaseContext
    {
        public MigrateDatabaseContext(string rootNamespace, string databaseKey)
        {
            RootNamespace = rootNamespace;
            DatabaseKey = databaseKey;
        }

        public string RootNamespace { get; set; }
        public string DatabaseKey { get; set; }

        public string ConnectionString => ConfigurationManager.AppSettings[$"mgr:{DatabaseKey}_ConnString"];

        public string DatabaseName
        {
            get
            {
                var builder = new SqlConnectionStringBuilder(ConnectionString);
                return builder.InitialCatalog;
            }
        }

        public int ConnectionTimeout
        {
            get
            {
                var builder = new SqlConnectionStringBuilder(ConnectionString);
                return builder.ConnectTimeout;
            }
        }

        public string CurrentDatabaseNamespace => $"{RootNamespace}.{DatabaseKey}";
        public string CurrentRefScriptNamespace => $"{RootNamespace}.{DatabaseKey}._RefScript";

        public string SqlArchitectureRefScriptNamespace     => $"{CurrentRefScriptNamespace}.DataAndStructure";
        public string SqlFunctionRefScriptNamespace         => $"{CurrentRefScriptNamespace}.Function";
        public string SqlStoredRefScriptNamespace           => $"{CurrentRefScriptNamespace}.Stored";
    }
}
