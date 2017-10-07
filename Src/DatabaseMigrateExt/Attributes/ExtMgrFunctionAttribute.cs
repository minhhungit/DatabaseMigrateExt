namespace DatabaseMigrateExt
{
    public class ExtMgrFunctionAttribute : BaseExtMgrAttribute
    {
        private static DatabaseScriptType ScriptType => DatabaseScriptType.SqlFunction;

        public ExtMgrFunctionAttribute(int year, int month, int day, int hour, int minute, int second)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
        }

        public ExtMgrFunctionAttribute(int year, int month, int day, int hour, int minute, int second, bool useTransaction)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
            UseTransaction = useTransaction;
        }

        public ExtMgrFunctionAttribute(string author, int year, int month, int day, int hour, int minute, int second)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
            Author = author;
        }

        public ExtMgrFunctionAttribute(string author, int year, int month, int day, int hour, int minute, int second, bool useTransaction)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
            UseTransaction = useTransaction;
            Author = author;
        }
    }
}