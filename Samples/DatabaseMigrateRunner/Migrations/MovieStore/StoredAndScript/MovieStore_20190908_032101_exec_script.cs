using FluentMigrator;
using System;
using DatabaseMigrateExt;
using DatabaseMigrateExt.Utils;

namespace DatabaseMigrateRunner.Migrations.MovieStore
{
    [ExtMgrStoredProcedureAndScript(2019,09,08,03,21,01)]
    public class MovieStore_20190908_032101_exec_script : Migration
    {
        public override void Up()
        {
            this.ExecuteTsqlScript("v0000001_do_something.sql");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}