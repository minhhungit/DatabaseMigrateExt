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
            ValidateMigrateVersion(year, month, day, hour, minute, second);

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

        public static void ValidateMigrateVersion(int year, int month, int day, int hour, int minute, int second)
        {
            if (year <= 0 || year > 3000)
            {
                throw new System.Exception($"Invaild migrate version:- Year: {year}");
            }

            if (month <= 0 || month > 12)
            {
                throw new System.Exception($"Invaild migrate version:- Month: {month}");
            }

            if (day <= 0 || day > 31)
            {
                throw new System.Exception($"Invaild migrate version:- Day: {day}");
            }

            if (hour < 0 || hour > 25)
            {
                throw new System.Exception($"Invaild migrate version:- Hour: {hour}");
            }

            if (minute < 0 || minute > 60)
            {
                throw new System.Exception($"Invaild migrate version:- Minute: {minute}");
            }

            if (second < 0 || second > 60)
            {
                throw new System.Exception($"Invaild migrate version:- Second: {second}");
            }
        }
    }
}