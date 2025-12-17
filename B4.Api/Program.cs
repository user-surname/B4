using B4.Api.Middleware;
using B4.Data.MySQL;
using B4.Data.PostgreSQL.Repositories;
using B4.Data.PostgreSQL.Repositories.DataRepositories;
using B4.Data.Services;
using B4.Models.Interfaces.DataInterfaces;
using B4.Models.Interfaces.LkInterfaces;
using B4.Shared;
using Microsoft.AspNetCore.ResponseCompression;

// Interfaces

// NLog
using NLog;
using NLog.Web;

using System.IO.Compression;
using System.Threading;

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

logger.Info("Iniciando API B4...");

// Migraciones

var sharedConfig = SharedConfig.Load();

B4.Data.MySQL.DbUpMigrator.EnsureDatabaseUpdated(sharedConfig);


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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --------------------------------------------------
// CONEXIÓN BASE DE DATOS
// --------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddSingleton<MySQLDapperContext>();

// --------------------------------------------------
// REGISTRO DE TODOS LOS REPOSITORIOS DATA*
// --------------------------------------------------

builder.Services.AddScoped<IDataComentariosRepository>();

builder.Services.AddScoped<IDataBudgetRepository>();

builder.Services.AddScoped<IDataForecastRepository>();

builder.Services.AddScoped<IDataBridgesFyRepository>();

builder.Services.AddScoped<IDataBridgesFyBwRepository>();

builder.Services.AddScoped<IDataBridgesFyBwEurRepository>();

builder.Services.AddScoped<IDataBridgesMonthBwRepository>();

builder.Services.AddScoped<IEpigrafeRepository>();


builder.Services.AddAuthorization();

// --------------------------------------------------
// BUILD APP
// --------------------------------------------------
var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAll");

logger.Info("API B4 iniciada correctamente (NLog Activo)");

app.Run();
