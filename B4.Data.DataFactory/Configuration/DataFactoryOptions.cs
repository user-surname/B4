namespace B4.Data.DataFactory.Configuration
{
    /// <summary>
    /// Stores provider and connection strings used by DataFactory.
    /// </summary>
    public sealed class DataFactoryOptions
    {
        /// <summary>
        /// Configuration key used to select the active provider.
        /// </summary>
        public const string ProviderConfigurationKey = "bbdd";

        /// <summary>
        /// Connection string name for MySQL data database.
        /// </summary>
        public const string MySqlConnectionStringName = "MySQLConnectionB4Data";

        /// <summary>
        /// Connection string name for PostgreSQL data database.
        /// </summary>
        public const string PostgreSqlConnectionStringName = "PostgresConnectionB4Data";

        /// <summary>
        /// Connection string name reserved for future SQL Server support.
        /// </summary>
        public const string SqlServerConnectionStringName = "SqlServerConnectionB4Data";

        /// <summary>
        /// Gets or sets the active provider value read from configuration.
        /// </summary>
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the MySQL connection string.
        /// </summary>
        public string MySqlConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the PostgreSQL connection string.
        /// </summary>
        public string PostgreSqlConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the SQL Server connection string for future usage.
        /// </summary>
        public string SqlServerConnectionString { get; set; } = string.Empty;
    }
}
