using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace DatabaseMigrateExt.Models
{
    public class MigrationSetting
    {
        public MigrationSetting()
        {
        }

        public MigrationSetting(List<string> databaseKeys)
        {
            DatabaseKeys = databaseKeys;
        }

        public MigrationSetting(List<string> databaseKeys, Assembly migrationAssembly)
        {
            DatabaseKeys = databaseKeys;
            MigrationAssembly = migrationAssembly;
        }

        public List<string> DatabaseKeys { get; set; } =
            ConfigurationManager.AppSettings["mgr:DatabaseKeys"]
                .Split(',')
                .Select(p => p.Trim())
                .ToList();

        public List<DatabaseScriptType> AvailableLevels { get; set; } =
            Enum.GetValues(typeof(DatabaseScriptType))
                .OfType<DatabaseScriptType>()
                .ToList();

        public Assembly MigrationAssembly { get; set; } = Assembly.GetCallingAssembly();
    }
}
