using Azure.Core.Serialization;
using B4.Api.Extensions;
using B4.Api.Middleware;
using B4.Data.MySQL;
using B4.Data.PostgreSQL;
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
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Web;
using Npgsql;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

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

    if (bbdd.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase))
    {
        ConfigureNLogPostgreSql(sharedConfig, logger);
    }

    // AutoMapper
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
    // Aqui se inicializa la infraestructura de DataFactory para la API.
    // DataFactory: infraestructura comun y repositorios ya migrados.
    builder.Services.AddDataFactoryModule(builder.Configuration);

    // --------------------------------------------------
    // SERVICES (DOMAIN)
    // --------------------------------------------------
    // LK Services
    builder.Services.AddScoped<ICiclosService, CiclosService>();
    builder.Services.AddScoped<IEpigrafeService, EpigrafeService>();
    builder.Services.AddScoped<IFasesService, FasesService>();
    builder.Services.AddScoped<IPlantCompanyService, PlantCompanyService>();
    builder.Services.AddScoped<IPlantControllersService, PlantControllersService>();
    builder.Services.AddScoped<IPlantCountryService, PlantCountryService>();
    builder.Services.AddScoped<IPlantCurrencyService, PlantCurrencyService>();
    builder.Services.AddScoped<IPlantDivisionCompanyService, PlantDivisionCompanyService>();
    builder.Services.AddScoped<IPlantDivisionService, PlantDivisionService>();
    builder.Services.AddScoped<IPlantillasBotonesPasosTiposService, PlantillasBotonesPasosTiposService>();
    builder.Services.AddScoped<IPlantSubdivisionService, PlantSubdivisionService>();
    builder.Services.AddScoped<IPlantTreeService, PlantTreeService>();
    builder.Services.AddScoped<IControlService, ControlService>();
    builder.Services.AddScoped<IControlPlantaService, ControlPlantaService>();


    // DATA Services (contribuido)
    builder.Services.AddScoped<IDataBudgetService, DataBudgetService>();
    builder.Services.AddScoped<IDataForecastService, DataForecastService>();
    builder.Services.AddScoped<IDataComentariosService, DataComentariosService>();
    builder.Services.AddScoped<IDataTipoCambioService, DataTipoCambioService>();
    builder.Services.AddScoped<IDataBridgesFyService, DataBridgesFyService>();
    builder.Services.AddScoped<IDataBridgesMonthService, DataBridgesMonthService>();

    // Si ya migraste DataActuals al patrón service, registra también:
    builder.Services.AddScoped<IDataActualsService, DataActualsService>();

    // DATA BW Services (read-only)
    builder.Services.AddScoped<IDataActualsBwService, DataActualsBwService>();
    builder.Services.AddScoped<IDataBudgetBwService, DataBudgetBwService>();
    builder.Services.AddScoped<IDataForecastBwService, DataForecastBwService>();
    builder.Services.AddScoped<IDataBridgesFyBwService, DataBridgesFyBwService>();
    builder.Services.AddScoped<IDataBridgesFyBwEurService, DataBridgesFyBwEurService>();
    builder.Services.AddScoped<IDataBridgesMonthBwService, DataBridgesMonthBwService>();

    // --------------------------------------------------
    // DB + REPOSITORIES (según bbdd)
    // --------------------------------------------------
    
    try
    {
        if (bbdd.Equals("MySQL", StringComparison.OrdinalIgnoreCase))
        {
            logger.Info("Iniciando migraciones para MySQL...");
            B4.Data.MySQL.DbUpMigrator.EnsureDatabaseUpdated(sharedConfig);
            logger.Info("Base de datos MySQL actualizada correctamente.");

            var ctx = new MySQLDapperContext(sharedConfig);
            builder.Services.AddSingleton(ctx);

            // LK repos (MySQL)
            builder.Services.AddScoped<ICiclosRepository>(_ => new B4.Data.MySQL.Repositories.LkRepositories.CiclosRepository(ctx));
            builder.Services.AddScoped<IEpigrafeRepository>(_ => new B4.Data.MySQL.Repositories.LkRepositories.EpigrafeRepository(ctx));
            builder.Services.AddScoped<IFasesRepository>(_ => new B4.Data.MySQL.Repositories.LkRepositories.FasesRepository(ctx));
            builder.Services.AddScoped<IPlantCompanyRepository>(_ => new B4.Data.MySQL.Repositories.LkRepositories.PlantCompanyRepository(ctx));
            builder.Services.AddScoped<IPlantControllersRepository>(_ => new B4.Data.MySQL.Repositories.LkRepositories.PlantControllersRepository(ctx));
            builder.Services.AddScoped<IPlantCountryRepository>(_ => new B4.Data.MySQL.Repositories.LkRepositories.PlantCountryRepository(ctx));
            builder.Services.AddScoped<IPlantCurrencyRepository>(_ => new B4.Data.MySQL.Repositories.LkRepositories.PlantCurrencyRepository(ctx));
            builder.Services.AddScoped<IPlantDivisionCompanyRepository>(_ => new B4.Data.MySQL.Repositories.LkRepositories.PlantDivisionCompanyRepository(ctx));
            builder.Services.AddScoped<IPlantDivisionRepository>(_ => new B4.Data.MySQL.Repositories.LkRepositories.PlantDivisionRepository(ctx));
            builder.Services.AddScoped<IPlantillasBotonesPasosTiposRepository>(_ => new B4.Data.MySQL.Repositories.LkRepositories.PlantillasBotonesPasosTiposRepository(ctx));
            builder.Services.AddScoped<IPlantSubdivisionRepository>(_ => new B4.Data.MySQL.Repositories.LkRepositories.PlantSubdivisionRepository(ctx));
            builder.Services.AddScoped<IPlantTreeRepository>(_ => new B4.Data.MySQL.Repositories.LkRepositories.PlantTreeRepository(ctx));

            // DATA repos (MySQL)
            builder.Services.AddScoped<IDataComentariosRepository>(_ => new B4.Data.MySQL.Repositories.DataRepositories.DataComentariosRepository(ctx));
            builder.Services.AddScoped<IDataBudgetRepository>(_ => new B4.Data.MySQL.Repositories.DataRepositories.DataBudgetRepository(ctx));
            builder.Services.AddScoped<IDataBudgetBwRepository>(_ => new B4.Data.MySQL.Repositories.DataRepositories.DataBudgetBwRepository(ctx));
            builder.Services.AddScoped<IDataForecastRepository>(_ => new B4.Data.MySQL.Repositories.DataRepositories.DataForecastRepository(ctx));
            builder.Services.AddScoped<IDataForecastBwRepository>(_ => new B4.Data.MySQL.Repositories.DataRepositories.DataForecastBwRepository(ctx));

            builder.Services.AddScoped<IDataBridgesFyRepository>(_ => new B4.Data.MySQL.Repositories.DataRepositories.DataBridgesFyRepository(ctx));
            builder.Services.AddScoped<IDataBridgesFyBwRepository>(_ => new B4.Data.MySQL.Repositories.DataRepositories.DataBridgesFyBwRepository(ctx));
            builder.Services.AddScoped<IDataBridgesFyBwEurRepository>(_ => new B4.Data.MySQL.Repositories.DataRepositories.DataBridgesFyBwEurRepository(ctx));

            builder.Services.AddScoped<IDataBridgesMonthRepository>(_ => new B4.Data.MySQL.Repositories.DataRepositories.DataBridgesMonthRepository(ctx));
            builder.Services.AddScoped<IDataBridgesMonthBwRepository>(_ => new B4.Data.MySQL.Repositories.DataRepositories.DataBridgesMonthBwRepository(ctx));

            builder.Services.AddScoped<IDataTipoCambioRepository>(_ => new B4.Data.MySQL.Repositories.DataRepositories.DataTipoCambioRepository(ctx));

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

            builder.Services.AddScoped<IUsuarioRepository>(_ =>
                new B4.Data.MySQL.Repositories.UsuarioRepository(ctx));

            builder.Services.AddScoped<IControlRepository>(_ =>
                new B4.Data.MySQL.Repositories.ControlRepository(ctx));
        }
        else if (bbdd.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase))
        {
            logger.Info("Iniciando migraciones para PostgreSQL...");
            B4.Data.PostgreSQL.DbUpMigrator.EnsureDatabaseUpdated(sharedConfig);
            logger.Info("Base de datos PostgreSQL actualizada correctamente.");

            var ctx = new PostgreSQLDapperContext(sharedConfig);
            builder.Services.AddSingleton(ctx);

            // LK repos (PostgreSQL)  Eañade los que falten según tu proyecto
            builder.Services.AddScoped<IEpigrafeRepository>(_ => new B4.Data.PostgreSQL.Repositories.LKRepositories.EpigrafeRepository(ctx));
            // Si tienes más LK repos en Postgres, regístralos aquí igual que en MySQL:
            // builder.Services.AddScoped<ICiclosRepository>(_ => new ...);
            // builder.Services.AddScoped<IFasesRepository>(_ => new ...);
            // etc.

            // DATA repos (PostgreSQL)
            builder.Services.AddScoped<IDataComentariosRepository>(_ => new B4.Data.PostgreSQL.Repositories.DataRepositories.DataComentariosRepository(ctx));
            builder.Services.AddScoped<IDataBudgetRepository>(_ => new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBudgetRepository(ctx));
            //builder.Services.AddScoped<IDataBudgetBwRepository>(_ => new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBudgetBwRepository(ctx));
            builder.Services.AddScoped<IDataForecastRepository>(_ => new B4.Data.PostgreSQL.Repositories.DataRepositories.DataForecastRepository(ctx));
            builder.Services.AddScoped<IDataForecastBwRepository>(_ => new B4.Data.PostgreSQL.Repositories.DataRepositories.DataForecastBwRepository(ctx));

            builder.Services.AddScoped<IDataBridgesFyRepository>(_ => new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBridgesFyRepository(ctx));
            builder.Services.AddScoped<IDataBridgesFyBwRepository>(_ => new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBridgesFyBwRepository(ctx));
            builder.Services.AddScoped<IDataBridgesFyBwEurRepository>(_ => new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBridgesFyBwEurRepository(ctx));

            builder.Services.AddScoped<IDataBridgesMonthRepository>(_ => new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBridgesMonthRepository(ctx));
            builder.Services.AddScoped<IDataBridgesMonthBwRepository>(_ => new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBridgesMonthBwRepository(ctx));

            builder.Services.AddScoped<IDataTipoCambioRepository>(_ => new B4.Data.PostgreSQL.Repositories.DataRepositories.DataTipoCambioRepository(ctx));

        }
        else
        {
            logger.Error("Valor desconocido para 'bbdd' en configuracion: {0}", bbdd);
            throw new InvalidOperationException($"Valor desconocido para 'bbdd': {bbdd}");
        }
    }
    catch (Exception ex)
    {
        logger.Fatal(ex, $"Error durante la actualizacion de la base de datos {bbdd}.");
        throw;
    }

    // --------------------------------------------------
    // MIDDLEWARES / INFRA
    // --------------------------------------------------
    builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();
    builder.Services.AddTransient<ResponseWrapperMiddleware>();

    // Compresión GZIP
    builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.SmallestSize;
    });
    builder.Services.AddResponseCompression(options =>
    {
        options.Providers.Add<GzipCompressionProvider>();
        options.EnableForHttps = true;
    });

    // MemoryCache (LK)
    // MemoryCache tablas maestras
    builder.Services.AddMemoryCache();
    builder.Services.AddScoped<IMemoryCacheService, MemoryCacheService>();


    // Controllers + Swagger + Versionado
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    });

    builder.Services.AddVersionedApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    builder.Services.AddSwaggerGen(c =>
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

    builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

    // PasswordHasher
    builder.Services.AddSingleton<IPasswordHasherService, PasswordHasherService>();

    // CORS (ya que haces app.UseCors("AllowAll"))
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod());
    });

    // --------------------------------------------------
    // AUTH JWT
    // --------------------------------------------------
    var jwtConfig = sharedConfig.GetSection("Jwt");
    var key = jwtConfig["Key"];
    if (string.IsNullOrEmpty(key))
        throw new Exception("JWT Key no está configurada en appsettings.json");

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var jwtKey = jwtConfig["Key"];
        var issuer = jwtConfig["Issuer"];
        var audience = jwtConfig["Audience"];

        if (string.IsNullOrWhiteSpace(jwtKey))
            throw new InvalidOperationException("La clave JWT (Jwt:Key) no está configurada.");
        if (string.IsNullOrWhiteSpace(issuer))
            throw new InvalidOperationException("El issuer JWT (Jwt:Issuer) no está configurado.");
        if (string.IsNullOrWhiteSpace(audience))
            throw new InvalidOperationException("La audiencia JWT (Jwt:Audience) no está configurada.");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                // Evita el mensaje genérico de WWW-Authenticate
                context.HandleResponse();

                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize (new
                {
                    coderror = 401,
                    action = context?.Request?.Path.Value,
                    msg = "No estás autorizado",
                    ts = DateTime.UtcNow,
                    exectimems = 0,
                    count = 0,
                    data = (object)null
                });

                return context.Response.WriteAsync(result);
            }
        };

    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

        // Política global que requiere autenticación para todas las rutas por defecto
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });

    builder.Services.AddScoped<JwtService>();

    builder.Services.AddAuthorization(options =>
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });

    // Añadir servicios de controladores y configurar JSON
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            // Convierte PascalCase del backend a camelCase para Angular
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });


    // --------------------------------------------------
    // BUILD APP
    // --------------------------------------------------
    var app = builder.Build();

    // Swagger
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
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

        app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    // Global errors JSON
    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

    // Response wrapper
    app.UseMiddleware<ResponseWrapperMiddleware>();

    // try/catch para errores de binding
    app.Use(async (context, next) =>
    {
        var endpoint = context.GetEndpoint();
        if (endpoint == null)
        {
            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                coderror = 404,
                action = context.Request.Path,
                msg = "Ruta o parámetro enviado no es válido",
                ts = DateTime.UtcNow,
                exectimems = 0,
                count = 0,
                data = (object)null
            });
            return;
        }

        await next();
    });

    app.UseAuthentication();
    app.UseAuthorization();

    // Middleware para rutas inexistentes
    app.UseStatusCodePages(async context =>
    {
        var response = context.HttpContext.Response;

        if (response.StatusCode == StatusCodes.Status404NotFound)
        {
            response.ContentType = "application/json";

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Route Not Found",
                Type = "https://httpstatuses.com/404",
                Detail = "The requested route does not exist.",
                Instance = context.HttpContext.Request.Path
            };

            await response.WriteAsJsonAsync(problemDetails);
        }
    });

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

