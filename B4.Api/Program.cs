using B4.Api.Middleware;
using B4.Data.MySQL;
using B4.Data.PostgreSQL;
using B4.Data.PostgreSQL.Repositories;
using B4.Data.PostgreSQL.Repositories.DataRepositories;
using B4.Data.Services;
using B4.Models.Interfaces.DataInterfaces;
using B4.Models.Interfaces.LkInterfaces;
using B4.Shared;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Web;
using System;
using System.IO;
using System.IO.Compression;

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

    // Migraciones MySQL
    var sharedConfig = SharedConfig.Load();

    // Obtener qué base de datos usar: MySQL o PostgreSQL
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

            var MyDapperContext = new MySQLDapperContext(sharedConfig);
            builder.Services.AddSingleton(MyDapperContext);


            builder.Services.AddScoped<IDataComentariosRepository>(provider =>
                new B4.Data.MySQL.Repositories.DataRepositories.DataComentariosRepository(MyDapperContext));

            builder.Services.AddScoped<IDataBudgetRepository>(provider =>
                new B4.Data.MySQL.Repositories.DataRepositories.DataBudgetRepository(MyDapperContext));

            builder.Services.AddScoped<IDataForecastRepository>(provider =>
                new B4.Data.MySQL.Repositories.DataRepositories.DataForecastRepository(MyDapperContext));

            builder.Services.AddScoped<IDataBridgesFyRepository>(provider =>
                new B4.Data.MySQL.Repositories.DataRepositories.DataBridgesFyRepository(MyDapperContext));

            builder.Services.AddScoped<IDataBridgesFyBwRepository>(provider =>
                new B4.Data.MySQL.Repositories.DataRepositories.DataBridgesFyBwRepository(MyDapperContext));

            builder.Services.AddScoped<IDataBridgesFyBwEurRepository>(provider =>
                new B4.Data.MySQL.Repositories.DataRepositories.DataBridgesFyBwEurRepository(MyDapperContext));

            builder.Services.AddScoped<IDataBridgesMonthBwRepository>(provider =>
                new B4.Data.MySQL.Repositories.DataRepositories.DataBridgesMonthBwRepository(MyDapperContext));

            builder.Services.AddScoped<IEpigrafeRepository>(provider =>
                new B4.Data.MySQL.Repositories.LkRepositories.EpigrafeRepository(MyDapperContext));

        }
        else if (bbdd.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase))
        {
            logger.Info("Iniciando migraciones para PostgreSQL...");
            B4.Data.PostgreSQL.DbUpMigrator.EnsureDatabaseUpdated(sharedConfig);
            logger.Info("Base de datos PostgreSQL actualizada correctamente.");

            var MyDapperContext = new PostgreSQLDapperContext(sharedConfig);
            builder.Services.AddSingleton(MyDapperContext);

            builder.Services.AddScoped<IDataComentariosRepository>(provider =>
                new B4.Data.PostgreSQL.Repositories.DataRepositories.DataComentariosRepository(MyDapperContext));

            builder.Services.AddScoped<IDataBudgetRepository>(provider =>
                new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBudgetRepository(MyDapperContext));

            builder.Services.AddScoped<IDataForecastRepository>(provider =>
                new B4.Data.PostgreSQL.Repositories.DataRepositories.DataForecastRepository(MyDapperContext));

            builder.Services.AddScoped<IDataBridgesFyRepository>(provider =>
                new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBridgesFyRepository(MyDapperContext));

            builder.Services.AddScoped<IDataBridgesFyBwRepository>(provider =>
                new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBridgesFyBwRepository(MyDapperContext));

            builder.Services.AddScoped<IDataBridgesFyBwEurRepository>(provider =>
                new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBridgesFyBwEurRepository(MyDapperContext));

            builder.Services.AddScoped<IDataBridgesMonthBwRepository>(provider =>
                new B4.Data.PostgreSQL.Repositories.DataRepositories.DataBridgesMonthBwRepository(MyDapperContext));

            builder.Services.AddScoped<IEpigrafeRepository>(provider =>
                new B4.Data.PostgreSQL.Repositories.LKRepositories.EpigrafeRepository(MyDapperContext));

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

    // Middleware global
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

    // --------------------------------------------------
    // CONEXIÓN BASE DE DATOS PostgreSQL
    // --------------------------------------------------

    //var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
    //builder.Services.AddSingleton<MySQLDapperContext>();

    builder.Services.AddAuthorization();

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
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthorization();
    app.UseCors("AllowAll");
    app.MapControllers();

    logger.Info("API B4 iniciada correctamente (NLog Activo)");

    try
    {
        app.Run();
    }
    catch (Exception ex)
    {
        logger.Fatal(ex, "Error crítico al iniciar la aplicación.");
        throw;
    }
}
catch (Exception ex)
{
    logger.Fatal(ex, "Error crítico durante el inicio de la API B4.");
}
finally
{
    LogManager.Shutdown();
}
