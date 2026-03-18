using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    /// <summary>
    /// Implementa la version MySQL de las queries de ControlPlanta.
    /// DataQueryProvider devuelve esta clase cuando "bbdd" vale "MySQL".
    /// </summary>
    public sealed class MySqlControlPlantaQueries : IControlPlantaQueries
    {
        /// <summary>
        /// Obtiene la query para insertar un ControlPlanta en MySQL.
        /// </summary>
        public string AddQuery()
        {
            return @"
                INSERT INTO CONTROL_PLANTA
                (idControlPlanta, idControl, idCompany)
                VALUES (@IdControlPlanta, @IdControl, @IdCompany)";
        }

        /// <summary>
        /// Obtiene la query para recuperar un ControlPlanta por identificador en MySQL.
        /// </summary>
        public string GetByIdQuery()
        {
            return "SELECT * FROM CONTROL_PLANTA WHERE idControlPlanta = @Id";
        }

        /// <summary>
        /// Obtiene la query para recuperar todos los registros de ControlPlanta en MySQL.
        /// </summary>
        public string GetAllQuery()
        {
            return "SELECT * FROM CONTROL_PLANTA";
        }

        /// <summary>
        /// Obtiene la query para actualizar un ControlPlanta en MySQL.
        /// </summary>
        public string UpdateQuery()
        {
            return @"
                UPDATE CONTROL_PLANTA SET
                    idControl = @IdControl,
                    idCompany = @IdCompany
                WHERE idControlPlanta = @IdControlPlanta";
        }

        /// <summary>
        /// Obtiene la query para eliminar un ControlPlanta en MySQL.
        /// </summary>
        public string DeleteQuery()
        {
            return "DELETE FROM CONTROL_PLANTA WHERE idControlPlanta = @Id";
        }
    }
}
