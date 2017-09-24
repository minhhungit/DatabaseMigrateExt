using System;
using DatabaseMigrateExt.Attributes;
using DatabaseMigrateExt.Models;
using FluentMigrator;

namespace DatabaseMigrateRunner.Migrations.InventoryDb
{
    [ExtMigration(DatabaseScriptType.SqlDataAndStructure, 2017, 9, 23, 03, 09, 02, false)]
    public class InventoryDb_20170923_030902_enable_broker : Migration
    {
        public override void Up()
        {
            var appContext = (MigrateDatabaseContext) this.ApplicationContext;
            this.Execute.Sql($"ALTER DATABASE [{appContext.DatabaseName}] SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE;");

            // or
            this.Execute.Sql(@"ALTER DATABASE CURRENT SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE;");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }        
    }
}