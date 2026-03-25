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
    public sealed class DataBridgesMonthRepository : IDataBridgesMonthRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public DataBridgesMonthRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(DataBridgesMonth entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                await conn.ExecuteAsync(_queryProvider.DataBridgesMonthQueries.AddQuery(), entity);
            }
            catch (Exception ex) { throw new Exception("Error al insertar DataBridgesMonth.", ex); }
        }

        public async Task<DataBridgesMonth?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<DataBridgesMonth>(_queryProvider.DataBridgesMonthQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener DataBridgesMonth con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataBridgesMonth>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataBridgesMonth>(_queryProvider.DataBridgesMonthQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de DataBridgesMonth.", ex); }
        }

        public async Task UpdateAsync(DataBridgesMonth entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataBridgesMonthQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro DataBridgesMonth con id {entity.Id} para actualizar.");
            }
            catch (Exception ex) { throw new Exception($"Error al actualizar DataBridgesMonth con id {entity.Id}.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataBridgesMonthQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro DataBridgesMonth con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar DataBridgesMonth con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataBridgesMonth>> GetByPlantaEjercicioAsync(int planta, int ejercicio)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataBridgesMonth>(_queryProvider.DataBridgesMonthQueries.GetByPlantaEjercicioQuery(), new { planta, ejercicio }); }
            catch (Exception ex) { throw new Exception($"Error al obtener DataBridgesMonth para planta {planta} ejercicio {ejercicio}.", ex); }
        }
    }
}
