
@REM Este script opcionalmente puede crear las carpetas de proyecto
@REM Este script puede necesitar actualizaciones de dependencias con el tiempo.


@REM /B4
@REM │
@REM ├── B4.Logger      (.NET 8)
@REM ├── B4.Api         (.NET 8)
@REM ├── B4.Data.MySQL  (.NET 8)
@REM ├── B4.Data.PostgreSQL (.NET 8)
@REM ├── B4.Domain      (.NET 8)
@REM ├── B4.Web         (.NET 8)   # si agregas frontend web
@REM ├── B4.Tests       (.NET 8)


@REM Crea una carpeta raíz y navega hasta ella
@REM mkdir B4
@REM cd B4

@REM Crea la solución principal
dotnet new sln -n B4

@REM Crea cada uno de los proyectos
dotnet new classlib    -n B4.Helpers -f net8.0
dotnet new classlib    -n B4.Logger  -f net8.0
dotnet new classlib    -n B4.Data.MySQL -f net8.0
dotnet new classlib    -n B4.Data.PostgreSQL -f net8.0
dotnet new classlib    -n B4.Models -f net8.0
dotnet new classlib    -n B4.Domain -f net8.0
dotnet new classlib    -n B4.DBMigrations -f net8.0
dotnet new webapi      -n B4.Api -f net8.0
dotnet new mvc         -n B4.Web -f net8.0
dotnet new xunit       -n B4.Tests -f net8.0

@REM Agrega proyectos a la solución
dotnet sln add B4.Logger/B4.Logger.csproj
dotnet sln add B4.Web/B4.Web.csproj
dotnet sln add B4.Api/B4.Api.csproj
dotnet sln add B4.Domain/B4.Domain.csproj
dotnet sln add B4.Data.MySQL/B4.Data.MySQL.csproj
dotnet sln add B4.Data.PostgreSQL/B4.Data.PostgreSQL.csproj
dotnet sln add B4.Models/B4.Models.csproj
dotnet sln add B4.Helpers/B4.Helpers.csproj
dotnet sln add B4.DBMigrations/B4.DBMigrations.csproj
dotnet sln add B4.Tests/B4.Tests.csproj

@REM # Crear el proyecto Excel Add-in manualmente en Visual Studio:
@REM # - Abrir Visual Studio
@REM # - Nuevo proyecto > Complemento de Excel VSTO
@REM # - Seleccionar .NET Framework 4.7.2 o superior
@REM # - Nombrar proyecto como B4.ExcelAddin
@REM # - Guardar en la carpeta B4

@REM dotnet sln add B4.ExcelAddin/B4.ExcelAddin.csproj

@REM Añade referencias necesarias
dotnet add B4.Api reference B4.Logger
dotnet add B4.Api reference B4.Helpers
dotnet add B4.Api reference B4.Domain
dotnet add B4.Api reference B4.Models

dotnet add B4.Domain reference B4.Logger
dotnet add B4.Domain reference B4.Helpers
dotnet add B4.Domain reference B4.Data.MySQL
dotnet add B4.Domain reference B4.Data.PostgreSQL
dotnet add B4.Domain reference B4.Models

dotnet add B4.Web reference B4.Logger
dotnet add B4.Web reference B4.Helpers
dotnet add B4.Web reference B4.Domain
@REM dotnet add B4.Web reference B4.Models

dotnet add B4.Tests reference B4.Logger
dotnet add B4.Tests reference B4.Helpers
dotnet add B4.Tests reference B4.Api
dotnet add B4.Tests reference B4.Domain
dotnet add B4.Tests reference B4.Data.MySQL
dotnet add B4.Tests reference B4.Data.PostgreSQL
dotnet add B4.Tests reference B4.Models

dotnet add B4.Data.MySQL reference B4.Logger
dotnet add B4.Data.MySQL reference B4.Models


dotnet add B4.Data.PostgreSQL reference B4.Logger
dotnet add B4.Data.PostgreSQL reference B4.Models

@REM Refeencias para proyecto migrations
dotnet add B4.DBMigrations reference B4.Logger
dotnet add B4.DBMigrations reference B4.Helpers
dotnet add B4.DBMigrations reference B4.Data.MySQL
dotnet add B4.DBMigrations reference B4.Data.PostgreSQL    
@REM dotnet add B4.DBMigrations reference B4.Helpers

@REM dependencias de paquetes NuGet
dotnet add B4.Logger package NLog
dotnet add B4.Logger package NLog.Extensions.Logging
dotnet add B4.Logger package NLog.Web.AspNetCore
dotnet add B4.Logger package Newtonsoft.Json

dotnet add B4.Data.MySQL package Newtonsoft.Json
dotnet add B4.Data.MySQL package MySql.Data
dotnet add B4.Data.MySQL package MySql.Connector.Core

dotnet add B4.Data.PostgreSQL package Newtonsoft.Json
dotnet add B4.Data.PostgreSQL package Npgsql

dotnet add B4.Api package Newtonsoft.Json
dotnet add B4.Models package Newtonsoft.Json
dotnet add B4.Domain package Newtonsoft.Json
dotnet add B4.Web package Newtonsoft.Json
dotnet add B4.Tests package Newtonsoft.Json
dotnet add B4.DBMigrations package Newtonsoft.Json



@REM Crea una carpeta raíz y navega hasta ella
@REM cd B4

dotnet add B4.Logger package NLog
dotnet add B4.Logger package NLog.Extensions.Logging
dotnet add B4.Logger package NLog.Web.AspNetCore
dotnet add B4.Logger package Newtonsoft.Json

dotnet add B4.Api package Newtonsoft.Json
dotnet add B4.Data.MySQL package Newtonsoft.Json
dotnet add B4.Data.PostgreSQL package Newtonsoft.Json
dotnet add B4.Domain package Newtonsoft.Json

dotnet add B4.Web package Newtonsoft.Json

dotnet add B4.Tests package Newtonsoft.Json

dotnet add B4.DBMigrations package Newtonsoft.Json

@REM dotnet add B4.Data package Microsoft.Extensions.DependencyInjection 

@REM Dependencias para B4.DBMigrations
@REM Dapper + DBUp
@REM dotnet add B4.DBMigrations package Microsoft.Data.Sqlite
dotnet add B4.DBMigrations package dbup-core
dotnet add B4.DBMigrations package dbup-postgresql
dotnet add B4.DBMigrations package dbup-mysql 
@REM dotnet add B4.DBMigrations package dbup-sqlserver 
@REM dotnet add B4.DBMigrations package dbup-sqlite 
dotnet add B4.DBMigrations package Dapper
dotnet add B4.DBMigrations package Microsoft.Extensions.DependencyInjection 

dotnet add B4.Data.MySQL package MySql.Data
dotnet add B4.Data.MySQL package MySql.Connector.Core

dotnet add B4.Data.PostgreSQL package Npgsql

@REM Dependencias para B4.Tests
dotnet add B4.Tests package Dapper
dotnet add B4.Tests package Microsoft.Extensions.DependencyInjection 



@REM dotnet restore
