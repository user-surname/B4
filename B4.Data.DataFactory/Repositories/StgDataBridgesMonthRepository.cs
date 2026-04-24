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
    public sealed class StgDataBridgesMonthRepository : IStgDataBridgesMonthRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public StgDataBridgesMonthRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(StgDataBridgesMonth entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                await conn.ExecuteAsync(_queryProvider.StgDataBridgesMonthQueries.AddQuery(), entity);
            }
            catch (Exception ex) { throw new Exception("Error al insertar StgDataBridgesMonth.", ex); }
        }

        public async Task<StgDataBridgesMonth?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<StgDataBridgesMonth>(_queryProvider.StgDataBridgesMonthQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener StgDataBridgesMonth con id {id}.", ex); }
        }

        public async Task<IEnumerable<StgDataBridgesMonth>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<StgDataBridgesMonth>(_queryProvider.StgDataBridgesMonthQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de StgDataBridgesMonth.", ex); }
        }

        public async Task UpdateAsync(StgDataBridgesMonth entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.StgDataBridgesMonthQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro StgDataBridgesMonth con id {entity.Id} para actualizar.");
            }
            catch (Exception ex) { throw new Exception($"Error al actualizar StgDataBridgesMonth con id {entity.Id}.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.StgDataBridgesMonthQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro StgDataBridgesMonth con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar StgDataBridgesMonth con id {id}.", ex); }
        }

        public async Task<IEnumerable<StgDataBridgesMonth>> GetByPlantaEjercicioAsync(int planta, int ejercicio)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<StgDataBridgesMonth>(_queryProvider.StgDataBridgesMonthQueries.GetByPlantaEjercicioQuery(), new { planta, ejercicio }); }
            catch (Exception ex) { throw new Exception($"Error al obtener StgDataBridgesMonth para planta {planta} ejercicio {ejercicio}.", ex); }
        }
    }
}
