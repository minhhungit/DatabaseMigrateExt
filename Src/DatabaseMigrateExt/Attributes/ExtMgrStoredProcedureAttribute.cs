namespace DatabaseMigrateExt
{
    public class ExtMgrStoredProcedureAttribute : BaseExtMgrAttribute
    {
        private static DatabaseScriptType ScriptType => DatabaseScriptType.SqlStoredProcedure;

        public ExtMgrStoredProcedureAttribute(int year, int month, int day, int hour, int minute, int second)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
        }

        public ExtMgrStoredProcedureAttribute(int year, int month, int day, int hour, int minute, int second, bool useTransaction)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
            UseTransaction = useTransaction;
        }

        public ExtMgrStoredProcedureAttribute(string author, int year, int month, int day, int hour, int minute, int second)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
            Author = author;
        }

        public ExtMgrStoredProcedureAttribute(string author, int year, int month, int day, int hour, int minute, int second, bool useTransaction)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
            UseTransaction = useTransaction;
            Author = author;
        }
    }
}
