using System.ComponentModel;

namespace DatabaseMigrateExt.Models
{
    public enum DatabaseScriptType
    {
        [Description("Data And Structure")]
        SqlDataAndStructure = 1000,
        [Description("Function")]
        SqlFunction = 2000,
        [Description("Stored Procedure")]
        SqlStoredProcedure = 3000
    }
}
