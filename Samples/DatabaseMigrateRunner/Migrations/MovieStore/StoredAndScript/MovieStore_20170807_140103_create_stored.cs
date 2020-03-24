using System;
using DatabaseMigrateExt;

namespace DatabaseMigrateRunner.Migrations.MovieStore
{
    [ExtMgrStoredProcedures(2017, 8, 7, 14, 01, 03)]
    public class MovieStore_20170807_140103_create_stored : ExtStoreProcedureMigration
    {
        public override void Up()
        {
            this.ExecuteStoredProcedureRefScript("dbo.GetMovieById.sql");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}