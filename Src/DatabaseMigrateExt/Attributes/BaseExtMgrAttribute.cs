using FluentMigrator;

namespace DatabaseMigrateExt
{
    public abstract class BaseExtMgrAttribute : MigrationAttribute
    {
        protected BaseExtMgrAttribute(long version) : base(version)
        {
        }

        protected BaseExtMgrAttribute(long version, string description) : base(version, description)
        {
        }

        protected BaseExtMgrAttribute(long version, TransactionBehavior transactionBehavior) : base(version, transactionBehavior)
        {
        }

        protected BaseExtMgrAttribute(long version, TransactionBehavior transactionBehavior, string description) : base(version, transactionBehavior, description)
        {
        }

        protected static long CalculateValue(DatabaseScriptType scriptType, int year, int month, int day, int hour, int minute, int second)
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

        public string Author { get; set; }
        public bool UseTransaction { get; set; } = true;
    }
}