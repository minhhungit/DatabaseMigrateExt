using System.Collections.Generic;
using System.Reflection;

namespace DatabaseMigrateExt
{
    public class ExtMigrationRunnerContext
    {
        public Assembly MigrationAssembly { get; internal set; }
        public string RootNamespace { get; internal set; }
        public SortedList<int, string> DatabaseKeys { get; internal set; }
        public SortedList<int, DatabaseScriptType> DatabaseScriptTypes { get; internal set; }
        public bool HasInvaildScripts { get; internal set; }
    }
}
