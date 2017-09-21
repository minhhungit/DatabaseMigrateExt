# DatabaseMigrateExt
A new way, new rule to work with MSSQL database version use fluent migrator

## Installation
>Install-Package DatabaseMigrateExt

## Sample Project

See [Sample Project](https://github.com/minhhungit/DatabaseMigrateExt/tree/master/Tutorials/DatabaseMigrateRunner) at [https://github.com/minhhungit/DatabaseMigrateExt/tree/master/Tutorials/DatabaseMigrateRunner](https://github.com/minhhungit/DatabaseMigrateExt/tree/master/Tutorials/DatabaseMigrateRunner)

## Config
```xml
<add key="mgr:DatabaseKeys" value="MovieStore, InventoryDb"/>
<add key="mgr:RootNamespace" value="DatabaseMigrateRunner.Migrations"/>
<add key="mgr:AvailableLevels" value="1000, 2000, 3000"/>

<add key="mgr:MovieStore_ConnString" value="Data Source=JINPC\MSSQL2014;Initial Catalog=FirstDb;User ID=fake;Password=fake;Connection Timeout=6000;"/>
<add key="mgr:InventoryDb_ConnString" value="Data Source=JINPC\MSSQL2014;Initial Catalog=SecondDb;User ID=fake;Password=fake;Connection Timeout=6000;"/>
```


## Run
```c#
using DatabaseMigrateExt;
...
var databaseKeys = ConfigurationManager.AppSettings["mgr:DatabaseKeys"].Split(',').Select(p => p.Trim()).ToList();
var availableLevels = ConfigurationManager.AppSettings["mgr:AvailableLevels"].Split(',').Select(x => (DatabaseScriptType)int.Parse(x)).ToList();

var setting = new MigrationSetting
{
	DatabaseKeys = databaseKeys,
	AvailableLevels = availableLevels,
	MigrationAssembly = Assembly.GetExecutingAssembly()
};

Console.WriteLine("Start...");
MigrationManager.Instance.Run(setting);
Console.WriteLine("Completed!");

Console.ReadKey();
```
