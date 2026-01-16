using B4.Data.MySQL;
using B4.Data.PostgreSQL;
using B4.Data.Services;
using B4.Models.Interfaces.DataInterfaces;
using B4.Models.Interfaces.LkInterfaces;
using B4.Shared;
using Microsoft.AspNetCore.ResponseCompression;
using NLog;
using NLog.Web;
using System.IO.Compression;

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
    builder.Services.AddSwaggerGen();

    builder.Services.AddAuthorization();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    // Errores (genera body JSON en fallos)
    app.UseMiddleware<ResponseWrapperMiddleware>();
    // Wrapper metadata (envuelve todo)
    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    
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
