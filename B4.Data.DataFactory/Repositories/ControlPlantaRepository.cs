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
    /// Repositorio de ControlPlanta migrado a DataFactory.
    /// Obtiene la conexion desde IDbConnectionFactory y el SQL desde IDataQueryProvider.
    /// </summary>
    public sealed class ControlPlantaRepository : IControlPlantaRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        /// <summary>
        /// Inicializa el repositorio con los componentes comunes de DataFactory.
        /// </summary>
        /// <param name="connectionFactory">Factoría de conexiones a base de datos.</param>
        /// <param name="queryProvider">Origen de queries según proveedor.</param>
        public ControlPlantaRepository(
            IDbConnectionFactory connectionFactory,
            IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        /// <summary>
        /// Inserta un nuevo ControlPlanta.
        /// </summary>
        /// <param name="entity">Entidad a insertar.</param>
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
        /// Recupera un ControlPlanta por identificador.
        /// </summary>
        /// <param name="id">Identificador a buscar.</param>
        /// <returns>La entidad encontrada o null.</returns>
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
        /// Recupera todos los registros de ControlPlanta.
        /// </summary>
        /// <returns>Colección de entidades.</returns>
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
        /// Actualiza un ControlPlanta existente.
        /// </summary>
        /// <param name="entity">Entidad a actualizar.</param>
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
        /// Elimina un ControlPlanta existente.
        /// </summary>
        /// <param name="id">Identificador a eliminar.</param>
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
