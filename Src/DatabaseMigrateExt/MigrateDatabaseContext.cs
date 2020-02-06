using System;
using System.Configuration;
using System.Data.SqlClient;

namespace DatabaseMigrateExt
{
    public class MigrateDatabaseContext
    {
        public MigrateDatabaseContext(string rootNamespace, string databaseKey)
        {
            if (string.IsNullOrWhiteSpace(rootNamespace) || string.IsNullOrWhiteSpace(databaseKey))
            {
                throw new ArgumentException("<rootNamespace> or <databaseKey> should not null or empty");
            }

            RootNamespace = rootNamespace.Trim();
            DatabaseKey = databaseKey.Trim();
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
        public string CurrentRefScriptNamespace => $"{CurrentDatabaseNamespace}._RefScript";

        public string SqlArchitectureRefScriptNamespace
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[$"mgr:SqlArchitectureRefScriptNamespace"]))
                {
                    return $"{CurrentRefScriptNamespace}.DataAndStructure";
                }
                else
                {
                    return $"{CurrentRefScriptNamespace}.{ConfigurationManager.AppSettings[$"mgr:SqlArchitectureRefScriptNamespace"].ToString().Trim()}";
                }
            }
        }

        public string SqlFunctionRefScriptNamespace
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[$"mgr:SqlFunctionRefScriptNamespace"]))
                {
                    return $"{CurrentRefScriptNamespace}.Function";
                }
                else
                {
                    return $"{CurrentRefScriptNamespace}.{ConfigurationManager.AppSettings[$"mgr:SqlFunctionRefScriptNamespace"].ToString().Trim()}";
                }
            }
        }

        public string SqlStoredRefScriptNamespace
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[$"mgr:SqlStoredRefScriptNamespace"]))
                {
                    return $"{CurrentRefScriptNamespace}.Stored";
                }
                else
                {
                    return $"{CurrentRefScriptNamespace}.{ConfigurationManager.AppSettings[$"mgr:SqlStoredRefScriptNamespace"].ToString().Trim()}";
                }
            }
        }

        public string SqlGeneralScriptRefScriptNamespace
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[$"mgr:SqlGeneralScriptRefScriptNamespace"]))
                {
                    return $"{CurrentRefScriptNamespace}._General";
                }
                else
                {
                    return $"{CurrentRefScriptNamespace}.{ConfigurationManager.AppSettings[$"mgr:SqlGeneralScriptRefScriptNamespace"].ToString().Trim()}";
                }
            }
        }
    }
}
