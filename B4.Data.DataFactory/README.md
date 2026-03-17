# B4.Data.DataFactory

## Que es

`B4.Data.DataFactory` es la capa comun que se ha introducido para centralizar el acceso a base de datos cuando la aplicacion trabaja con varios motores.

Su objetivo es que un repositorio no tenga que decidir directamente si debe usar MySQL o PostgreSQL. Esa decision se concentra en DataFactory.

## Que problema resuelve

En la solucion ya existen estas dos capas de datos:

- `B4.Data.MySQL`
- `B4.Data.PostgreSQL`

Ambas siguen siendo validas y todavia conviven con DataFactory. El problema que intenta resolver este proyecto es evitar que cada nuevo repositorio tenga que duplicarse por proveedor y registrarse de forma distinta.

DataFactory permite migrar repositorios poco a poco, manteniendo el sistema actual mientras se introduce una capa comun.

## Estructura del proyecto

- `Configuration`
  Contiene las opciones de configuracion que necesita DataFactory.

- `Connections`
  Contiene la factoria de conexiones que crea `MySqlConnection` o `NpgsqlConnection` segun el proveedor activo.

- `Extensions`
  Contiene el metodo `AddDataFactory(...)` para registrar DataFactory en `Program.cs`.

- `Providers`
  Contiene el proveedor que resuelve el bloque de queries correcto segun el valor de `bbdd`.

- `Queries/Common`
  Define las interfaces comunes de queries que consumen los repositorios migrados.

- `Queries/MySQL`
  Implementa las queries especificas para MySQL.

- `Queries/PostgreSQL`
  Implementa las queries especificas para PostgreSQL.

- `Repositories`
  Contiene los repositorios que ya usan DataFactory.

## Piezas principales

### ConnectionFactory

Las piezas principales de conexion son:

- `DataFactoryOptions`
- `IDbConnectionFactory`
- `DbConnectionFactory`

`DbConnectionFactory` lee la configuracion y crea la conexion adecuada:

- `MySqlConnection` si `bbdd` vale `MySQL`
- `NpgsqlConnection` si `bbdd` vale `PostgreSQL`

Las cadenas de conexion que usa actualmente son:

- `ConnectionStrings:MySQLConnectionB4Data`
- `ConnectionStrings:PostgresConnectionB4Data`

### QueryProvider

Las piezas principales del proveedor de queries son:

- `IDataQueryProvider`
- `DataQueryProvider`

Su responsabilidad es entregar a cada repositorio el bloque de SQL correcto segun el proveedor activo. El repositorio solo pide sus queries; no decide por si mismo si esta trabajando con MySQL o PostgreSQL.

### Queries por proveedor

Actualmente existen queries por proveedor para:

- `Usuario`
- `ControlPlanta`
- `DataActuals`
- `DataActualsBw`

Cada grupo se organiza con:

- una interfaz en `Queries/Common`
- una implementacion MySQL
- una implementacion PostgreSQL

## Repositorios migrados a DataFactory

Actualmente los repositorios migrados a DataFactory son:

- `ControlPlantaRepository`
- `DataActualsRepository`
- `DataActualsBwRepository`

Estos repositorios ya utilizan:

- `IDbConnectionFactory`
- `IDataQueryProvider`
- queries especificas por proveedor
- `Dapper` para ejecutar el SQL

Con este patron, el repositorio:

1. pide una conexion a `IDbConnectionFactory`,
2. pide sus queries a `IDataQueryProvider`,
3. ejecuta el SQL con `Dapper`.

El resto de repositorios siguen usando todavia las implementaciones antiguas en:

- `B4.Data.MySQL`
- `B4.Data.PostgreSQL`

## Flujo actual

El flujo general de los repositorios migrados es este:

`Controller -> Service -> Repository -> DataFactory -> Base de datos`

Mas en detalle:

1. El controller recibe la peticion HTTP.
2. El service llama a la interfaz de repositorio.
3. El repositorio migrado usa `IDbConnectionFactory` para abrir la conexion correcta.
4. El repositorio pide a `IDataQueryProvider` las queries de su entidad.
5. `Dapper` ejecuta el SQL contra MySQL o PostgreSQL segun `bbdd`.

## Papel de Program.cs

`Program.cs` ya integra DataFactory en dos niveles:

1. Registro base de DataFactory:
   `builder.Services.AddDataFactory(builder.Configuration);`

2. Registro explicito de los repositorios ya migrados para que la API consuma sus versiones de DataFactory.

Esto significa que la API ya usa DataFactory en repositorios concretos, pero el resto del sistema sigue funcionando con el esquema antiguo.

## Que parte del proyecto ya usa DataFactory

Ahora mismo DataFactory ya se esta usando en:

- `ControlPlantaRepository`
- `DataActualsRepository`
- `DataActualsBwRepository`
- el registro base de DataFactory en `B4.Api/Program.cs`

## Que parte sigue usando el sistema antiguo

La mayor parte de la capa de datos sigue usando todavia:

- repositorios de `B4.Data.MySQL`
- repositorios de `B4.Data.PostgreSQL`
- la seleccion manual de registros en `Program.cs`
- los contextos `MySQLDapperContext` y `PostgreSQLDapperContext`

DataFactory convive con ese sistema; no lo ha reemplazado completo todavia.

## Como anadir un nuevo proveedor

Si en el futuro se quiere anadir otro proveedor, por ejemplo SQL Server, el camino previsto es:

1. anadir su cadena de conexion en configuracion,
2. ampliar `DataFactoryOptions` si hace falta,
3. implementar la conexion en `DbConnectionFactory`,
4. crear las queries especificas del nuevo proveedor,
5. ampliar `DataQueryProvider`,
6. migrar repositorios concretos para que usen ese nuevo bloque de queries.

## Que se ha validado ya

Con los repositorios migrados actuales ya se ha validado que:

- la API puede arrancar con DataFactory integrado,
- un mismo repositorio puede funcionar con MySQL o PostgreSQL,
- el proveedor se resuelve por configuracion,
- DataFactory puede convivir con los repositorios antiguos,
- el patron sirve tanto para lecturas simples como para CRUD y consultas especificas.

## Estado actual

### Que esta implementado

- `DataFactoryOptions`
- `IDbConnectionFactory`
- `DbConnectionFactory`
- `IDataQueryProvider`
- `DataQueryProvider`
- queries de `Usuario`
- queries de `ControlPlanta`
- queries de `DataActuals`
- queries de `DataActualsBw`
- `ControlPlantaRepository`
- `DataActualsRepository`
- `DataActualsBwRepository`
- integracion base en `B4.Api/Program.cs`

### Que esta probado

- `B4.Data.DataFactory` compila
- `B4.Api` compila con DataFactory integrado
- los tres repositorios migrados ya estan registrados en la API

### Que queda pendiente

- seguir migrando repositorios de forma incremental
- reducir la seleccion manual de registros en `Program.cs`
- validar mas flujos funcionales contra base de datos real
- decidir si a futuro se simplifican o se retiran los repositorios antiguos
