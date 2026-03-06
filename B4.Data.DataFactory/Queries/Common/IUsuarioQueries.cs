namespace B4.Data.DataFactory.Queries.Common
{
    /// <summary>
    /// Defines SQL query contracts for Usuario operations.
    /// </summary>
    public interface IUsuarioQueries
    {
        /// <summary>
        /// Gets the query used to retrieve a Usuario by identifier.
        /// </summary>
        /// <returns>SQL query text.</returns>
        string GetByIdQuery();
    }
}
