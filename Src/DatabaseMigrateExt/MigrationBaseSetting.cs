using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace DatabaseMigrateExt
{
    public class MigrationBaseSetting
    {
        public static string RootNamespace => ConfigurationManager.AppSettings["mgr:RootNamespace"];

        public static Assembly DefaultMigrationAssembly()
        {
            var usedAttrAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assy => assy != typeof(ExtMigrationAttribute).Assembly)
                .FirstOrDefault(assy => assy.GetTypes()
                    .Select(type => Attribute.IsDefined(type, typeof(ExtMigrationAttribute)))
                    .Any(x => x));

            if (usedAttrAssemblies != null)
            {
                return usedAttrAssemblies;
            }
            throw new TypeLoadException("Can not load migration assembly");
        }
    }
}
