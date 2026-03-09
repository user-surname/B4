FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar todos los proyectos
COPY B4.Api ./B4.Api
COPY B4.Models ./B4.Models
COPY B4.Domain ./B4.Domain
COPY B4.Helpers ./B4.Helpers
COPY B4.Shared ./B4.Shared
COPY B4.Data.MySQL ./B4.Data.MySQL
COPY B4.Data.PostgreSQL ./B4.Data.PostgreSQL

# Restaurar dependencias
RUN dotnet restore B4.Api/B4.Api.csproj

# Publicar backend
RUN dotnet publish B4.Api/B4.Api.csproj -c Release -o out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "B4.Api.dll"]