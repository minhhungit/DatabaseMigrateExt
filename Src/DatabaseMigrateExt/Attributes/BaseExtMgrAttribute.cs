using FluentMigrator;
using System;
using System.Collections.Concurrent;

namespace DatabaseMigrateExt
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class BaseExtMgrAttribute : MigrationAttribute
    {
        public static ConcurrentDictionary<long, DateTime> VersionBank { get; private set; }

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

            var version = (branchNumber * 100000000000000L) +
                   (year * 10000000000L) +
                   (month * 100000000L) +
                   (day * 1000000L) +
                   (hour * 10000L) +
                   (minute * 100L) +
                   second;

            if (VersionBank == null)
            {
                VersionBank = new ConcurrentDictionary<long, DateTime>();
            }

            if (!VersionBank.ContainsKey(version))
            {
                VersionBank[version] = new DateTime(year, month, day, hour, minute, second);
            }            

            return version;
        }

        public string Author { get; set; }
        public bool UseTransaction { get; set; } = true;
    }
}