using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    /// <summary>
    /// MySQL-specific Usuario query definitions.
    /// </summary>
    public sealed class MySqlUsuarioQueries : IUsuarioQueries
    {
        /// <summary>
        /// Gets the MySQL query used to retrieve a Usuario by identifier.
        /// </summary>
        /// <returns>SQL query text.</returns>
        public string GetByIdQuery()
        {
            return "SELECT * FROM Usuarios WHERE Id = @Id LIMIT 1;";
        }
    }
}
