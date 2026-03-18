namespace B4.Data.DataFactory.Queries.Common
{
    /// <summary>
    /// Define las queries SQL que necesita el repositorio de ControlPlanta
    /// sin acoplarlo a un proveedor concreto de base de datos.
    /// </summary>
    public interface IControlPlantaQueries
    {
        /// <summary>
        /// Obtiene la query para insertar un ControlPlanta.
        /// </summary>
        string AddQuery();

        /// <summary>
        /// Obtiene la query para recuperar un ControlPlanta por identificador.
        /// </summary>
        string GetByIdQuery();

        /// <summary>
        /// Obtiene la query para recuperar todos los registros de ControlPlanta.
        /// </summary>
        string GetAllQuery();

        /// <summary>
        /// Obtiene la query para actualizar un ControlPlanta.
        /// </summary>
        string UpdateQuery();

        /// <summary>
        /// Obtiene la query para eliminar un ControlPlanta.
        /// </summary>
        string DeleteQuery();
    }
}
