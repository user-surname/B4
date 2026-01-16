# Changelog

## v0.20260116.1 - Slava

- B4.Api.Program.cs -> agregado servicio de autenticacion JWT
- B4.Api.Controllers -> Controller de autenticacion JWT AuthController: creado
- B4.Shared -> JwtService: creado
- B4.Models.Entities -> LoginRequest: creado
- B4.Tests -> EpigrafeIntegrationTests: creado

## v0.20251219.1 - Slava

- B4.Data.MySQL -> Nueva estructura de Repositories 
- B4.Data.Models -> Nueva estructura de entidades y Interfaces
- B4.Api -> condicional para conectarse a la bbdd a taves del Shared, inyeccion de dependencias y control de exepciones
- B4.Shared -> SharedConfig: classe para las cadenas de conexion

## v0.20251127.1 - Slava

- B4.Data.MySQL -> Readme: agregado
- B4.Api -> Cadenas de conexion MySQLConnetion y PostgresConnection en appsettings.json: agregado

## v0.20251126.2 - Slava

- B4.Data.MySQL -> DapperContext -> MySQLDapperContext
- B4.Api -> appsettings.json: DeafultConnection -> MySQLConnection

## v0.20251126.1 - Slava

- B4.Data.MySQL -> Data(...)Repository + Test + Intefice + Entity: creado
- B4.Data.MySQL -> StgData(...)Repository + Test + Intefice + Entity: creado

## v0.20251124.1 - Slava

- Migraacion a net8.0
	- Quite "<OutputType>Exe</OutputType>" de B4.Logger para que no de error
- Actualizacin de Nlog: NLog.Web no es compatible con .NET 8 -> Solucion: NLog.Extensions.Logging

## v0.20251120.1

- global.json
	- fichero config a nivel de solucion apra unificar el sdk a utilizar
	- agregado en carpeta: elementos de la solucion_

## v0.20251119.1 - Slava

- B4.Tests -> CiclosRepositoryTest: arreglado el problema de AUTO_INCREMENT
- B4.Data.MySQL -> Migracion de DbUpMigrator, Migrations y Scripts 

## v0.20251118.1 - Slava

- B4.Data.MySQL -> PlantTreeRepository + Interface + Test: creado
- B4.Data.MySQL -> PlantSubdivisionRepository + Interface + Test: creado
- B4.Data.MySQL -> PlantillasBotonesPasosTiposRepository + Interface + Test: creado
- B4.Data.MySQL -> PlantDivisionRepository + Interface + Test: creado
- B4.Data.MySQL -> PlantDivisionCompanyRepository + Interface + Test: creado
- B4.Data.MySQL -> PlantCurrencyRepository + Interface + Test: creado
- B4.Data.MySQL -> PlantCountryRepository + Interface + Test: creado
- B4.Data.MySQL -> PlantControllersRepository + Interface + Test: creado
- B4.Data.MySQL -> PlantCompanyRepository + Interface + Test: creado
- B4.Data.MySQL -> FasesRepository + Interface + Test: creado
- B4.Data.MySQL -> ControlPlantaRepository + Interface + Test: creado
- B4.Data.MySQL -> CiclosRepository + Interface + Test: creado
	- TODO: B4.Tests -> La tabla DE Ciclos tiene autoincrement, mirar solucion para test

## v0.20251117.1 - Slava

- B4.Tests -> EpigrafesRepositoryTest: creado
- B4.Data.MySQL -> ControlRepository + Test: creado			
	- TODO: B4.Model.Entities -> Revisar nombre de campo Epigrafe para que no sea igual que el de la tabla Epigrafes


## v0.20251111.2

- B4.Api
	- Program.cs agregamos servicios y configuraciones basicas
- B4.Api.Middleware.GlobalExceptionHandlerMiddleware: gestion de errores en respuesta HTTP
- B4.Models -> carpetas Modelos y DTOs
- B4.DataPostgreSQL
	- carpetas Repositories y Services
	- dependencias: Microsoft.Extensions.Caching.Memory
	- MemoryCacheService: pdte de encontrar ubicacion final del servicio

## v0.20251111.1

- B4.Models -> IRepository<T>
- B4.Models -> IUsuarioRepository<T> : IRepository<T>
- B4.DataPostgreSQL -> Dependencia con B4.Models
- B4.DataPostgreSQL -> UsuarioRepositorioPostgres
- B4.DataMySQL -> Dependencia con B4.Models
- B4.API -> acceso a UsuarioRepositorioPostgres
	- TODO: concretar si definir en API o en Domain
	- TODO: como centralizar la configuracion de TODO, cadenas de conexion, logger....o como compartir por servicio config de API con resto dependencias

- ELIMINADO
	- Domain -> IRepository<T>
	- Domain -> IUsuarioRepository<T> : IRepository<T>

## v0.20251111

- Domain -> IRepository<T>
- Domain -> IUsuarioRepository<T> : IRepository<T>
 
## v0.20251105

- Estructura de solucion
- git-cheat-sheet, pdf y enlace
- ajustes de dependencias: NLog,Newtonsoft.Json, postgreSQL, mySQL
- API demo (Weatherforecast) probada


