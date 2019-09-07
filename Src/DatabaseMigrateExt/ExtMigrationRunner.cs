using System;
using System.Collections.Concurrent;
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
        public static List<string> ValidScriptsStore { get; private set; } = new List<string>();

        internal string RootNamespace { get; set; }
        internal SortedList<int, string> DatabaseKeys { get; set; } = new SortedList<int, string>();
        internal SortedList<int, DatabaseScriptType> DatabaseScriptTypes { get; set; } = new SortedList<int, DatabaseScriptType>();
        internal Assembly MigrationAssembly { get; set; }
        internal bool HasInvaildScripts { get; set; }

        public ExtMigrationRunnerContext GetRunnerContext()
        {
            return new ExtMigrationRunnerContext
            {
                RootNamespace = this.RootNamespace,
                DatabaseKeys = this.DatabaseKeys,
                DatabaseScriptTypes = this.DatabaseScriptTypes,
                MigrationAssembly = this.MigrationAssembly,
                HasInvaildScripts = this.HasInvaildScripts
            };
        }

        private ExtMigrationRunner() { }

        /// <summary>
        /// Initialize default settings
        /// </summary>
        /// <returns></returns>
        public static ExtMigrationRunner Initialize()
        {
            var runner = new ExtMigrationRunner();

            #region Initialize RootNamespace

            var rootNamespaceFromAppSetting = ConfigurationManager.AppSettings["mgr:RootNamespace"];
            if (string.IsNullOrWhiteSpace(rootNamespaceFromAppSetting))
            {
                throw new ArgumentException("<rootNamespace> should not null or empty");
            }
            else
            {
                runner.RootNamespace = rootNamespaceFromAppSetting;
            }

            #endregion

            #region Initialize DatabaseKeys

            if (runner.DatabaseKeys == null)
            {
                runner.DatabaseKeys = new SortedList<int, string>();
            }
            var databaseKeys = ConfigurationManager.AppSettings["mgr:DatabaseKeys"];
            if (!string.IsNullOrWhiteSpace(databaseKeys))
            {
                var databaseKeyArr = databaseKeys.Split(',');
                if (databaseKeyArr.Any())
                {
                    for (var i = 0; i < databaseKeyArr.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(databaseKeyArr[i]))
                        {
                            runner.DatabaseKeys.Add(i, databaseKeyArr[i].Trim());
                        }
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
            runner.HasInvaildScripts = false;

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

            var duplicateVersionsTmp = new List<string>();
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
                        var versionLong = ((BaseExtMgrAttribute)migAttr).Version;
                        var key = $"{dbKey.Key}.{versionLong}";

                        if (BaseExtMgrAttribute.VersionBank != null && BaseExtMgrAttribute.VersionBank.ContainsKey(versionLong))
                        {
                            var dtVersion = BaseExtMgrAttribute.VersionBank[versionLong];

                            var versionInvailMsg = ValidateMigrateVersion(dtVersion.Year, dtVersion.Month, dtVersion.Day, dtVersion.Hour, dtVersion.Minute, dtVersion.Second);
                            if (!string.IsNullOrWhiteSpace(versionInvailMsg))
                            {
                                invalidScripts.Add(new KeyValuePair<Type, string>(script, versionInvailMsg));
                                continue;
                            }
                        }

                        if (duplicateVersionsTmp.Contains(key))
                        {
                            invalidScripts.Add(new KeyValuePair<Type, string>(script, $"[DUPLICATE VERSION - {{{dbKey.Key}}}]"));
                            continue;
                        }
                        duplicateVersionsTmp.Add(key);
                        ValidScriptsStore.Add($"{versionLong}.{dbKey.Key}");
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
                runner.HasInvaildScripts = true;
                Logger.Info($"There are some invalid scripts:");
                foreach (var item in invalidScripts.OrderBy(x => x.Key.Name))
                {
                    Logger.Info($"  > {item.Key.Name} {item.Value}");
                }

                Logger.Info("");
            }
        }

        static string ValidateMigrateVersion(int year, int month, int day, int hour, int minute, int second)
        {
            if (year <= 0 || year > 3000)
            {
                return $"Invaild migrate version:- Year: {year}";
            }

            if (month <= 0 || month > 12)
            {
                return $"Invaild migrate version:- Month: {month}";
            }

            if (day <= 0 || day > 31)
            {
                return $"Invaild migrate version:- Day: {day}";
            }

            if (hour < 0 || hour > 25)
            {
                return $"Invaild migrate version:- Hour: {hour}";
            }

            if (minute < 0 || minute > 60)
            {
                return $"Invaild migrate version:- Minute: {minute}";
            }

            if (second < 0 || second > 60)
            {
                return $"Invaild migrate version:- Second: {second}";
            }

            return string.Empty;
        }
    }
}