static void ConfigureNLogPostgreSql(IConfiguration configuration, Logger logger)
{
    var connectionString = configuration.GetConnectionString("PostgresConnectionB4Control");
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        logger.Warn("NLog PostgreSQL desactivado: falta ConnectionStrings:PostgresConnectionB4Control.");
        return;
    }

    EnsureNLogTableExists(connectionString);

    var nlogConfig = LogManager.Configuration;
    if (nlogConfig is null)
    {
        logger.Warn("NLog PostgreSQL desactivado: configuración NLog no disponible.");
        return;
    }

    if (nlogConfig.FindTargetByName("postgresDb") is not null)
    {
        return;
    }

    var dbTarget = new DatabaseTarget("postgresDb")
    {
        DBProvider = "Npgsql.NpgsqlConnection, Npgsql",
        ConnectionString = connectionString,
        CommandText = @"
            INSERT INTO app_logs (level, logger, message, exception, machine_name, request_url)
            VALUES (@level, @logger, @message, @exception, @machine_name, @request_url);"
    };

    dbTarget.Parameters.Add(new DatabaseParameterInfo("@level", "${level:uppercase=true}"));
    dbTarget.Parameters.Add(new DatabaseParameterInfo("@logger", "${logger}"));
    dbTarget.Parameters.Add(new DatabaseParameterInfo("@message", "${message}"));
    dbTarget.Parameters.Add(new DatabaseParameterInfo("@exception", "${exception:format=tostring}"));
    dbTarget.Parameters.Add(new DatabaseParameterInfo("@machine_name", "${machinename}"));
    dbTarget.Parameters.Add(new DatabaseParameterInfo("@request_url", "${aspnet-request-url}"));

    nlogConfig.AddTarget(dbTarget);
    nlogConfig.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Info, dbTarget));
    LogManager.ReconfigExistingLoggers();

    logger.Info("NLog PostgreSQL target activo.");
}

static void EnsureNLogTableExists(string connectionString)
{
    const string sql = @"
        CREATE TABLE IF NOT EXISTS app_logs (
            id BIGSERIAL PRIMARY KEY,
            created_at TIMESTAMPTZ NOT NULL DEFAULT NOW(),
            level VARCHAR(20) NOT NULL,
            logger VARCHAR(300) NULL,
            message TEXT NOT NULL,
            exception TEXT NULL,
            machine_name VARCHAR(200) NULL,
            request_url TEXT NULL
        );

        CREATE INDEX IF NOT EXISTS idx_app_logs_created_at ON app_logs (created_at DESC);";

    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();
    using var command = new NpgsqlCommand(sql, connection);
    command.ExecuteNonQuery();
}

// ✁ENecesario para tests con WebApplicationFactory (PUNTO 7)
public partial class Program { }

