using System;
using DatabaseMigrateExt;

namespace DatabaseMigrateRunner.Migrations.MovieStore
{
    [ExtMgrDataStructureAndFunctions(2017,10,22,15,45,01)]
    public class MovieStore_20171022_154501_inital_function : ExtFunctionsMigration
    {
        public override void Up()
        {
            ExecuteFunctionRefScript("dbo.fnSplit.sql");
            ExecuteFunctionRefScript("dbo.fnRemoveNonAlphaCharacters.sql");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }        
    }
}