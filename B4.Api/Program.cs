using B4.Api.Extensions;
using B4.Shared;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Carga la configuración compartida (sharedConfig), que contiene parámetros
// comunes como el tipo de base de datos ("bbdd"), cadenas de conexión y JWT.
// Esta configuración es independiente del appsettings.json del proyecto.
var sharedConfig = SharedConfig.Load();

// Lee qué motor de base de datos se va a usar (ej: "MySQL" o "PostgreSQL").
// Esta clave determina qué repositorios e infraestructura se registran en el DI.
var bbdd = sharedConfig["bbdd"]?.Trim();

LogManager.Configuration.Variables["dbType"] = bbdd;

//  1. Cargar configuración ANTES de NLog
var preConfig = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

//  2. Inicializar NLog con esa configuración
var logger = LogManager.Setup()
    .LoadConfigurationFromFile(Path.Combine(AppContext.BaseDirectory, "nlog.config"))
    .LoadConfigurationFromSection(preConfig) 
    .GetCurrentClassLogger();

//  3. Reemplazar logging por NLog
builder.Logging.ClearProviders();
builder.Host.UseNLog();

try
{
    logger.Info("Iniciando API B4...");

    if (string.IsNullOrWhiteSpace(bbdd))
    {
        logger.Error("No se encontró la clave 'bbdd' en el archivo de configuración.");
        throw new InvalidOperationException("Debe especificar la base de datos a usar en 'bbdd'.");
    }

    // Registro de servicios en el contenedor de inyección de dependencias (DI):
    builder.Services
        .AddB4ApiCore()          // Middlewares, compresión, CORS, Swagger, caché y versionado
        .AddB4DomainServices()   // Servicios de dominio/negocio (ciclos, controles, datos, etc.)
        .AddB4Infrastructure(builder.Configuration, sharedConfig, bbdd, logger) // DB, migraciones y repositorios
        .AddB4JwtAuth(sharedConfig); // Autenticación JWT y políticas de autorización

    var app = builder.Build();

    // Configura el pipeline de middlewares HTTP en el orden correcto
    // (ver ApplicationBuilderExtensions para el detalle de cada paso)
    app.UseB4MiddlewarePipeline();

    // Mapea los controladores de la API a sus rutas correspondientes
    app.MapControllers();

    logger.Info("API B4 iniciada correctamente (NLog Activo)");
    app.Run();
}
catch (Exception ex)
{
    // Captura cualquier error crítico durante el arranque (ej: fallo de migración,
    // configuración inválida, etc.) y lo registra como FATAL antes de terminar.
    logger.Fatal(ex, "Error crítico durante el inicio de la API B4.");
}
finally
{
    // Garantiza que NLog libera sus recursos correctamente (vacía buffers, cierra ficheros)
    // independientemente de si la app arrancó bien o falló.
    LogManager.Shutdown();
}

// Declaración parcial de Program necesaria para que los tests de integración
// (WebApplicationFactory<Program>) puedan referenciar el entry point de la app.
public partial class Program { }