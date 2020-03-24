using System;

namespace DatabaseMigrateExt
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExtMgrDataStructureAndFunctionsAttribute : BaseExtMgrAttribute
    {
        private static DatabaseScriptType ScriptType => DatabaseScriptType.SqlDataStructureFunction;

        public ExtMgrDataStructureAndFunctionsAttribute(int year, int month, int day, int hour, int minute, int second)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
        }

        public ExtMgrDataStructureAndFunctionsAttribute(int year, int month, int day, int hour, int minute, int second, bool useTransaction)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
            UseTransaction = useTransaction;
        }

        public ExtMgrDataStructureAndFunctionsAttribute(string author, int year, int month, int day, int hour, int minute, int second)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
            Author = author;
        }

        public ExtMgrDataStructureAndFunctionsAttribute(string author, int year, int month, int day, int hour, int minute, int second, bool useTransaction)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
            UseTransaction = useTransaction;
            Author = author;
        }
    }
}