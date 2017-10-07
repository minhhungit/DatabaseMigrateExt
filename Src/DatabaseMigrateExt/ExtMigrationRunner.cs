using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace DatabaseMigrateExt
{
    public class ExtMigrationRunner
    {
        internal string RootNamespace { get; set; }
        internal List<string> DatabaseKeys { get; set; }
        internal List<DatabaseScriptType> DatabaseLayers { get; set; }
        internal Assembly MigrationAssembly { get; set; }

        /// <summary>
        /// Initialize default settings
        /// </summary>
        /// <returns></returns>
        public static ExtMigrationRunner Initialize()
        {
            var runner = new ExtMigrationRunner();
            var rootNamespaceFromAppSetting = ConfigurationManager.AppSettings["mgr:RootNamespace"];
            runner.RootNamespace = rootNamespaceFromAppSetting;

            var databaseKeys = ConfigurationManager.AppSettings["mgr:DatabaseKeys"];
            if (databaseKeys != null)
            {
                runner.DatabaseKeys = databaseKeys.Split(',').Select(p => p.Trim()).ToList();
            }

            runner.DatabaseLayers = Enum.GetValues(typeof(DatabaseScriptType))
                .OfType<DatabaseScriptType>()
                .ToList();

            var usedAttrAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assy => assy != typeof(ExtMigrationRunner).Assembly)
                .FirstOrDefault(assy => assy.GetTypes()
                    .Select(type => Attribute.IsDefined(type, typeof(BaseExtMgrAttribute)))
                    .Any(x => x));

            if (usedAttrAssemblies != null)
            {
                runner.MigrationAssembly = usedAttrAssemblies;
            }

            return runner;
        }
    }
}
