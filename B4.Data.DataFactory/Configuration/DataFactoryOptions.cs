namespace B4.Data.DataFactory.Configuration
{
    /// <summary>
    /// Defines configurable options for DataFactory provider resolution.
    /// </summary>
    public sealed class DataFactoryOptions
    {
        /// <summary>
        /// Gets or sets the database provider key (for example: mysql, postgresql, mssql).
        /// </summary>
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the connection string used by the selected provider.
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;
    }
}
