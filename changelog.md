# Changelog

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
