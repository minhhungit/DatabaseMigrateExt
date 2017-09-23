using System;
using DatabaseMigrateExt;
using DatabaseMigrateExt.Attributes;
using FluentMigrator;

namespace DatabaseMigrateRunner.Migrations.InventoryDb
{
    [ExtMigration(scriptType: DatabaseScriptType.SqlDataAndStructure, year: 2017, month: 9, day: 22, hour: 02, minute: 08, second: 01)]
    public class InventoryDb_20170922_020801_inital_tables : Migration
    {
        public override void Up()
        {
            if (!this.Schema.Schema("dbo").Table("Inventory").Exists())
            {
                Create.Table("Inventory").InSchema("dbo")
                    .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
                    .WithColumn("Test").AsString(200).NotNullable();
            }
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }        
    }
}