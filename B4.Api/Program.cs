
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using B4.Api.Middleware;

// necesario para inyeccion de dependencias
using B4.Models.Interfaces;
using B4.Data.PostgreSQL.Repositories;
using B4.Data.Services;


// creacion builder
var builder = WebApplication.CreateBuilder(args);

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


// 1. Obtener la cadena de conexión desde la configuración
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

// 2. Registrar el repositorio
builder.Services.AddScoped<IUsuarioRepository>(provider =>
    new UsuarioRepository(connectionString!));
// NOTA: Usa 'new' o 'ActivatorUtilities.CreateInstance' para pasar el string al constructor

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

//app.UseAuthorization(); 
builder.Services.AddAuthorization();
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("UserPolicy", policy => policy.RequireRole("LkPlantCompany"));
//});

app.MapControllers();

app.UseCors("AllowAll");

app.Run();

