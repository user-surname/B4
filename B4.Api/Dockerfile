# ===============================
# ETAPA 1: BUILD (Compilación)
# ===============================

# Usa la imagen oficial de .NET SDK 8.0 para poder compilar el proyecto
# Esta imagen contiene herramientas como dotnet restore, build y publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Define el directorio de trabajo dentro del contenedor
# A partir de ahora todos los comandos se ejecutarán en /app
WORKDIR /app


# ---------- COPIA DE PROYECTOS ----------

# Copia cada proyecto de la solución al contenedor
# Esto es necesario porque la API depende de estos proyectos
# (Models, Domain, Helpers, etc.)

COPY B4.Api ./B4.Api
COPY B4.Models ./B4.Models
COPY B4.Domain ./B4.Domain
COPY B4.Helpers ./B4.Helpers
COPY B4.Shared ./B4.Shared
COPY B4.Data.MySQL ./B4.Data.MySQL
COPY B4.Data.PostgreSQL ./B4.Data.PostgreSQL


# ---------- RESTORE DE DEPENDENCIAS ----------

# Descarga todas las dependencias NuGet necesarias
# (las definidas en los .csproj)
# Esto es equivalente a ejecutar "dotnet restore" en local
RUN dotnet restore B4.Api/B4.Api.csproj


# ---------- PUBLICACIÓN DE LA API ----------

# Compila la API en modo Release
# y genera los archivos listos para ejecutar en la carpeta /app/out
#
# Esto incluye:
# - DLLs compiladas
# - dependencias
# - runtime config
RUN dotnet publish B4.Api/B4.Api.csproj -c Release -o out



# ===============================
# ETAPA 2: RUNTIME (Ejecución)
# ===============================

# Usa una imagen más ligera que solo contiene el runtime de ASP.NET
# (no incluye herramientas de compilación)
# Esto reduce mucho el tamaño final de la imagen
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Directorio donde se ejecutará la aplicación
WORKDIR /app


# Copia los archivos publicados desde la etapa de build
# /app/out de la etapa anterior -> /app del contenedor final
COPY --from=build /app/out .


# ---------- CONFIGURACIÓN DE LA API ----------

# Indica que el contenedor usará el puerto 5000
# (esto es solo informativo para Docker)
EXPOSE 5000

# Configura ASP.NET para escuchar en todas las interfaces del contenedor
# (+ significa 0.0.0.0)
ENV ASPNETCORE_URLS=http://+:5000

# Define el entorno de ejecución
# Development permite Swagger, logs detallados, etc.
ENV ASPNETCORE_ENVIRONMENT=Development


# ---------- COMANDO DE INICIO ----------

# Cuando el contenedor se inicie ejecutará este comando
# Esto arranca la API ASP.NET
ENTRYPOINT ["dotnet", "B4.Api.dll"]