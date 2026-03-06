using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Providers
{
    /// <summary>
    /// Exposes query groups resolved for the active database provider.
    /// </summary>
    public interface IDataQueryProvider
    {
        /// <summary>
        /// Gets Usuario queries for the active provider.
        /// </summary>
        IUsuarioQueries UsuarioQueries { get; }
    }
}
