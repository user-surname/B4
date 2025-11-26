
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using B4.Api.Middleware;
using B4.Data.PostgreSQL.Repositories;
using B4.Data.Services;
using B4.DBMigrations;
using B4.Data.MySQL;
// necesario para inyeccion de dependencias
using B4.Models.Interfaces;
using Microsoft.AspNetCore.ResponseCompression;
using ProyectoPILOTO.Data;
using System.IO.Compression;
// ---- NLOG ----
using NLog;
using NLog.Web;

// creacion builder
var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseNLog();

var logger = LogManager.Setup()
    .LoadConfigurationFromFile(Path.Combine(AppContext.BaseDirectory, "nlog.config"))
    .GetCurrentClassLogger();

logger.Info("Iniciando API B4...");

builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

// servicio compresion GZIP
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});


// Servicio MemoryCache para tablas maestras
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IMemoryCacheService, MemoryCacheService>();


// agregamos los controllers
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Obtener la cadena de conexi�n desde la configuraci�n
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddSingleton<MySQLDapperContext>();

// 2. Registrar el repositorio
//builder.Services.AddScoped<IUsuarioRepository>(provider =>
//    new UsuarioRepository(connectionString!));
// NOTA: Usa 'new' o 'ActivatorUtilities.CreateInstance' para pasar el string al constructor

builder.Services.AddScoped<IDataComentariosRepository>(provider =>
    new DataComentariosRepository(connectionString!));


builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

DbUpMigrator.EnsureDatabaseUpdated(builder.Configuration);

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization(); 

app.MapControllers();

app.UseCors("AllowAll");

logger.Info("API B4 iniciada correctamente (NLog Activo)");

app.Run();

