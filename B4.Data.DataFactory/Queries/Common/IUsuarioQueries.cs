namespace B4.Data.DataFactory.Queries.Common
{
    /// <summary>
    /// Define las queries SQL de Usuario que DataFactory puede resolver
    /// segun el proveedor activo.
    /// </summary>
    public interface IUsuarioQueries
    {
        /// <summary>
        /// Obtiene la query para recuperar un Usuario por identificador.
        /// </summary>
        /// <returns>Texto SQL de la consulta.</returns>
        string GetByIdQuery();
    }
}
