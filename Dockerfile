# ===== 1) Build =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos todo el repo (solución completa)
COPY . .

# Publicamos la API
RUN dotnet publish ./B4.Api/B4.Api.csproj -c Release -o /app/publish

# ===== 2) Runtime =====
# ===== FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
# ===== WORKDIR /app
# =====EXPOSE 8080

# ASP.NET Core en contenedor debe escuchar en 8080
# =====ENV ASPNETCORE_URLS=http://+:8080

# =====COPY --from=build /app/publish .
# ===== ENTRYPOINT ["dotnet", "B4.Api.dll"]