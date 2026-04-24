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
    public sealed class StgDataBudgetRepository : IStgDataBudgetRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public StgDataBudgetRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(StgDataBudget entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                entity.RecalculateFinancialFields(increaseVersion: false);
                using var conn = _connectionFactory.CreateConnection();
                await conn.ExecuteAsync(_queryProvider.StgDataBudgetQueries.AddQuery(), entity);
            }
            catch (Exception ex) { throw new Exception("Error al insertar StgDataBudget.", ex); }
        }

        public async Task<StgDataBudget?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<StgDataBudget>(_queryProvider.StgDataBudgetQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener StgDataBudget con id {id}.", ex); }
        }

        public async Task<IEnumerable<StgDataBudget>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<StgDataBudget>(_queryProvider.StgDataBudgetQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de StgDataBudget.", ex); }
        }

        public async Task UpdateAsync(StgDataBudget entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                entity.RecalculateFinancialFields(increaseVersion: false);
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.StgDataBudgetQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro StgDataBudget con id {entity.Id} para actualizar.");
            }
            catch (Exception ex) { throw new Exception($"Error al actualizar StgDataBudget con id {entity.Id}.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.StgDataBudgetQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro StgDataBudget con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar StgDataBudget con id {id}.", ex); }
        }

        public async Task<IEnumerable<StgDataBudget>> GetByPlantaEjercicioAsync(int planta, int ejercicio)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<StgDataBudget>(_queryProvider.StgDataBudgetQueries.GetByPlantaEjercicioQuery(), new { planta, ejercicio }); }
            catch (Exception ex) { throw new Exception($"Error al obtener StgDataBudget para planta {planta} ejercicio {ejercicio}.", ex); }
        }
    }
}
