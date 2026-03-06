using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    /// <summary>
    /// PostgreSQL-specific Usuario query definitions.
    /// </summary>
    public sealed class PostgreSqlUsuarioQueries : IUsuarioQueries
    {
        /// <summary>
        /// Gets the PostgreSQL query used to retrieve a Usuario by identifier.
        /// </summary>
        /// <returns>SQL query text.</returns>
        public string GetByIdQuery()
        {
            return "SELECT * FROM \"Usuarios\" WHERE \"Id\" = @Id LIMIT 1;";
        }
    }
}
