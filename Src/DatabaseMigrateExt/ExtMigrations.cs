﻿using DatabaseMigrateExt.Utils;
using FluentMigrator;

namespace DatabaseMigrateExt
{
    public abstract class ExtBaseMigration : Migration
    {
        public void ExecuteGeneralRefScript(string fileName)
        {
            MigrationUtils.ExecuteGeneralRefScript(this, fileName);
        }
    }

    public abstract class ExtDataStructureMigration : ExtBaseMigration
    {
        public void ExecuteDataStructureRefScript(string fileName)
        {
            MigrationUtils.ExecuteDataStructureRefScript(this, fileName);
        }
    }

    public abstract class ExtFunctionsMigration : ExtBaseMigration
    {
        public void ExecuteFunctionRefScript(string fileName)
        {
            MigrationUtils.ExecuteFunctionRefScript(this, fileName);
        }
    }

    public abstract class ExtStoreProcedureMigration : ExtBaseMigration
    {
        public void ExecuteStoredProcedureRefScript(string fileName)
        {
            MigrationUtils.ExecuteStoredProcedureRefScript(this, fileName);
        }
    }
}
