using System;
using DatabaseMigrateExt;
using FluentMigrator;

namespace DatabaseMigrateRunner.Migrations.InventoryDb
{
    [ExtMgrDataStructure("Hung Vo", 2017, 11, 13, 01, 05, 05, false)]
    class private_classs_script : Migration
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