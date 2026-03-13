using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    /// <summary>
    /// PostgreSQL-specific ControlPlanta query definitions.
    /// </summary>
    public sealed class PostgreSqlControlPlantaQueries : IControlPlantaQueries
    {
        /// <summary>
        /// Gets the query used to insert a ControlPlanta.
        /// </summary>
        public string AddQuery()
        {
            return @"
                INSERT INTO control_planta
                (idcontrol, idcompany)
                VALUES (@IdControl, @IdCompany)";
        }

        /// <summary>
        /// Gets the query used to retrieve a ControlPlanta by identifier.
        /// </summary>
        public string GetByIdQuery()
        {
            return "SELECT * FROM control_planta WHERE idcontrolplanta = @Id";
        }

        /// <summary>
        /// Gets the query used to retrieve all ControlPlanta records.
        /// </summary>
        public string GetAllQuery()
        {
            return "SELECT * FROM control_planta";
        }

        /// <summary>
        /// Gets the query used to update a ControlPlanta.
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
        /// Gets the query used to delete a ControlPlanta.
        /// </summary>
        public string DeleteQuery()
        {
            return "DELETE FROM control_planta WHERE idcontrolplanta = @Id";
        }
    }
}
