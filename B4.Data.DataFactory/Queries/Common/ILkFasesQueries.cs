namespace B4.Data.DataFactory.Queries.Common
{
    /// <summary>
    /// Define las queries SQL que necesita el repositorio de LkFases.
    /// </summary>
    public interface ILkFasesQueries
    {
        string AddQuery();
        string GetByIdQuery();
        string GetAllQuery();
        string UpdateQuery();
        string DeleteQuery();
    }
}
