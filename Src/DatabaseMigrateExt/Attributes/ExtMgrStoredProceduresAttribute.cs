using System;

namespace DatabaseMigrateExt
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExtMgrStoredProceduresAttribute : BaseExtMgrAttribute
    {
        private static DatabaseScriptType ScriptType => DatabaseScriptType.SqlStoredProcedures;

        public ExtMgrStoredProceduresAttribute(int year, int month, int day, int hour, int minute, int second)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
        }

        public ExtMgrStoredProceduresAttribute(int year, int month, int day, int hour, int minute, int second, bool useTransaction)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
            UseTransaction = useTransaction;
        }

        public ExtMgrStoredProceduresAttribute(string author, int year, int month, int day, int hour, int minute, int second)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
            Author = author;
        }

        public ExtMgrStoredProceduresAttribute(string author, int year, int month, int day, int hour, int minute, int second, bool useTransaction)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
            UseTransaction = useTransaction;
            Author = author;
        }
    }
}
