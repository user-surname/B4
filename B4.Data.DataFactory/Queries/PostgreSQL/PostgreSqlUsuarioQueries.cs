using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    /// <summary>
    /// Implementa la version PostgreSQL de las queries de Usuario.
    /// DataQueryProvider devuelve esta clase cuando "bbdd" vale "PostgreSQL".
    /// </summary>
    public sealed class PostgreSqlUsuarioQueries : IUsuarioQueries
    {
        /// <summary>
        /// Obtiene la query de PostgreSQL para recuperar un Usuario por identificador.
        /// </summary>
        /// <returns>Texto SQL de la consulta.</returns>
        public string GetByEmailQuery()
        {
            return "SELECT * FROM \"Usuarios\" WHERE \"Id\" = @Id LIMIT 1;";
        }
    }
}
