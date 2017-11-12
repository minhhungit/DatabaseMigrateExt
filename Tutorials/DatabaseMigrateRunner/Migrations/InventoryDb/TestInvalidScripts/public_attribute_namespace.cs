using System;
using DatabaseMigrateExt;
using FluentMigrator;

namespace DatabaseMigrateRunner.Migrations.InventoryDb.Stored
{
    [TimestampedMigration(2017, 9, 23, 03, 09, 02)]
    public class public_attribute_namespace : Migration
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