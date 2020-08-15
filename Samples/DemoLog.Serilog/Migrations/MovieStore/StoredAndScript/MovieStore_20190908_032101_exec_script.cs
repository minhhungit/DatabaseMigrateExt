using System;
using DatabaseMigrateExt;

namespace DemoLog.Serilog.Migrations.MovieStore
{
    [ExtMgrStoredProcedures(2019,09,08,03,21,01)]
    public class MovieStore_20190908_032101_exec_script : ExtStoreProcedureMigration
    {
        public override void Up()
        {
            base.ExecuteGeneralRefScript("v0000001_do_something.sql");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}