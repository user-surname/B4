using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace B4.Data.MySQL
{ 
    // Esta clase encapsula la lógica de conexión a la base de datos
    // y permite crear instancias de conexión cuando se necesiten en los repositorios o controladores
    public class DapperContext
{
    // Para acceder a la configuración de la aplicación (appsettings.json)
    private readonly IConfiguration _configuration;

    // Cadena de conexión obtenida desde la configuración
    private readonly string _connectionString;

    // Constructor que recibe la configuración mediante inyección de dependencias
    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;

        // Obtiene la cadena de conexión llamada "DefaultConnection" del appsettings.json
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    // Método para crear y devolver una conexión a la base de datos PostgreSQL
    // Cada vez que se llama, se obtiene una nueva instancia de NpgsqlConnection
    public IDbConnection CreateConnection()
        => new MySqlConnection(_connectionString);
}
}
