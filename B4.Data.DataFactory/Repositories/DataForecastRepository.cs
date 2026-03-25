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
    public sealed class DataForecastRepository : IDataForecastRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public DataForecastRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(DataForecast entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                entity.RecalculateFinancialFields(increaseVersion: false);
                using var conn = _connectionFactory.CreateConnection();
                await conn.ExecuteAsync(_queryProvider.DataForecastQueries.AddQuery(), entity);
            }
            catch (Exception ex) { throw new Exception("Error al insertar DataForecast.", ex); }
        }

        public async Task<DataForecast?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<DataForecast>(_queryProvider.DataForecastQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener DataForecast con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataForecast>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataForecast>(_queryProvider.DataForecastQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de DataForecast.", ex); }
        }

        public async Task UpdateAsync(DataForecast entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                entity.RecalculateFinancialFields(increaseVersion: false);
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataForecastQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro DataForecast con id {entity.Id} para actualizar.");
            }
            catch (Exception ex) { throw new Exception($"Error al actualizar DataForecast con id {entity.Id}.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataForecastQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro DataForecast con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar DataForecast con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataForecast>> GetByPlantaEjercicioAsync(int planta, int ejercicio)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataForecast>(_queryProvider.DataForecastQueries.GetByPlantaEjercicioQuery(), new { planta, ejercicio }); }
            catch (Exception ex) { throw new Exception($"Error al obtener DataForecast para planta {planta} ejercicio {ejercicio}.", ex); }
        }
    }
}
