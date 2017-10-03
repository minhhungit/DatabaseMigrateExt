using FluentMigrator;
using System;
using DatabaseMigrateExt.Attributes;
using DatabaseMigrateExt.Models;
using DatabaseMigrateExt.Utils;

namespace DatabaseMigrateRunner.Migrations.MovieStore
{
    [ExtMigration(author: "Hung Vo", scriptType: DatabaseScriptType.SqlStoredProcedure, year: 2017, month: 8, day: 7, hour: 14, minute: 01, second: 03)]
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