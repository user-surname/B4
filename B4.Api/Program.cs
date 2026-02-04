using B4.Api.Controllers;
using B4.Api.Middleware;
using B4.Data.MySQL;
using B4.Data.MySQL.Repositories;
using B4.Data.PostgreSQL;
using B4.Data.PostgreSQL.Repositories;
using B4.Data.PostgreSQL.Repositories.DataRepositories;
using B4.Data.Services;
using B4.Models.Interfaces;
using B4.Models.Interfaces.DataInterfaces;
using B4.Models.Interfaces.LkInterfaces;
using B4.Shared;
using B4.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

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

    var sharedConfig = B4.Shared.SharedConfig.Load();

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

            builder.Services.AddScoped<ICiclosRepository>(_ =>
                new B4.Data.MySQL.Repositories.LkRepositories.CiclosRepository(ctx));

            builder.Services.AddScoped<IEpigrafeRepository>(_ =>
                new B4.Data.MySQL.Repositories.LkRepositories.EpigrafeRepository(ctx));

            builder.Services.AddScoped<IFasesRepository>(_ =>
                new B4.Data.MySQL.Repositories.LkRepositories.FasesRepository(ctx));

            builder.Services.AddScoped<IPlantCompanyRepository>(_ =>
                new B4.Data.MySQL.Repositories.LkRepositories.PlantCompanyRepository(ctx));

            builder.Services.AddScoped<IPlantControllersRepository>(_ =>
                new B4.Data.MySQL.Repositories.LkRepositories.PlantControllersRepository(ctx));

            builder.Services.AddScoped<IPlantCountryRepository>(_ =>
                new B4.Data.MySQL.Repositories.LkRepositories.PlantCountryRepository(ctx));

            builder.Services.AddScoped<IPlantCurrencyRepository>(_ =>
                new B4.Data.MySQL.Repositories.LkRepositories.PlantCurrencyRepository(ctx));

            builder.Services.AddScoped<IPlantDivisionCompanyRepository>(_ =>
                new B4.Data.MySQL.Repositories.LkRepositories.PlantDivisionCompanyRepository(ctx));

            builder.Services.AddScoped<IPlantDivisionRepository>(_ =>
                new B4.Data.MySQL.Repositories.LkRepositories.PlantDivisionRepository(ctx));

            builder.Services.AddScoped<IPlantillasBotonesPasosTiposRepository>(_ =>
                new B4.Data.MySQL.Repositories.LkRepositories.PlantillasBotonesPasosTiposRepository(ctx));

            builder.Services.AddScoped<IPlantSubdivisionRepository>(_ =>
                new B4.Data.MySQL.Repositories.LkRepositories.PlantSubdivisionRepository(ctx));

            builder.Services.AddScoped<IPlantTreeRepository>(_ =>
                new B4.Data.MySQL.Repositories.LkRepositories.PlantTreeRepository(ctx));

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

            builder.Services.AddScoped<IUsuarioRepository>(_ =>
                new B4.Data.MySQL.Repositories.UsuarioRepository(ctx));
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

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    });

    // Inicializacion de sistema de versiones de la API

    builder.Services.AddVersionedApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    builder.Services.AddSwaggerGen(c =>
    {
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

    builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

    // --------------------------------------------------
    // CONEXIÓN BASE DE DATOS PostgreSQL
    // --------------------------------------------------

    //var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
    //builder.Services.AddSingleton<MySQLDapperContext>();
    builder.Services.AddSingleton<IPasswordHasherService, PasswordHasherService>();

    var jwtConfig = sharedConfig.GetSection("Jwt");
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


    builder.Services.AddAuthorization(options =>
    {
        // Policy que exige que el usuario sea Admin
        options.AddPolicy("AdminOnly", policy =>
            policy.RequireRole("Admin"));
    });

    builder.Services.AddScoped<JwtService>();


    // --------------------------------------------------
    // BUILD APP
    // --------------------------------------------------
    var app = builder.Build();
    app.UseSwaggerUI(c =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant()
            );
        }
    });

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
