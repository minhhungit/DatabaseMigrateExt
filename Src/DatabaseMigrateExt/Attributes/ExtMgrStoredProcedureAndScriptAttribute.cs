namespace DatabaseMigrateExt
{
    public class ExtMgrStoredProcedureAndScriptAttribute : BaseExtMgrAttribute
    {
        private static DatabaseScriptType ScriptType => DatabaseScriptType.SqlStoredProcedureAndTsqlScripts;

        public ExtMgrStoredProcedureAndScriptAttribute(int year, int month, int day, int hour, int minute, int second)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
        }

        public ExtMgrStoredProcedureAndScriptAttribute(int year, int month, int day, int hour, int minute, int second, bool useTransaction)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
            UseTransaction = useTransaction;
        }

        public ExtMgrStoredProcedureAndScriptAttribute(string author, int year, int month, int day, int hour, int minute, int second)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
            Author = author;
        }

        public ExtMgrStoredProcedureAndScriptAttribute(string author, int year, int month, int day, int hour, int minute, int second, bool useTransaction)
            : base(CalculateValue(ScriptType, year, month, day, hour, minute, second))
        {
            UseTransaction = useTransaction;
            Author = author;
        }
    }
}
