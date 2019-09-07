using FluentMigrator;
using System;
using DatabaseMigrateExt;
using DatabaseMigrateExt.Utils;

namespace DatabaseMigrateRunner.Migrations.MovieStore
{
    [ExtMgrStoredProcedureAndScript(2017, 8, 7, 14, 01, 03)]
    public class MovieStore_20170807_140103_create_stored : Migration
    {
        public override void Up()
        {
            this.ExecuteStoredProcedure("dbo.GetMovieById.sql");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}