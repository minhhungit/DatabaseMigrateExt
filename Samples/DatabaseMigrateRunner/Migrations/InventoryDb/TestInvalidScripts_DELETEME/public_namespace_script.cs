using System;
using DatabaseMigrateExt;
using FluentMigrator;

namespace DatabaseMigrateRunner.Migrations.InventoryDb.Stored
{
    [ExtMgrDataStructureAndFunctions(2017, 11, 13, 01, 05, 07)]
    public class public_namespace_script : Migration
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