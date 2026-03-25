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
    public sealed class DataForecastBwRepository : IDataForecastBwRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public DataForecastBwRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(DataForecastBw entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                entity.RecalculateFinancialFields(increaseVersion: false);
                using var conn = _connectionFactory.CreateConnection();
                await conn.ExecuteAsync(_queryProvider.DataForecastBwQueries.AddQuery(), entity);
            }
            catch (Exception ex) { throw new Exception("Error al insertar DataForecastBw.", ex); }
        }

        public async Task<DataForecastBw?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<DataForecastBw>(_queryProvider.DataForecastBwQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener DataForecastBw con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataForecastBw>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataForecastBw>(_queryProvider.DataForecastBwQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de DataForecastBw.", ex); }
        }

        public async Task UpdateAsync(DataForecastBw entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                entity.RecalculateFinancialFields(increaseVersion: false);
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataForecastBwQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro DataForecastBw con id {entity.Id} para actualizar.");
            }
            catch (Exception ex) { throw new Exception($"Error al actualizar DataForecastBw con id {entity.Id}.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataForecastBwQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro DataForecastBw con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar DataForecastBw con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataForecastBw>> GetByPlantaEjercicioAsync(int planta, int ejercicio)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataForecastBw>(_queryProvider.DataForecastBwQueries.GetByPlantaEjercicioQuery(), new { planta, ejercicio }); }
            catch (Exception ex) { throw new Exception($"Error al obtener DataForecastBw para planta {planta} ejercicio {ejercicio}.", ex); }
        }
    }
}
