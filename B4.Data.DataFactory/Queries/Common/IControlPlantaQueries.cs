namespace B4.Data.DataFactory.Queries.Common
{
    /// <summary>
    /// Defines SQL query contracts for ControlPlanta operations.
    /// </summary>
    public interface IControlPlantaQueries
    {
        /// <summary>
        /// Gets the query used to insert a ControlPlanta.
        /// </summary>
        string AddQuery();

        /// <summary>
        /// Gets the query used to retrieve a ControlPlanta by identifier.
        /// </summary>
        string GetByIdQuery();

        /// <summary>
        /// Gets the query used to retrieve all ControlPlanta records.
        /// </summary>
        string GetAllQuery();

        /// <summary>
        /// Gets the query used to update a ControlPlanta.
        /// </summary>
        string UpdateQuery();

        /// <summary>
        /// Gets the query used to delete a ControlPlanta.
        /// </summary>
        string DeleteQuery();
    }
}
