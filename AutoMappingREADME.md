# MappingProfile

## Descripción

`MappingProfile` es una clase que hereda de `Profile` (AutoMapper) y centraliza toda la configuración de mapeo entre:

-   **Entidades del modelo (Models)**
    
-   **DTOs de salida (`GetDto`)**
    
-   **DTOs de entrada (`PostDto`)**
    

Permite convertir objetos automáticamente sin escribir código manual propiedad por propiedad.

* * *

## Funcionamiento

Dentro del constructor se definen reglas de transformación usando:

`CreateMap<Origen, Destino>();`

AutoMapper copiará automáticamente las propiedades que tengan:

-   El mismo nombre
    
-   El mismo tipo
    

* * *

## Modelo ➜ DTO (Get)

Ejemplo:

`CreateMap<LkCiclos, CiclosGetDto>();`

Permite transformar una entidad obtenida de base de datos en un DTO para devolver en la API:

`var dto = _mapper.Map<CiclosGetDto>(entity);`

* * *

## DTO (Post) ➜ Modelo

Ejemplo:

`CreateMap<CiclosPostDto, LkCiclos>();`

Permite convertir un objeto recibido en un `POST` en una entidad lista para persistir:

`var entity = _mapper.Map<LkCiclos>(dto);`

* * *

## Registro en la aplicación

Se registra en `Program.cs`:

`builder.Services.AddAutoMapper(typeof(MappingProfile));`

Y luego se inyecta `IMapper` en controladores o servicios:

`private readonly IMapper _mapper;`

* * *

## Objetivo

-   Evitar mapeo manual repetitivo
    
-   Mantener controladores limpios
    
-   Centralizar la lógica de transformación
    
-   Reducir errores humanos
    

* * *

## Resumen

`MappingProfile` define todas las reglas de conversión entre entidades y DTOs, permitiendo que AutoMapper realice las transformaciones automáticamente en toda la aplicación.