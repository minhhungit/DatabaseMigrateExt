using System;
using DatabaseMigrateExt;
using FluentMigrator;

namespace DatabaseMigrateRunner.Migrations.InventoryDb
{
    [ExtMgrDataStructure(5000, 11, 13, 01, 05, 07)]
    public class invalid_migrate_version_year : Migration
    {
        public override void Up() { }

        public override void Down() { throw new NotImplementedException(); }
    }

    [ExtMgrDataStructure(2018, 25, 13, 01, 05, 07)]
    public class invalid_migrate_version_month : Migration
    {
        public override void Up() { }

        public override void Down() { throw new NotImplementedException(); }
    }

    [ExtMgrDataStructure(2018, 11, 62, 01, 05, 07)]
    public class invalid_migrate_version_day : Migration
    {
        public override void Up() { }

        public override void Down() { throw new NotImplementedException(); }
    }

    [ExtMgrDataStructure(2018, 11, 13, 44, 05, 07)]
    public class invalid_migrate_version_hour : Migration
    {
        public override void Up() { }

        public override void Down() { throw new NotImplementedException(); }
    }

    [ExtMgrDataStructure(2018, 11, 13, 01, 115, 07)]
    public class invalid_migrate_version_minute : Migration
    {
        public override void Up() { }

        public override void Down() { throw new NotImplementedException(); }
    }

    [ExtMgrDataStructure(2018, 11, 13, 01, 05, 523532)]
    public class invalid_migrate_version_second : Migration
    {
        public override void Up() { }

        public override void Down() { throw new NotImplementedException(); }
    }
}