@REM Crea una carpeta raíz y navega hasta ella
@REM cd B4


dotnet add B4.Logger package NLog
dotnet add B4.Logger package NLog.Extensions.Logging
dotnet add B4.Logger package NLog.Web.AspNetCore
dotnet add B4.Logger package Newtonsoft.Json

@REM dotnet add B4.Api package NLog
@REM dotnet add B4.Api package NLog.Extensions.Logging
@REM dotnet add B4.Api package NLog.Web.AspNetCore
dotnet add B4.Api package Newtonsoft.Json

@REM dotnet add B4.Data.MySQL package NLog
@REM dotnet add B4.Data.MySQL package NLog.Extensions.Logging
dotnet add B4.Data.MySQL package Newtonsoft.Json

@REM dotnet add B4.Data.PostgreSQL package NLog
@REM dotnet add B4.Data.PostgreSQL package NLog.Extensions.Logging
dotnet add B4.Data.PostgreSQL package Newtonsoft.Json

@REM dotnet add B4.Domain package NLog
@REM dotnet add B4.Domain package NLog.Extensions.Logging
dotnet add B4.Domain package Newtonsoft.Json

@REM dotnet add B4.Web package NLog
@REM dotnet add B4.Web package NLog.Extensions.Logging
dotnet add B4.Web package Newtonsoft.Json

@REM dotnet add B4.Tests package NLog
@REM dotnet add B4.Tests package NLog.Extensions.Logging
dotnet add B4.Tests package Newtonsoft.Json

@REM dotnet add B4.DBMigrations package NLog
@REM dotnet add B4.DBMigrations package NLog.Extensions.Logging
dotnet add B4.DBMigrations package Newtonsoft.Json

@REM dotnet add package WebMarkupMin.Core
@REM dotnet add package WebMarkupMin.AspNetCore6

@REM @REM otros paquetes
@REM dotnet add B4.Api package mapster
@REM dotnet add B4.Data package mapster
@REM dotnet add B4.Domain package mapster
@REM dotnet add B4.Tests package mapster
@REM dotnet add B4.Web package mapster

@REM --------
@REM Dependencias para B4.Data
@REM Microsoft.Data.Sqlite
@REM Dapper (para mapeo ligero) o SQLite-net-pcl (aunque aquí se usa Dapper en ejemplo)
@REM Microsoft.Extensions.DependencyInjection (para DI)
@REM dotnet add B4.Data package Microsoft.Extensions.DependencyInjection 
@REM dotnet add B4.Data package Microsoft.Data.Sqlite
@REM https://youtu.be/FuXx-N2-zoM
@REM dotnet add B4.Data package dbup-postgresql
@REM dotnet add B4.Data package dbup-mysql 
@REM dotnet add B4.Data package dbup-sqlserver 

dotnet add B4.Data.MySQL package MySql.Data
dotnet add B4.Data.MySQL package MySql.Connector.Core

dotnet add B4.Data.PostgreSQL package Npgsql

@REM Dependencias para B4.Api
@REM Microsoft.Extensions.DependencyInjection (para DI)

@REM Dependencias para B4.Tests
@REM dotnet add B4.Tests package Microsoft.Data.Sqlite
dotnet add B4.Tests package Dapper
dotnet add B4.Tests package Microsoft.Extensions.DependencyInjection 

@REM Dependencias para B4.DBMigrations
@REM Dapper + DBUp
dotnet add B4.DBMigrations package dbup-core
@REM dotnet add B4.DBMigrations package dbup-sqlite 
@REM dotnet add B4.DBMigrations package Microsoft.Data.Sqlite
dotnet add B4.DBMigrations package Dapper
dotnet add B4.DBMigrations package Microsoft.Extensions.DependencyInjection 

