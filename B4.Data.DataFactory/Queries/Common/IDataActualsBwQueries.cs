namespace B4.Data.DataFactory.Queries.Common
{
    /// <summary>
    /// Define las queries SQL que necesita el repositorio de DataActualsBw
    /// para resolverlas por proveedor desde DataFactory.
    /// </summary>
    public interface IDataActualsBwQueries
    {
        /// <summary>
        /// Obtiene la query para insertar un DataActualsBw.
        /// </summary>
        string AddQuery();

        /// <summary>
        /// Obtiene la query para recuperar un DataActualsBw por identificador.
        /// </summary>
        string GetByIdQuery();

        /// <summary>
        /// Obtiene la query para recuperar todos los registros de DataActualsBw.
        /// </summary>
        string GetAllQuery();

        /// <summary>
        /// Obtiene la query para actualizar un DataActualsBw.
        /// </summary>
        string UpdateQuery();

        /// <summary>
        /// Obtiene la query para eliminar un DataActualsBw.
        /// </summary>
        string DeleteQuery();
    }
}
