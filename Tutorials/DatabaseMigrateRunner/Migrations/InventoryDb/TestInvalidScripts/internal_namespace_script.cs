using System;
using DatabaseMigrateExt;
using FluentMigrator;

namespace DatabaseMigrateRunner.Migrations.InventoryDb.Stored
{
    [ExtMgrDataStructure("Hung Vo", 2017, 11, 13, 01, 05, 04, false)]
    internal class internal_namespace_script : Migration
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