# Changelog

## v0.20260211.1 - Slava

- B4 -> Implementacion de la tabla Control y Tests
- B4.Api.Middleware -> MappingProfile creado
- B4.Api.Controllers -> Automaping para controllers creado
- B4.Tests -> Automaping para Tests creado

## v0.20260206.1 - Slava

- B4.Domain -> Services para Controllers creados
- B4.ServiceInterfaces -> Interfaces para Services creadas
- B4.Api.Controllers -> Controllers utilizan Services

## v0.20260206.1 Victor

- B4.Api.LkPruebas
  - Posible herencia para los controllers Lk
- B4.Data.Postgresql.Services
  - Implementacion del MemoryCache y CacheKeys

## v0.20260204.1 - Slava

- B4.Api.Controllers -> Controllers LK creados
- B4.Dto -> Dtos para LK creados
- B4.Tests -> Tests para controllers LK creados

## v0.20260128.1 - Slava

- B4.Models.Entities -> LoginRequest y Usuario creado
- B4.Shared -> JwtService creado
- B4.Data.MySQL -> UsuarioRepository y IUsuarioRepository creado
- B4.Api.Controllers -> AuthController creado


## v0.20260116.1 Victor

- B4.Api.MiddleWare
  - Metadata para envolver los DTO
- B4.Api.Dto
  - Modificacion de Dto para POST y GET de DataActuals

## v0.20260116.1 - Slava

- B4.Api.Program.cs -> agregado servicio de autenticacion JWT
- B4.Api.Controllers -> Controller de autenticacion JWT AuthController: creado
- B4.Shared -> JwtService: creado
- B4.Models.Entities -> LoginRequest: creado
- B4.Tests -> EpigrafeIntegrationTests: creado

## v0.20251219.1 - Slava

- B4.Api.Controllers
  - Solucionar problemas de compilacion
- B4.Api.Dto
  - Solucionar problemas de compilacion
- B4.Api.Test
  - Creacion de un Test para el Controller

## v0.20260115.1 Victor

- B4.Api.Controllers
  - Filtrar als busquedas de DataActualsController
- B4.Api.Dto
  - Modificacion de Dto para POST y GET de DataActuals
- B4.Api.Middleware
  - Creacion de un Middleware para envolver lso metadatos

## v0.20260114.1 Victor

- B4.Api.Controllers
  - Creacion de DataActualsController
  - Herencia de ControllerBase
- B4.Api.Dto
  - Creacion de Dto para POST de DataActuals
  - Creacion de Dto para GET de DataActuals

## v0.20251217.2 Victor

- B4.Models
  - Refactorització de les classes Data financeres
  - Afegits mètodes comuns per al càlcul de:
    - Checksum
    - IsZero
    - Version
  
- B4.Data.PostgreSQL
  - Actualització dels repositoris financers per aplicar automàticament:
    - Recalcul de Checksum, IsZero i Version en CREATE i UPDATE
    - Gestió automàtica de CreatedAt i UpdatedAt
  - Refactorització dels repositoris LK:
    - Centralització de CreatedAt, UpdatedAt i IsActive a BaseLkRepository
    - Simplificació dels repositoris concrets 

## v0.20251217.1 Victor
- B4.Models
	- Optimizar las clases Data financieras
- B4.Data.PostgreSQL
	- Creacion de la clase PostgreSQLDapperContext.cs
	- Cambiar l'estructura de les carpetas


## v0.20251209.1 Victor
- B4.Models
	- Creacion de la clase abstracta DataBaseFinanciera
	- Optimizar las clases Data financieras
- B4.Data.PostgreSQL
	- Creacion de la clase abstracta BaseFinancialRepository
	- Optimizar los repositorios 


## v0.20251202.1 Victor
- B4.Models
	- Creacion de la clase abstracta DataBase
	- Agregar los nuevos campos a las tablas Data 

## v0.20251127.1 Victor
- B4.DataPostgreSQL
	- Adaptar los repositorios Data
- B4.Shared
	- Creacion de la carpeta Shared
	- Migración parcial de Logger dentro de Shared
	TODO: Falta ajustar un par de cosas
## v0.20251126.1 Victor
- B4.DataPostgreSQL
	- Adaptar repositorios LK_xxx y control, implementados por Slava
	- Integrar dbmigrations en B4.Data.PostgreSQL
- B4.Test
	- Implementación de los Test

## v0.20251124.1 Victor

- B4.DataPostgreSQL
	- Implementación de los repositorios en Data
- B4.Test
	- Implementación de los Test
- B4.Models
	- Implementación de todas las Interfaces

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
