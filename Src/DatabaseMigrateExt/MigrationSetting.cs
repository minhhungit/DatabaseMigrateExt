using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DatabaseMigrateExt
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

        public List<string> DatabaseKeys { get; set; }

        public List<DatabaseScriptType> AvailableLevels { get; set; } = Enum.GetValues(typeof(DatabaseScriptType)).OfType<DatabaseScriptType>().ToList();

        public Assembly MigrationAssembly { get; set; } = Assembly.GetCallingAssembly();
    }
}
