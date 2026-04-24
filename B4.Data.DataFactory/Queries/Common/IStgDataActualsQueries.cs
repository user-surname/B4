namespace B4.Data.DataFactory.Queries.Common
{
    /// <summary>
    /// Define las queries SQL que necesita el repositorio de DataActuals,
    /// incluidas sus consultas por planta, ejercicio y epigrafe.
    /// </summary>
    public interface IStgDataActualsQueries
    {
        /// <summary>
        /// Obtiene la query para insertar un DataActuals.
        /// </summary>
        string AddQuery();

        /// <summary>
        /// Obtiene la query para recuperar un DataActuals por identificador.
        /// </summary>
        string GetByIdQuery();

        /// <summary>
        /// Obtiene la query para recuperar todos los registros de DataActuals.
        /// </summary>
        string GetAllQuery();

        /// <summary>
        /// Obtiene la query para actualizar un DataActuals.
        /// </summary>
        string UpdateQuery();

        /// <summary>
        /// Obtiene la query para eliminar un DataActuals.
        /// </summary>
        string DeleteQuery();

        /// <summary>
        /// Obtiene la query para recuperar DataActuals por planta y ejercicio.
        /// </summary>
        string GetByPlantaEjercicioQuery();

        /// <summary>
        /// Obtiene la query para recuperar DataActuals por planta, ejercicio y epigrafe.
        /// </summary>
        string GetByPlantaEjercicioEpigrafeQuery();
    }
}
