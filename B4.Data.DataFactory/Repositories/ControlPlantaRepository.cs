using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Data.DataFactory.Connections;
using B4.Data.DataFactory.Providers;
using B4.Models.Entities;
using B4.Models.RepositoryInterfaces;
using Dapper;

namespace B4.Data.DataFactory.Repositories
{
    /// <summary>
    /// Pilot ControlPlanta repository backed by DataFactory connections and queries.
    /// </summary>
    public sealed class ControlPlantaRepository : IControlPlantaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlPlantaRepository"/> class.
        /// </summary>
        /// <param name="connectionFactory">Database connection factory.</param>
        /// <param name="queryProvider">Provider-specific SQL query source.</param>
        public ControlPlantaRepository(
            IDbConnectionFactory connectionFactory,
            IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        /// <summary>
        /// Inserts a new ControlPlanta.
        /// </summary>
        /// <param name="entity">Entity to insert.</param>
        public async Task AddAsync(ControlPlanta entity)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                await conn.ExecuteAsync(_queryProvider.ControlPlantaQueries.AddQuery(), entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un ControlPlanta en la base de datos.", ex);
            }
        }

        /// <summary>
        /// Retrieves a ControlPlanta by identifier.
        /// </summary>
        /// <param name="id">Identifier to search.</param>
        /// <returns>The matching entity or null.</returns>
        public async Task<ControlPlanta?> GetByIdAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<ControlPlanta>(
                    _queryProvider.ControlPlantaQueries.GetByIdQuery(),
                    new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el ControlPlanta con id {id}.", ex);
            }
        }

        /// <summary>
        /// Retrieves all ControlPlanta records.
        /// </summary>
        /// <returns>Collection of entities.</returns>
        public async Task<IEnumerable<ControlPlanta>> GetAllAsync()
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                return await conn.QueryAsync<ControlPlanta>(
                    _queryProvider.ControlPlantaQueries.GetAllQuery());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de ControlPlanta.", ex);
            }
        }

        /// <summary>
        /// Updates an existing ControlPlanta.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        public async Task UpdateAsync(ControlPlanta entity)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.ControlPlantaQueries.UpdateQuery(), entity);

                if (rows == 0)
                {
                    throw new Exception(
                        $"No se encontro el ControlPlanta con id {entity.IdControlPlanta} para actualizar.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Error al actualizar el ControlPlanta con id {entity.IdControlPlanta}.",
                    ex);
            }
        }

        /// <summary>
        /// Deletes an existing ControlPlanta.
        /// </summary>
        /// <param name="id">Identifier to delete.</param>
        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(
                    _queryProvider.ControlPlantaQueries.DeleteQuery(),
                    new { Id = id });

                if (rows == 0)
                {
                    throw new Exception($"No se encontro el ControlPlanta con id {id} para eliminar.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el ControlPlanta con id {id}.", ex);
            }
        }
    }
}
