using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Data.DataFactory.Connections;
using B4.Data.DataFactory.Providers;
using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;
using Dapper;

namespace B4.Data.DataFactory.Repositories
{
    public sealed class StgDataBridgesFyRepository : IStgDataBridgesFyRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public StgDataBridgesFyRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(StgDataBridgesFy entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                await conn.ExecuteAsync(_queryProvider.StgDataBridgesFyQueries.AddQuery(), entity);
            }
            catch (Exception ex) { throw new Exception("Error al insertar StgDataBridgesFy.", ex); }
        }

        public async Task<StgDataBridgesFy?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<StgDataBridgesFy>(_queryProvider.StgDataBridgesFyQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener StgDataBridgesFy con id {id}.", ex); }
        }

        public async Task<IEnumerable<StgDataBridgesFy>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<StgDataBridgesFy>(_queryProvider.StgDataBridgesFyQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de StgDataBridgesFy.", ex); }
        }

        public async Task UpdateAsync(StgDataBridgesFy entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.StgDataBridgesFyQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro StgDataBridgesFy con id {entity.Id} para actualizar.");
            }
            catch (Exception ex) { throw new Exception($"Error al actualizar StgDataBridgesFy con id {entity.Id}.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.StgDataBridgesFyQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro StgDataBridgesFy con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar StgDataBridgesFy con id {id}.", ex); }
        }

        public async Task<IEnumerable<StgDataBridgesFy>> GetByPlantaEjercicioAsync(int planta, int ejercicio)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<StgDataBridgesFy>(_queryProvider.StgDataBridgesFyQueries.GetByPlantaEjercicioQuery(), new { planta, ejercicio }); }
            catch (Exception ex) { throw new Exception($"Error al obtener StgDataBridgesFy para planta {planta} ejercicio {ejercicio}.", ex); }
        }
    }
}
