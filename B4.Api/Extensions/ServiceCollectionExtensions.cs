// ============================================================
// ServiceCollectionExtensions.cs — Registro de servicios en el DI
// Centraliza toda la configuración del contenedor de dependencias
// dividiéndola en métodos de extensión por responsabilidad.
// ============================================================

using B4.Api.Middleware;
using B4.Data.DataFactory.Extensions;
using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Data.PostgreSQL.Services;
using B4.Domain.Services;
using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces;
using B4.Models.RepositoryInterfaces.DataInterfaces;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using B4.Models.ServiceInterfaces;
using B4.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace B4.Api.Extensions;

public static class ServiceCollectionExtensions
{
    // -------------------------------------------------------
    // Registra los servicios transversales de infraestructura
    // de la API: compresión, caché, controladores, versionado,
    // Swagger, CORS y servicios de seguridad base.
    // -------------------------------------------------------
    public static IServiceCollection AddB4ApiCore(this IServiceCollection services)
    {
        // Registra el middleware global de excepciones como Transient:
        // se crea una instancia nueva por cada vez que es invocado.
        services.AddTransient<GlobalExceptionHandlerMiddleware>();

        // Configura la compresión GZIP al nivel más agresivo (menor tamaño, mayor CPU).
        // Se aplica también sobre HTTPS para reducir el ancho de banda en producción.
        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.SmallestSize;
        });
        services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
            options.EnableForHttps = true;
        });

        // Registra la caché en memoria del proceso (IMemoryCache).
        // IMemoryCacheService es el wrapper propio de B4 sobre IMemoryCache.
        services.AddMemoryCache();
        services.AddScoped<IMemoryCacheService, MemoryCacheService>();

        // Registra los controladores MVC y fuerza camelCase en las respuestas JSON,
        // alineando la serialización con las convenciones del frontend.
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        services.AddEndpointsApiExplorer();

        // Habilita el versionado de la API por URL (ej: /api/v1/...).
        // Si el cliente no especifica versión, se asume la v1.0 por defecto.
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true; // Incluye las versiones disponibles en las cabeceras de respuesta
        });
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";       // Formato: v1, v2, etc.
            options.SubstituteApiVersionInUrl = true; // Sustituye {version} en la URL del grupo de Swagger
        });

        // Configura Swagger/OpenAPI con soporte para autenticación JWT:
        // añade el botón "Authorize" en la UI para enviar el token Bearer en las peticiones.
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Ingrese 'Bearer {token}'"
            });

            // Hace que todos los endpoints de Swagger requieran el token Bearer por defecto
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // Registra las opciones de Swagger por versión (genera un documento swagger.json por cada versión de la API)
        services.ConfigureOptions<ConfigureSwaggerOptions>();

        // IPasswordHasherService es Singleton porque no tiene estado mutable:
        // la misma instancia se reutiliza durante toda la vida de la aplicación.
        services.AddSingleton<IPasswordHasherService, PasswordHasherService>();

        // JwtService se registra como Scoped porque su ciclo de vida
        // está ligado a la petición HTTP (puede necesitar datos del contexto).
        services.AddScoped<JwtService>();

        // Política CORS permisiva: permite cualquier origen, cabecera y método HTTP.
        // Pensada para entornos de desarrollo o APIs internas; revisar para producción.
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        return services;
    }

    // -------------------------------------------------------
    // Registra todos los servicios de dominio/negocio de B4.
    // Cada servicio encapsula la lógica de su entidad y se
    // inyecta a través de su interfaz correspondiente (Scoped:
    // una instancia por petición HTTP).
    // -------------------------------------------------------
    public static IServiceCollection AddB4DomainServices(this IServiceCollection services)
    {
        // --- Servicios de estructura / lookup ---
        services.AddScoped<ICiclosService, CiclosService>();
        services.AddScoped<IEpigrafeService, EpigrafeService>();
        services.AddScoped<IFasesService, FasesService>();
        services.AddScoped<IPlantCompanyService, PlantCompanyService>();
        services.AddScoped<IPlantControllersService, PlantControllersService>();
        services.AddScoped<IPlantCountryService, PlantCountryService>();
        services.AddScoped<IPlantCurrencyService, PlantCurrencyService>();
        services.AddScoped<IPlantDivisionCompanyService, PlantDivisionCompanyService>();
        services.AddScoped<IPlantDivisionService, PlantDivisionService>();
        services.AddScoped<IPlantillasBotonesPasosTiposService, PlantillasBotonesPasosTiposService>();
        services.AddScoped<IPlantSubdivisionService, PlantSubdivisionService>();
        services.AddScoped<IPlantTreeService, PlantTreeService>();
        services.AddScoped<IControlService, ControlService>();
        services.AddScoped<IControlPlantaService, ControlPlantaService>();

        // --- Servicios de datos financieros ---
        services.AddScoped<IDataBudgetService, DataBudgetService>();
        services.AddScoped<IDataForecastService, DataForecastService>();
        services.AddScoped<IDataComentariosService, DataComentariosService>();
        services.AddScoped<ILogActividadService, LogActividadService>();
        services.AddScoped<IDataTipoCambioService, DataTipoCambioService>();

        // --- Servicios de bridges (puentes de variación entre periodos) ---
        services.AddScoped<IDataBridgesFyService, DataBridgesFyService>();
        services.AddScoped<IDataBridgesMonthService, DataBridgesMonthService>();
        services.AddScoped<IDataActualsService, DataActualsService>();

        // --- Servicios de datos en modo "BW" (posiblemente Business Warehouse / vista agregada) ---
        services.AddScoped<IDataActualsBwService, DataActualsBwService>();
        services.AddScoped<IDataBudgetBwService, DataBudgetBwService>();
        services.AddScoped<IDataForecastBwService, DataForecastBwService>();
        services.AddScoped<IDataBridgesFyBwService, DataBridgesFyBwService>();
        services.AddScoped<IDataBridgesFyBwEurService, DataBridgesFyBwEurService>();
        services.AddScoped<IDataBridgesMonthBwService, DataBridgesMonthBwService>();

        return services;
    }

    // -------------------------------------------------------
    // Registra la infraestructura de acceso a datos:
    // - AutoMapper (mapeo entre entidades y DTOs)
    // - DataFactory (capa de abstracción de acceso a datos)
    // - Migraciones de base de datos con DbUp
    // - Repositorios concretos según el motor de BD configurado
    // -------------------------------------------------------
    public static IServiceCollection AddB4Infrastructure(
        this IServiceCollection services,
        IConfiguration configuration,   // appsettings.json del proyecto API
        IConfiguration sharedConfig,    // configuración compartida (bbdd, JWT, cadenas de conexión)
        string bbdd,                    // motor de BD: "MySQL" o "PostgreSQL"
        Logger logger)
    {
        // Registra los perfiles de AutoMapper para mapear entre entidades de dominio y DTOs
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

        // Registra el módulo DataFactory, que actúa como capa de abstracción
        // sobre el acceso a datos (puede encapsular Dapper, EF Core, etc.)
        services.AddDataFactoryModule(configuration);

        // Ejecuta las migraciones de base de datos al arrancar la aplicación.
        // DbUp compara los scripts SQL embebidos en el ensamblado con los ya
        // aplicados en la BD y ejecuta solo los pendientes, en orden.
        logger.Info("Iniciando migraciones...");
        DbUpMigrator.EnsureDatabaseUpdated(sharedConfig);
        logger.Info("Base de datos actualizada correctamente.");

        // Registra los repositorios concretos según el motor de BD seleccionado.
        // Ambas ramas implementan la misma interfaz (IUsuarioRepository),
        // lo que permite al resto de la aplicación ser agnóstico al motor de BD.
        if (bbdd.Equals("MySQL", StringComparison.OrdinalIgnoreCase))
        {
            // MySQLDapperContext encapsula la conexión Dapper a MySQL
            services.AddScoped<MySQLDapperContext>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        }
        else if (bbdd.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase))
        {
            // Para PostgreSQL el repositorio necesita la cadena de conexión directamente
            // (no usa DapperContext), por lo que se registra con una factory lambda
            // que resuelve la cadena desde IConfiguration en tiempo de resolución.
            services.AddScoped<IUsuarioRepository>(sp =>
            {
                var cs = sp.GetRequiredService<IConfiguration>()
                    .GetConnectionString("PostgresConnectionB4Data");
                if (string.IsNullOrWhiteSpace(cs))
                {
                    throw new InvalidOperationException("Falta ConnectionStrings:PostgresConnectionB4Data para IUsuarioRepository.");
                }

                return new B4.Data.PostgreSQL.Repositories.UsuarioRepository(cs);
            });
        }

        return services;
    }

    // -------------------------------------------------------
    // Configura la autenticación JWT y las políticas de autorización.
    // Lee la clave, issuer y audience desde sharedConfig (sección "Jwt")
    // y valida todos los parámetros del token en cada petición.
    // -------------------------------------------------------
    public static IServiceCollection AddB4JwtAuth(this IServiceCollection services, IConfiguration sharedConfig)
    {
        var jwtConfig = sharedConfig.GetSection("Jwt");
        var key = jwtConfig["Key"];
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException("JWT Key no está configurada en appsettings.json");
        }

        services.AddAuthentication(options =>
        {
            // Establece JWT Bearer como esquema de autenticación por defecto
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var jwtKey = jwtConfig["Key"];
            var issuer = jwtConfig["Issuer"];
            var audience = jwtConfig["Audience"];

            // Validación temprana de configuración: falla al arrancar si falta algún parámetro,
            // en lugar de fallar silenciosamente en la primera petición autenticada.
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new InvalidOperationException("La clave JWT (Jwt:Key) no está configurada.");
            if (string.IsNullOrWhiteSpace(issuer))
                throw new InvalidOperationException("El issuer JWT (Jwt:Issuer) no está configurado.");
            if (string.IsNullOrWhiteSpace(audience))
                throw new InvalidOperationException("La audiencia JWT (Jwt:Audience) no está configurada.");

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,           // Verifica que el token fue emitido por el issuer esperado
                ValidateAudience = true,          // Verifica que el token está dirigido a esta API
                ValidateLifetime = true,          // Rechaza tokens expirados
                ValidateIssuerSigningKey = true,  // Verifica la firma criptográfica del token
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };

            // Sobreescribe la respuesta por defecto del challenge 401:
            // en lugar de redirigir o devolver HTML, retorna un JSON estructurado
            // coherente con el formato de error estándar de la API B4.
            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse(); // Cancela el comportamiento por defecto

                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";

                    var result = JsonSerializer.Serialize(new
                    {
                        coderror = 401,
                        action = context.Request.Path.Value,
                        msg = "No estás autorizado",
                        ts = DateTime.UtcNow,
                        exectimems = 0,
                        count = 0,
                        data = (object?)null
                    });

                    return context.Response!.WriteAsync(result);
                }
            };
        });

        services.AddAuthorization(options =>
        {
            // Política específica para endpoints de administración: solo usuarios con rol "Admin"
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

            // Política de fallback: cualquier endpoint sin atributo [AllowAnonymous] ni política
            // explícita requiere que el usuario esté autenticado (token JWT válido).
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }
}