using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Common.Logging;
using FluentMigrator;

namespace DatabaseMigrateExt
{
    public class ExtMigrationRunner
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ExtMigrationRunner));

        internal string RootNamespace { get; set; }
        internal SortedList<int, string> DatabaseKeys { get; set; } = new SortedList<int, string>();
        internal SortedList<int, DatabaseScriptType> DatabaseScriptTypes { get; set; } = new SortedList<int, DatabaseScriptType>();
        internal Assembly MigrationAssembly { get; set; }

        public ExtMigrationRunnerContext GetRunnerContext()
        {
            return new ExtMigrationRunnerContext
            {
                RootNamespace = this.RootNamespace,
                DatabaseKeys = this.DatabaseKeys,
                DatabaseScriptTypes = this.DatabaseScriptTypes,
                MigrationAssembly = this.MigrationAssembly
            };
        }

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

            if (runner.DatabaseScriptTypes == null)
            {
                runner.DatabaseScriptTypes = new SortedList<int, DatabaseScriptType>();
            }

            var layers = Enum.GetValues(typeof(DatabaseScriptType))
                .OfType<DatabaseScriptType>().OrderBy(x => x);

            foreach (var layer in layers)
            {
                runner.DatabaseScriptTypes.Add((int)layer, layer);
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

            ShowValidateScripts(runner);
            return runner;
        }

        static void ShowValidateScripts(ExtMigrationRunner runner)
        {
            // warning invalid scripts
            var invalidScripts = new List<KeyValuePair<Type, string>>();

            var allScriptFiles = runner.MigrationAssembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(MigrationBase)))
                .ToList();

            var validNamespaces = new Dictionary<string, string>();
            foreach (var dbKey in runner.DatabaseKeys)
            {
                if (!validNamespaces.ContainsKey(dbKey.Value))
                {
                    validNamespaces.Add(dbKey.Value, $"{runner.RootNamespace}.{dbKey.Value}");
                }
            }

            var scriptVersions = new List<string>();
            foreach (var script in allScriptFiles)
            {
                if (!validNamespaces.Values.Contains(script.Namespace))
                {
                    invalidScripts.Add(new KeyValuePair<Type, string>(script, "[INCORRECT NAMESPACE]"));
                    continue;
                }
                else
                {
                    var dbKey = validNamespaces.First(x => x.Value == script.Namespace);

                    if (!script.IsPublic)
                    {
                        invalidScripts.Add(new KeyValuePair<Type, string>(script, $"[NOT PUBLIC - {{{dbKey.Key}}}]"));
                        continue;
                    }

                    var migAttr = script.GetCustomAttributes(typeof(BaseExtMgrAttribute), false).FirstOrDefault();

                    if (migAttr != null)
                    {
                        var key = $"{dbKey.Key}.{((BaseExtMgrAttribute)migAttr).Version}";

                        if (scriptVersions.Contains(key))
                        {
                            invalidScripts.Add(new KeyValuePair<Type, string>(script, $"[DUPLICATE VERSION - {{{dbKey.Key}}}]"));
                            continue;
                        }
                        scriptVersions.Add(key);
                    }
                    else
                    {
                        invalidScripts.Add(new KeyValuePair<Type, string>(script, $"[INCORRECT ATTRIBUTE - {{{dbKey.Key}}}]"));
                        continue;
                    }
                }
            }

            if (invalidScripts.Any())
            {
                Logger.Info($"There are some invalid scripts:");
                foreach (var item in invalidScripts.OrderBy(x => x.Key.Name))
                {
                    Logger.Info($"  > {item.Key.Name} {item.Value}");
                }

                Logger.Info("");
            }
        }
    }
}
