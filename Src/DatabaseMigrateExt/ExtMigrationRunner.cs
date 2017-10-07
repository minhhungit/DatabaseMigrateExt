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
        internal SortedList<int, string> DatabaseKeys { get; set; } = new SortedList<int, string>();
        internal SortedList<int, DatabaseScriptType> DatabaseLayers { get; set; } = new SortedList<int, DatabaseScriptType>();
        internal Assembly MigrationAssembly { get; set; }

        /// <summary>
        /// Initialize default settings
        /// </summary>
        /// <returns></returns>
        public static ExtMigrationRunner Initialize()
        {
            var runner = new ExtMigrationRunner();

            #region Initialize RootNamespace

            var rootNamespaceFromAppSetting = ConfigurationManager.AppSettings["mgr:RootNamespace"];
            runner.RootNamespace = rootNamespaceFromAppSetting;

            #endregion

            #region Initialize DatabaseKeys

            if (runner.DatabaseKeys == null)
            {
                runner.DatabaseKeys = new SortedList<int, string>();
            }
            var databaseKeys = ConfigurationManager.AppSettings["mgr:DatabaseKeys"];
            if (databaseKeys != null)
            {
                var databaseKeyArr = databaseKeys.Split(',');
                if (databaseKeyArr.Any())
                {
                    for (var i = 0; i < databaseKeyArr.Length; i++)
                    {
                        runner.DatabaseKeys.Add(i, databaseKeyArr[i].Trim());
                    }
                }

            }

            #endregion

            #region Initialize DatabaseLayers

            if (runner.DatabaseLayers == null)
            {
                runner.DatabaseLayers = new SortedList<int, DatabaseScriptType>();
            }

            var layers = Enum.GetValues(typeof(DatabaseScriptType))
                .OfType<DatabaseScriptType>().OrderBy(x => x);

            foreach (var layer in layers)
            {
                runner.DatabaseLayers.Add((int)layer, layer);
            }

            #endregion

            #region Initialize MigrationAssembly

            var usedAttrAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assy => assy != typeof(ExtMigrationRunner).Assembly)
                .FirstOrDefault(assy => assy.GetTypes()
                    .Select(type => Attribute.IsDefined(type, typeof(BaseExtMgrAttribute)))
                    .Any(x => x));

            if (usedAttrAssemblies != null)
            {
                runner.MigrationAssembly = usedAttrAssemblies;
            }

            #endregion

            return runner;
        }
    }
}
