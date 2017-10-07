using FluentMigrator;
using System;
using DatabaseMigrateExt;
using DatabaseMigrateExt.Utils;

namespace DatabaseMigrateRunner.Migrations.MovieStore
{
    [ExtMgrFunction(2017, 9, 22, 8, 30, 01)]
    public class MovieStore_20170922_083001_create_functions : Migration
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