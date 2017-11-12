using System;
using DatabaseMigrateExt;
using FluentMigrator;

namespace DatabaseMigrateRunner.Migrations.InventoryDb
{
    [ExtMgrDataStructure("Hung Vo", 2017, 9, 23, 03, 09, 02, false)]
    class private_classs : Migration
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