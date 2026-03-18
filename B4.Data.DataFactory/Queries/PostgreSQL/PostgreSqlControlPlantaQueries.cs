using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    /// <summary>
    /// Implementa la version PostgreSQL de las queries de ControlPlanta.
    /// DataQueryProvider devuelve esta clase cuando "bbdd" vale "PostgreSQL".
    /// </summary>
    public sealed class PostgreSqlControlPlantaQueries : IControlPlantaQueries
    {
        /// <summary>
        /// Obtiene la query para insertar un ControlPlanta en PostgreSQL.
        /// </summary>
        public string AddQuery()
        {
            return @"
                INSERT INTO control_planta
                (idcontrol, idcompany)
                VALUES (@IdControl, @IdCompany)";
        }

        /// <summary>
        /// Obtiene la query para recuperar un ControlPlanta por identificador en PostgreSQL.
        /// </summary>
        public string GetByIdQuery()
        {
            return "SELECT * FROM control_planta WHERE idcontrolplanta = @Id";
        }

        /// <summary>
        /// Obtiene la query para recuperar todos los registros de ControlPlanta en PostgreSQL.
        /// </summary>
        public string GetAllQuery()
        {
            return "SELECT * FROM control_planta";
        }

        /// <summary>
        /// Obtiene la query para actualizar un ControlPlanta en PostgreSQL.
        /// </summary>
        public string UpdateQuery()
        {
            return @"
                UPDATE control_planta SET
                    idcontrol = @IdControl,
                    idcompany = @IdCompany
                WHERE idcontrolplanta = @IdControlPlanta";
        }

        /// <summary>
        /// Obtiene la query para eliminar un ControlPlanta en PostgreSQL.
        /// </summary>
        public string DeleteQuery()
        {
            return "DELETE FROM control_planta WHERE idcontrolplanta = @Id";
        }
    }
}
