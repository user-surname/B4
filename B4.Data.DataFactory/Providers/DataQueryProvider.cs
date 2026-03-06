using System;
using B4.Data.DataFactory.Configuration;
using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Providers
{
    /// <summary>
    /// Base provider resolver placeholder for query collections by database engine.
    /// </summary>
    public sealed class DataQueryProvider : IDataQueryProvider
    {
        private readonly DataFactoryOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQueryProvider"/> class.
        /// </summary>
        /// <param name="options">DataFactory options.</param>
        public DataQueryProvider(DataFactoryOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Gets Usuario queries for the selected provider.
        /// </summary>
        public IUsuarioQueries UsuarioQueries =>
            throw new NotImplementedException(
                $"Query provider is not implemented for provider '{_options.Provider}'.");
    }
}
