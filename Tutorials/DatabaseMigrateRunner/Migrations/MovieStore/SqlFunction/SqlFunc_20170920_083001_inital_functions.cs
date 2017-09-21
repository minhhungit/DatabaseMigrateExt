using DatabaseMigrateExt;
using DatabaseMigrateExt.Attributes;
using FluentMigrator;
using System;

namespace DatabaseMigrateRunner.Migrations.MovieStore.SqlFunction
{
    [ExtMigration(author: "Hung Vo", scriptType: DatabaseScriptType.SqlFunction , year: 2017, month: 9, day: 20, hour: 8, minute: 30, second: 01)]
    public class SqlFunc_20170920_083001_inital_functions : Migration
    {
        public override void Up()
        {
            this.ExecuteFunction("dbo.fnSplit.sql");
            this.ExecuteFunction("dbo.fnRemoveNonAlphaCharacters.sql");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }        
    }
}