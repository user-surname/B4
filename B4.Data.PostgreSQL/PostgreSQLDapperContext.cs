using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace B4.Data.PostgreSQL
{
    public class PostgreSQLDapperContext
    {
        private readonly string _connectionString;

        public PostgreSQLDapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresConnection");
        }

        public IDbConnection CreateConnection()
            => new NpgsqlConnection(_connectionString);
    }
}
