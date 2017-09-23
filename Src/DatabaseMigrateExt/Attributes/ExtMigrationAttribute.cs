namespace DatabaseMigrateExt.Attributes
{
    public class ExtMigrationAttribute : FluentMigrator.MigrationAttribute
    {
        public ExtMigrationAttribute(DatabaseScriptType scriptType, int year, int month, int day, int hour, int minute, int second)
            : base(CalculateValue(scriptType, year, month, day, hour, minute, second))
        {
            ScriptType = scriptType;
        }

        public ExtMigrationAttribute(DatabaseScriptType scriptType, int year, int month, int day, int hour, int minute, int second, string author)
           : base(CalculateValue(scriptType, year, month, day, hour, minute, second))
        {
            ScriptType = scriptType;
            Author = author;
        }

        public string Author { get; private set; }
        public DatabaseScriptType ScriptType { get; private set; }

        private static long CalculateValue(DatabaseScriptType scriptType, int year, int month, int day, int hour, int minute, int second)
        {
            var branchNumber = (int)scriptType;

            return (branchNumber * 100000000000000L) +
                   (year * 10000000000L) +
                   (month * 100000000L) +
                   (day * 1000000L) +
                   (hour * 10000L) +
                   (minute * 100L) +
                   second;
        }
    }
}
