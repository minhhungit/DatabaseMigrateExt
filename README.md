# <img rel="https://raw.githubusercontent.com/minhhungit/DatabaseMigrateExt/master/develop/Icon/logo.png" src="https://raw.githubusercontent.com/minhhungit/DatabaseMigrateExt/develop/wiki/Icon/logo.png" width="32" /> DatabaseMigrateExt <a href="https://www.nuget.org/packages/DatabaseMigrateExt/"><img src="https://img.shields.io/nuget/v/DatabaseMigrateExt.svg?style=flat" /></a> [![Codacy Badge](https://api.codacy.com/project/badge/Grade/d1b3c86714594b6894bb9382c3f85f33)](https://www.codacy.com/app/it.minhhung/DatabaseMigrateExt?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=minhhungit/DatabaseMigrateExt&amp;utm_campaign=Badge_Grade) [![CodeFactor](https://www.codefactor.io/repository/github/minhhungit/databasemigrateext/badge)](https://www.codefactor.io/repository/github/minhhungit/databasemigrateext)

A new way, new rule to work with MSSQL database version use [fluent migrator](https://github.com/fluentmigrator/fluentmigrator)
- [https://minhhungit.github.io/DatabaseMigrateExt](https://minhhungit.github.io/DatabaseMigrateExt)

### Installation
>Install-Package DatabaseMigrateExt

### Overview ?
When we use `Fluent Migrator` or even `EF Migration`, it will be very hard to check history of a sql function/stored procedure, 
DatabaseMigrateExt can help you resolve the problem using ChangeScript/RefScript pattern. 

That mean sql function/ stored procedure will be keeped in each file separately (ref-script) 
and every times we want to do a change we will create a migration classs with a version number (change-script)
and link it to ref-script, for example:
- We have a ref-script stored proc `dbo.GetProducts`
- Migration 01 - update `dbo.GetProducts` and commit
- Migration 02 - update `dbo.GetProducts` and commit again
- ...
By using source control like Git we know the changes history of stored `dbo.GetProducts`

Next problem, using RefScript/ChangeScript pattern means your stored `dbo.GetProducts` is only one file, so what happen if some day we want to re-run all migrations for a new empty database. 
Normally this will cause a lot errors because file `dbo.GetProducts` is latest state, every times migrator runs it will apply the latest state of the stored procedure, 
in above sample, `Migration 01` and `Migration 02` will call one file `dbo.GetProducts`.
There is a high possibility that migratior can not execute your stored procedure because some tables have not created yet.

To hanlde it, we will need to classify type of change-scripts by using `ExtMigration Attributes`, all migrations relate to schema like Create Database, Alter Column, Created Index, Function... should be run 
before Stored Procedure alter migration. Then when you re-run, you will have newes tables, newest schema and Stored Procedure will work.

So that is what DatabaseMigrateExt does.

### Sample Project

See [Sample Project](https://github.com/minhhungit/DatabaseMigrateExt/tree/master/Samples) at [https://github.com/minhhungit/DatabaseMigrateExt/tree/master/Samples](https://github.com/minhhungit/DatabaseMigrateExt/tree/master/Samples)


### Config

```xml
<add key="mgr:DatabaseKeys"     value="MovieStore"/>
<add key="mgr:RootNamespace"    value="DatabaseMigrateRunner.Migrations"/>

<add key="mgr:MovieStore_ConnString"    value="Your_ConnectionString_For_MovieStore_At_Here"/>
```

*Note*: 
> mgr:MovieStore_ConnString = 'mgr:' + [DatabaseKey] + '_ConnString'

Anytime you want to add new database, just need to add new database name into `mgr:DatabaseKeys` and add new connection key for it.
Make sure that name of DatabaseKey setting must match with ConnString setting.

For example:
```xml
<add key="mgr:DatabaseKeys"		value="MovieStore, InventoryDb, MyNewDatabase"/>
<add key="mgr:MovieStore_ConnString"	value="ConnectionString_For_MovieStore"/>
<add key="mgr:InventoryDb_ConnString"   value="ConnectionString_For_InventoryDb"/>
<add key="mgr:MyNewDatabase_ConnString"	value="ConnectionString_For_MyNewDatabase"/>
```
Of course, you also need a child folder for that database to store migration scripts in `Migrations` folder, like `InvenetoryDb` and `MovieStore` folders (match name with DatabaseKey)


### Migration Scripts
***ExtMigration Attributes:***

There are 3 attributes:
- Use `ExtMgrDataStructure` for `structure` or `data` migrations.
- Use `ExtMgrFunction` for `function` migrations.
- Use `ExtMgrStoredProcedureAndScript` for `stored procedure` or `t-sql scripts` migrations.

> Note: System will just find migration scripts which used **ExtMigration Attributes** to apply, everything else will be skipped.

***Migration structure:***
You can place migration class in everywhere as long as it is placed under namespace of database.
> For example: **DatabaseMigrateRunner.Migrations.MovieStore**

But, remember that your ref-scripts must be placed fixed in folder @RefScript inside DatabaseKey folder.
You can change name of children folders like DataAndStructure/Function/Stored like anything you want with these settings:
- mgr:SqlArchitectureRefScriptNamespace
- mgr:SqlFunctionRefScriptNamespace
- mgr:SqlStoredRefScriptNamespace

***Order of migrations:***
DatabaseMigrateExt will executes migration scripts with bellow order:

- DataStructure (version start at 1000..., ex: 100020171021194001)
- Function (version start at 2000..., ex: 200020170922083001)
- StoredProcedureAndScript (version start at 2000..., ex: 300020190908032101)

<img src="https://raw.githubusercontent.com/minhhungit/DatabaseMigrateExt/master/wiki/Images/db-versions.png" />

### Ref-Script And Change-Script
<img src="https://raw.githubusercontent.com/minhhungit/DatabaseMigrateExt/master/wiki/Images/ref-and-change-script.png" />

### Note & Tips:
- Your sql scripts (can be strucuture/stored/funtion...) must be marked as 'Embedded Resource' - see bellow image :point_down:)
- DatabaseMigrateExt will find and show all invaild migration scripts when application starts, you should check them
- DatabaseMigrateExt will not execute invalid scripts

<img src="https://raw.githubusercontent.com/minhhungit/DatabaseMigrateExt/master/wiki/Images/embedded_resource.png" />

### Demo
<img src="https://raw.githubusercontent.com/minhhungit/DatabaseMigrateExt/master/wiki/Images/demo-result.png" />


***Sample script:*** 

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
