using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace B4.Data.PostgreSQL
{
    // Clase que gestiona la conexión con PostgreSQL usando Dapper
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("PostgresAiven"); // nombre en appsettings
        }

        public IDbConnection CreateConnection()
            => new NpgsqlConnection(_connectionString);
    }
}
