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
    public sealed class StgDataForecastRepository : IStgDataForecastRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public StgDataForecastRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(StgDataForecast entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                entity.RecalculateFinancialFields(increaseVersion: false);
                using var conn = _connectionFactory.CreateConnection();
                await conn.ExecuteAsync(_queryProvider.StgDataForecastQueries.AddQuery(), entity);
            }
            catch (Exception ex) { throw new Exception("Error al insertar StgDataForecast.", ex); }
        }

        public async Task<StgDataForecast?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<StgDataForecast>(_queryProvider.StgDataForecastQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener StgDataForecast con id {id}.", ex); }
        }

        public async Task<IEnumerable<StgDataForecast>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<StgDataForecast>(_queryProvider.StgDataForecastQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de StgDataForecast.", ex); }
        }

        public async Task UpdateAsync(StgDataForecast entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                entity.RecalculateFinancialFields(increaseVersion: false);
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.StgDataForecastQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro StgDataForecast con id {entity.Id} para actualizar.");
            }
            catch (Exception ex) { throw new Exception($"Error al actualizar StgDataForecast con id {entity.Id}.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.StgDataForecastQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro StgDataForecast con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar StgDataForecast con id {id}.", ex); }
        }

        public async Task<IEnumerable<StgDataForecast>> GetByPlantaEjercicioAsync(int planta, int ejercicio)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<StgDataForecast>(_queryProvider.StgDataForecastQueries.GetByPlantaEjercicioQuery(), new { planta, ejercicio }); }
            catch (Exception ex) { throw new Exception($"Error al obtener StgDataForecast para planta {planta} ejercicio {ejercicio}.", ex); }
        }
    }
}
