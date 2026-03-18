using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    /// <summary>
    /// Implementa la version MySQL de las queries de Usuario.
    /// DataQueryProvider devuelve esta clase cuando "bbdd" vale "MySQL".
    /// </summary>
    public sealed class MySqlUsuarioQueries : IUsuarioQueries
    {
        /// <summary>
        /// Obtiene la query de MySQL para recuperar un Usuario por identificador.
        /// </summary>
        /// <returns>Texto SQL de la consulta.</returns>
        public string GetByIdQuery()
        {
            return "SELECT * FROM Usuarios WHERE Id = @Id LIMIT 1;";
        }
    }
}
