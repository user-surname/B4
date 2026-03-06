using System;
using System.Data;
using B4.Data.DataFactory.Configuration;

namespace B4.Data.DataFactory.Connections
{
    /// <summary>
    /// Base connection factory placeholder for provider-specific connections.
    /// </summary>
    public sealed class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly DataFactoryOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionFactory"/> class.
        /// </summary>
        /// <param name="options">DataFactory options.</param>
        public DbConnectionFactory(DataFactoryOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Creates a provider-specific database connection.
        /// </summary>
        /// <returns>A database connection instance.</returns>
        public IDbConnection CreateConnection()
        {
            throw new NotImplementedException(
                $"Connection factory is not implemented for provider '{_options.Provider}'.");
        }
    }
}
