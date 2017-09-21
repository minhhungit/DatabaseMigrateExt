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
            DatabaseKeys = new List<string>();
            AvailableLevels = Enum.GetValues(typeof(DatabaseScriptType)).OfType<DatabaseScriptType>().ToList();
        }

        public List<string> DatabaseKeys { get; set; }
        public List<DatabaseScriptType> AvailableLevels { get; set; }
        public Assembly MigrationAssembly { get; set; }
    }
}
