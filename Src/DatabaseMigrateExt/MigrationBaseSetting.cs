using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace DatabaseMigrateExt
{
    public class MigrationBaseSetting
    {
        internal static string RootNamespace => ConfigurationManager.AppSettings["mgr:RootNamespace"];

        internal static Assembly DefaultMigrationAssembly()
        {
            var usedAttrAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assy => assy != typeof(MigrationManager).Assembly)
                .FirstOrDefault(assy => assy.GetTypes()
                    .Select(type => Attribute.IsDefined(type, typeof(BaseExtMgrAttribute)))
                    .Any(x => x));

            if (usedAttrAssemblies != null)
            {
                return usedAttrAssemblies;
            }
            throw new TypeLoadException("Can not load migration assembly");
        }
    }
}
