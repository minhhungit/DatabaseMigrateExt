using System;
using System.ComponentModel;

namespace DatabaseMigrateExt.Utils
{
    public static class EnumUtils
    {
        public static string GetEnumDescription(this Enum enumeratedType)
        {
            var description = enumeratedType.ToString();

            var fieldInfo = enumeratedType.GetType().GetField(enumeratedType.ToString());

            if (fieldInfo == null)
            {
                return description;
            }

            var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                description = ((DescriptionAttribute)attributes[0]).Description;
            }

            return description;
        }
    }
}
