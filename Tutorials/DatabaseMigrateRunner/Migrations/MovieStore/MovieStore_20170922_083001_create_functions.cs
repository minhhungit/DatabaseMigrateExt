using DatabaseMigrateExt.Attributes;
using FluentMigrator;
using System;
using DatabaseMigrateExt.Models;
using DatabaseMigrateExt.Utils;

namespace DatabaseMigrateRunner.Migrations.MovieStore
{
    [ExtMigration(author: "Hung Vo", scriptType: DatabaseScriptType.SqlFunction , year: 2017, month: 9, day: 22, hour: 8, minute: 30, second: 01)]
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