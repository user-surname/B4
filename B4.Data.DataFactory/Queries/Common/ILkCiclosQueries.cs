namespace B4.Data.DataFactory.Queries.Common
{
    /// <summary>
    /// Define las queries SQL que necesita el repositorio de LkCiclos.
    /// </summary>
    public interface ILkCiclosQueries
    {
        string AddQuery();
        string GetByIdQuery();
        string GetAllQuery();
        string UpdateQuery();
        string DeleteQuery();
    }
}
