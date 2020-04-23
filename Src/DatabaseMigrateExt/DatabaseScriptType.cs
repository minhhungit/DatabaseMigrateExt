using System.ComponentModel;

namespace DatabaseMigrateExt
{
    public enum DatabaseScriptType
    {
        [Description("Data, Structure And Functions")]
        SqlDataStructureFunction = 1000,
        [Description("Stored Procedures")]
        SqlStoredProcedures = 3000
    }
}
