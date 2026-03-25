namespace B4.Data.DataFactory.Queries.Common
{
    /// <summary>
    /// Define las queries SQL que necesita el repositorio de LkEpigrafe.
    /// </summary>
    public interface ILkEpigrafeQueries
    {
        string AddQuery();
        string GetByIdQuery();
        string GetAllQuery();
        string UpdateQuery();
        string DeleteQuery();
    }
}
