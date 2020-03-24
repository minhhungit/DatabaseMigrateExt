using System;
using DatabaseMigrateExt;
using FluentMigrator;

namespace DatabaseMigrateRunner.Migrations.InventoryDb
{
    [ExtMgrDataStructureAndFunctions(5000, 11, 13, 01, 05, 07)]
    public class invalid_migrate_version_year : Migration
    {
        public override void Up() { }

        public override void Down() { throw new NotImplementedException(); }
    }
}