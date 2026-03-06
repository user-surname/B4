using System.Data;

namespace B4.Data.DataFactory.Connections
{
    /// <summary>
    /// Exposes a factory contract to create database connections.
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Creates a new database connection instance.
        /// </summary>
        /// <returns>A database connection.</returns>
        IDbConnection CreateConnection();
    }
}
