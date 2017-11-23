using System;
using DatabaseMigrateExt;
using FluentMigrator;

namespace DatabaseMigrateRunner.Migrations.InventoryDb
{
    [ExtMgrDataStructure("Hung Vo", 2017, 9, 23, 03, 09, 02, false)]
    public class duplicate_version_script : Migration
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