using System;
using DatabaseMigrateExt;
using DatabaseMigrateExt.Attributes;
using FluentMigrator;

namespace DatabaseMigrateRunner.Migrations.InventoryDb
{
    [ExtMigration(DatabaseScriptType.SqlDataAndStructure, 2017, 9, 22, 02, 08, 01)]
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