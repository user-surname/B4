using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace B4.Data.PostgreSQL
{
    // Encapsula la conexión a PostgreSQL para usar con Dapper
    public class PostgreSQLDapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public PostgreSQLDapperContext(IConfiguration configuration)
        {
            _configuration = configuration;

            // 👇 Usa la misma key que estás usando en tu appsettings de la API
            _connectionString = _configuration.GetConnectionString("PostgresConnectionB4Data");

            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new InvalidOperationException("Falta ConnectionStrings:PostgresConnectionB4Data en appsettings.json");
        }

        public IDbConnection CreateConnection()
            => new NpgsqlConnection(_connectionString);
    }
}