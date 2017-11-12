using System;
using DatabaseMigrateExt;
using FluentMigrator;

namespace DatabaseMigrateRunner.Migrations.MovieStore
{
    [ExtMgrDataStructure(2017, 9, 23, 03, 09, 02)]
    public class checkDuplicateScript : Migration
    {
        public override void Up()
        {
            
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }        
    }
}