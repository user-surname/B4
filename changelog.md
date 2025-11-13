# Changelog

## v0.20251111.1

- B4.Models -> IRepository<T>
- B4.Models -> IUsuarioRepository<T> : IRepository<T>
- B4.DataPostgres -> Dependencia con B4.Models
- B4.DataPostgres -> UsuarioRepositorioPostgres
- B4.DataMySQL -> Dependencia con B4.Models
- B4.API -> acceso a UsuarioRepositorioPostgres
	- TO-DO: concretar si definir en API o en Domain
	- TO-DO: como centralizar la configuracion de TODO, cadenas de conexion, logger....o como compartir por servicio config de API con resto dependencias

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
