# <img src="https://raw.githubusercontent.com/minhhungit/DatabaseMigrateExt/master/Src/DatabaseMigrateExt/Resouces/favicon.ico" width="32"> DatabaseMigrateExt <img src="https://ci.appveyor.com/api/projects/status/el1b0f9rrgup8vgv?svg=true" /> <a href="https://www.nuget.org/packages/DatabaseMigrateExt/"><img src="https://img.shields.io/nuget/v/DatabaseMigrateExt.svg?style=flat" /></a>

A new way, new rule to work with MSSQL database version use fluent migrator
- [https://minhhungit.github.io/DatabaseMigrateExt](https://minhhungit.github.io/DatabaseMigrateExt)
- [https://github.com/minhhungit/DatabaseMigrateExt](https://github.com/minhhungit/DatabaseMigrateExt)


## Installation
>Install-Package DatabaseMigrateExt


## Sample Project

See [Sample Project](https://github.com/minhhungit/DatabaseMigrateExt/tree/master/Tutorials/DatabaseMigrateRunner) at [https://github.com/minhhungit/DatabaseMigrateExt/tree/master/Tutorials/DatabaseMigrateRunner](https://github.com/minhhungit/DatabaseMigrateExt/tree/master/Tutorials/DatabaseMigrateRunner)


## Config
```xml
<add key="mgr:DatabaseKeys" value="MovieStore, InventoryDb"/>
<add key="mgr:RootNamespace" value="DatabaseMigrateRunner.Migrations"/>

<add key="mgr:MovieStore_ConnString" value="Data Source=JINPC\MSSQL2014;Initial Catalog=FirstDb;User ID=u;Password=p;Connection Timeout=6000;"/>
<add key="mgr:InventoryDb_ConnString" value="Data Source=JINPC\MSSQL2014;Initial Catalog=SecondDb;User ID=u;Password=p;Connection Timeout=6000;"/>
```


## Migration Scripts
*Migration structure:*

<img src="https://raw.githubusercontent.com/minhhungit/DatabaseMigrateExt/master/wiki/sample%20migration%20structure.png" style="width: 130%; height: 130%" />

*Sample script:*
```c#
[ExtMigration(scriptType: DatabaseScriptType.SqlDataAndStructure, year: 2017, month: 9, day: 21, hour: 19, minute: 40, second: 01)]
public class SqlStructure_20170921_194001_inital_tables : Migration
{
    public override void Up()
    {
        // do something here
    }
    ....
}
```


## Run
```c#
using DatabaseMigrateExt;
.
.
static void Main(string[] args)
{
    var databaseKeys = new List<string> { "MovieStore", "InventoryDb" };

    // OR load values from AppSetting
    // databaseKeys = ConfigurationManager.AppSettings["mgr:DatabaseKeys"].Split(',').Select(p => p.Trim()).ToList();

    var setting = new MigrationSetting
    {
        DatabaseKeys = databaseKeys
    };

    Console.WriteLine("Start...");
    MigrationManager.Instance.Run(setting);

    Console.WriteLine("Completed!");
    Console.ReadKey();
}
```
