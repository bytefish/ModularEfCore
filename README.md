# ModularEfCore #

This repository shows a simple way to build modular applications using Entity Framework Core 2.1. Instead of building a huge ``DbContext`` for the Application, it is injecting mappings registered in the application root.

## Creating the Database ##

First adjust the following Connection String the ``appsettings.json`` of the ``ModularEfCore.Example.Web`` project:

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=SampleDB;Trusted_Connection=True;"
  }
}
```

To create the Database run the following from the Package Manager Console:

```
PM> Update-Database
```