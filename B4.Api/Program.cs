using B4.Data.MySQL;
using B4.Data.PostgreSQL;
using B4.Data.PostgreSQL.Repositories;
using B4.Data.PostgreSQL.Repositories.DataRepositories;
using B4.Data.Services;
using B4.Models.Interfaces.DataInterfaces;
using B4.Models.Interfaces.LkInterfaces;
using B4.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using NLog;
using NLog.Web;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Microsoft.OpenApi.Models;


using B4.Api.Middleware;




// --------------------------------------------------
// CREACIÓN DEL BUILDER
// --------------------------------------------------
var builder = WebApplication.CreateBuilder(args);

// NLog
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var logger = LogManager.Setup()
    .LoadConfigurationFromFile(Path.Combine(AppContext.BaseDirectory, "nlog.config"))
    .GetCurrentClassLogger();

try
{
    logger.Info("Iniciando API B4...");

    var sharedConfig = SharedConfig.Load();

    var bbdd = sharedConfig["bbdd"]?.Trim();

    if (string.IsNullOrWhiteSpace(bbdd))
    {
        logger.Error("No se encontró la clave 'bbdd' en el archivo de configuración.");
        throw new InvalidOperationException("Debe especificar la base de datos a usar en 'bbdd'.");
    }

    try
    {
        if (bbdd.Equals("MySQL", StringComparison.OrdinalIgnoreCase))
        {
            logger.Info("Iniciando migraciones para MySQL...");

            B4.Data.MySQL.DbUpMigrator.EnsureDatabaseUpdated(sharedConfig);
            logger.Info("Base de datos MySQL actualizada correctamente.");

            var ctx = new MySQLDapperContext(sharedConfig);
            builder.Services.AddSingleton(ctx);

            builder.Services.AddScoped<IDataComentariosRepository>(_ =>
                new B4.Data.MySQL.Repositories.DataRepositories.DataComentariosRepository(ctx));

            builder.Services.AddScoped<IDataBudgetRepository>(_ =>
                new B4.Data.MySQL.Repositories.DataRepositories.DataBudgetRepository(ctx));

            builder.Services.AddScoped<IDataForecastRepository>(_ =>
                new B4.Data.MySQL.Repositories.DataRepositories.DataForecastRepository(ctx));

            builder.Services.AddScoped<IDataBridgesFyRepository>(_ =>
                new B4.Data.MySQL.Repositories.DataRepositories.DataBridgesFyRepository(ctx));

            builder.Services.AddScoped<IDataBridgesFyBwRepository>(_ =>
                new B4.Data.MySQL.Repositories.DataRepositories.DataBridgesFyBwRepository(ctx));

            builder.Services.AddScoped<IDataBridgesFyBwEurRepository>(_ =>
                new B4.Data.MySQL.Repositories.DataRepositories.DataBridgesFyBwEurRepository(ctx));

            builder.Services.AddScoped<IDataBridgesMonthBwRepository>(_ =>
                new B4.Data.MySQL.Repositories.DataRepositories.DataBridgesMonthBwRepository(ctx));

            builder.Services.AddScoped<IEpigrafeRepository>(_ =>
                new B4.Data.MySQL.Repositories.LkRepositories.EpigrafeRepository(ctx));

            // ✅ ACTUALS (MySQL) - ajusta el namespace si tu repo MySQL existe con ese nombre
            builder.Services.AddScoped<IDataActualsRepository>(_ =>
                new B4.Data.MySQL.Repositories.DataRepositories.DataActualsRepository(ctx));
        }
        else if (bbdd.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase))
        {
            logger.Info("Iniciando migraciones para PostgreSQL...");
            B4.Data.PostgreSQL.DbUpMigrator.EnsureDatabaseUpdated(sharedConfig);
            logger.Info("Base de datos PostgreSQL actualizada correctamente.");

            var ctx = new PostgreSQLDapperContext(sharedConfig);
            builder.Services.AddSingleton(ctx);

            builder.Services.AddScoped<IDataComentariosRepository>(_ =>
                new B4.Data.PostgreSQL.Repositories.DataRepositories.DataComentariosRepository(ctx));

            builder.Services.AddScoped<IDataBudgetRepository>(_ =>
                new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBudgetRepository(ctx));

            builder.Services.AddScoped<IDataForecastRepository>(_ =>
                new B4.Data.PostgreSQL.Repositories.DataRepositories.DataForecastRepository(ctx));

            builder.Services.AddScoped<IDataBridgesFyRepository>(_ =>
                new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBridgesFyRepository(ctx));

            builder.Services.AddScoped<IDataBridgesFyBwRepository>(_ =>
                new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBridgesFyBwRepository(ctx));

            builder.Services.AddScoped<IDataBridgesFyBwEurRepository>(_ =>
                new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBridgesFyBwEurRepository(ctx));

            builder.Services.AddScoped<IDataBridgesMonthBwRepository>(_ =>
                new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBridgesMonthBwRepository(ctx));

            builder.Services.AddScoped<IEpigrafeRepository>(_ =>
                new B4.Data.PostgreSQL.Repositories.LKRepositories.EpigrafeRepository(ctx));

            // ✅ ACTUALS (PostgreSQL)
            builder.Services.AddScoped<IDataActualsRepository>(_ =>
                new B4.Data.PostgreSQL.Repositories.DataRepositories.DataActualsRepository(ctx));
        }
        else
        {
            logger.Error("Valor desconocido para 'bbdd' en configuración: {0}", bbdd);
            throw new InvalidOperationException($"Valor desconocido para 'bbdd': {bbdd}");
        }
    }
    catch (Exception ex)
    {
        logger.Fatal(ex, $"Error durante la actualización de la base de datos {bbdd}.");
        throw;
    }

    // Middleware de metadata
    builder.Services.AddTransient<ResponseWrapperMiddleware>();

    // Middleware global (errores)
    builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

    // Compresión GZIP
    builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.SmallestSize;
    });

    // MemoryCache tablas maestras
    builder.Services.AddMemoryCache();
    builder.Services.AddSingleton<IMemoryCacheService, MemoryCacheService>();

    // Controllers y Swagger
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        // Crear un documento Swagger/OpenAPI llamado "v1" con título y versión
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Mi API",  // Nombre de la API que aparecerá en Swagger UI
            Version = "v1"     // Versión del API
        });

        // -------------------------------
        // Definir esquema de seguridad JWT
        // -------------------------------
        // Esto permite que Swagger entienda que algunos endpoints requieren un token JWT
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",                   // Nombre del header HTTP donde se envía el token
            Type = SecuritySchemeType.Http,          // Tipo de autenticación HTTP
            Scheme = "Bearer",                        // Esquema de autenticación: Bearer token
            BearerFormat = "JWT",                     // Formato esperado del token
            In = ParameterLocation.Header,            // El token se envía en el header
            Description = "Ingrese 'Bearer {token}'" // Descripción que aparece en Swagger UI
        });

        // -------------------------------
        // Aplicar seguridad a todos los endpoints
        // -------------------------------
        // Esto indica que todos los endpoints protegidos por JWT usarán la definición anterior
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme, // Indica que hace referencia a un esquema de seguridad
                    Id = "Bearer"                        // ID del esquema definido arriba ("Bearer")
                }
            },
            new string[] {} // Array vacío: no se requieren scopes adicionales
        }
    });
    });

    // --------------------------------------------------
    // CONEXIÓN BASE DE DATOS PostgreSQL
    // --------------------------------------------------

    //var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
    //builder.Services.AddSingleton<MySQLDapperContext>();

    var jwtConfig = builder.Configuration.GetSection("Jwt");

    var key = jwtConfig["Key"];
    if (string.IsNullOrEmpty(key))
    {
        throw new Exception("JWT Key no está configurada en appsettings.json");
    }


    try
    {
        builder.Services.AddAuthentication(options =>
        {
            // -------------------------------
            // Configuración general de autenticación
            // -------------------------------
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        // -------------------------------
        // Configuración específica para JWT
        // -------------------------------
        .AddJwtBearer(options =>
        {
            try
            {
                var key = jwtConfig["Key"];
                var issuer = jwtConfig["Issuer"];
                var audience = jwtConfig["Audience"];

                // Validaciones básicas de configuración
                if (string.IsNullOrWhiteSpace(key))
                    throw new InvalidOperationException("La clave JWT (Jwt:Key) no está configurada.");
                if (string.IsNullOrWhiteSpace(issuer))
                    throw new InvalidOperationException("El issuer JWT (Jwt:Issuer) no está configurado.");
                if (string.IsNullOrWhiteSpace(audience))
                    throw new InvalidOperationException("La audiencia JWT (Jwt:Audience) no está configurada.");

                // -------------------------------
                // Parámetros de validación del token
                // -------------------------------
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(key)
                    )
                };
            }
            catch (Exception ex)
            {
                // Captura errores específicos de la configuración de JWT
                throw new InvalidOperationException("Error en la configuración de JWT", ex);
            }
        });
    }
    catch (Exception ex)
    {
        // Captura errores generales al agregar autenticación
        // Por ejemplo si jwtConfig es null
        Console.WriteLine($"Error configurando autenticación JWT: {ex.Message}");
        throw;
    }


    builder.Services.AddAuthorization();
    builder.Services.AddScoped<JwtService>();


    // --------------------------------------------------
    // BUILD APP
    // --------------------------------------------------
    var app = builder.Build();

    // Middleware global
    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    // Errores (genera body JSON en fallos)
    app.UseMiddleware<ResponseWrapperMiddleware>();
    // Wrapper metadata (envuelve todo)
    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors("AllowAll");
    app.MapControllers();

    logger.Info("API B4 iniciada correctamente (NLog Activo)");

    app.Run();
}
catch (Exception ex)
{
    logger.Fatal(ex, "Error crítico durante el inicio de la API B4.");
}
finally
{
    LogManager.Shutdown();
}

// ✅ Necesario para tests con WebApplicationFactory (PUNTO 7)
public partial class Program { }
