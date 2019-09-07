# <img rel="https://raw.githubusercontent.com/minhhungit/DatabaseMigrateExt/master/develop/Icon/logo.png" src="https://raw.githubusercontent.com/minhhungit/DatabaseMigrateExt/develop/wiki/Icon/logo.png" width="32" /> DatabaseMigrateExt <a href="https://www.nuget.org/packages/DatabaseMigrateExt/"><img src="https://img.shields.io/nuget/v/DatabaseMigrateExt.svg?style=flat" /></a> [![Codacy Badge](https://api.codacy.com/project/badge/Grade/d1b3c86714594b6894bb9382c3f85f33)](https://www.codacy.com/app/it.minhhung/DatabaseMigrateExt?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=minhhungit/DatabaseMigrateExt&amp;utm_campaign=Badge_Grade) [![CodeFactor](https://www.codefactor.io/repository/github/minhhungit/databasemigrateext/badge)](https://www.codefactor.io/repository/github/minhhungit/databasemigrateext)

A new way, new rule to work with MSSQL database version use [fluent migrator](https://github.com/fluentmigrator/fluentmigrator)
- [https://minhhungit.github.io/DatabaseMigrateExt](https://minhhungit.github.io/DatabaseMigrateExt)

### Installation
>Install-Package DatabaseMigrateExt


### Sample Project

See [Sample Project](https://github.com/minhhungit/DatabaseMigrateExt/tree/master/Samples) at [https://github.com/minhhungit/DatabaseMigrateExt/tree/master/Samples](https://github.com/minhhungit/DatabaseMigrateExt/tree/master/Samples)


### Config

```xml
<add key="mgr:DatabaseKeys"     value="MovieStore, InventoryDb"/>
<add key="mgr:RootNamespace"    value="DatabaseMigrateRunner.Migrations"/>

<add key="mgr:MovieStore_ConnString"    value="Your_ConnectionString_For_MovieStore_At_Here"/>
<add key="mgr:InventoryDb_ConnString"   value="Your_ConnectionString_For_Inventory_At_Here"/>
```

*Note*: 
> mgr:MovieStore_ConnString = 'mgr:' + [DatabaseKey] + '_ConnString'

Anytime you want to add new database, just need to add new database name into `mgr:DatabaseKeys` and add new connection key for it.
For example:
```xml
<add key="mgr:DatabaseKeys"     value="MovieStore, InventoryDb, MyNewDatabase"/>
<add key="mgr:MyNewDatabase_ConnString"   value="ConnectionString_For_MyNewDatabase"/>
```
Of course, you also need a child folder for that database to store migration scripts in `Migrations` folder, like `InvenetoryDb` and `MovieStore` folders


### Migration Scripts
***ExtMigration Attributes:***

Right now we just support these attributes:
- Use `ExtMgrDataStructure` for `structure` or `data` migrations.
- Use `ExtMgrFunction` for `function` migrations.
- Use `ExtMgrStoredProcedureAndScript` for `stored procedure` or `t-sql scripts` migrations.

***Migration structure:***
> Note: You can place migration class in everywhere as long as it is placed under namespace of database.
> For example: **DatabaseMigrateRunner.Migrations.MovieStore**

***Order of migrations:***
DatabaseMigrateExt will executes migration scripts with bellow order:

- DataStructure (version start at 1000..., ex: 100020171021194001)
- Function (version start at 2000..., ex: 200020170922083001)
- StoredProcedureAndScript (version start at 2000..., ex: 300020190908032101)

### Note & Tips:
- Your sql scripts (can be strucuture/stored/funtion...) must be mark as 'Embedded Resource' - see bellow image :point_down:)
- DatabaseMigrateExt will find and show all invaild migration scripts when application starts, you should check them
- DatabaseMigrateExt will not execute invalid scripts

<img src="https://raw.githubusercontent.com/minhhungit/DatabaseMigrateExt/master/wiki/Images/embedded_resource.png" />

### Demo
<img src="https://raw.githubusercontent.com/minhhungit/DatabaseMigrateExt/master/wiki/Images/demo.png" />


***Sample script:*** 
> Note: System will just find migration scripts which used **ExtMigration Attributes** to apply

```csharp
using DatabaseMigrateExt;

namespace DatabaseMigrateRunner.Migrations.MovieStore
{
    [ExtMgrDataStructure(2017, 9, 22, 02, 08, 01)]
    public class SqlStructure_20170921_194001_inital_tables : Migration
    {
        public override void Up()
        {
            // do something here
        }
        ....
    }
}
```
You also can define author on attribute, like this:
```csharp
[ExtMgrDataStructure("Hung Vo", 2017, 9, 22, 02, 08, 01)]
```

### Run
```csharp
using DatabaseMigrateExt;

static void Main(string[] args)
{
    // Run migration with default settings
    ExtMigrationRunner
        .Initialize()
        .Process();

    Console.WriteLine("Completed!");
    Console.ReadKey();
}
```

## Thanks
<a href="https://www.jetbrains.com/"><img src="https://raw.githubusercontent.com/minhhungit/DatabaseMigrateExt/master/wiki/Images/jetbrains-128x128.png" /></a>
