using System.Configuration;
using System.Data.SqlClient;

namespace DatabaseMigrateExt.Models
{
    public class MigrateDatabaseContext
    {
        public MigrateDatabaseContext(string databaseKey)
        {
            DatabaseKey = databaseKey;
        }

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

        public string RootNamespace => ConfigurationManager.AppSettings["mgr:RootNamespace"];
        public string CurrentDatabsaeNamespace => $"{RootNamespace}.{DatabaseKey}";

        public string SqlArchitectureRefScriptNamespace     => $"{CurrentDatabsaeNamespace}._RefScript.DataAndStructure";
        public string SqlFunctionRefScriptNamespace         => $"{CurrentDatabsaeNamespace}._RefScript.Function";
        public string SqlStoredRefScriptNamespace           => $"{CurrentDatabsaeNamespace}._RefScript.Stored";
    }
}
