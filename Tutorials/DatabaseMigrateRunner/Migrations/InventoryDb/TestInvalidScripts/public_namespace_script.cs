using System;
using DatabaseMigrateExt;
using FluentMigrator;

namespace DatabaseMigrateRunner.Migrations.InventoryDb.Stored
{
    [ExtMgrDataStructure(2017, 11, 13, 01, 05, 07)]
    public class public_namespace_script : Migration
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