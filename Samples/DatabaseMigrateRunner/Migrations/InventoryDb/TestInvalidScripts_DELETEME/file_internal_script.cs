using System;
using DatabaseMigrateExt;
using FluentMigrator;

namespace DatabaseMigrateRunner.Migrations.InventoryDb
{
    [ExtMgrDataStructureAndFunctions("Hung Vo", 2017, 11, 13, 01, 05, 03, false)]
    internal class file_internal_script : Migration
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