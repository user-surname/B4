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
    public sealed class DataBudgetBwRepository : IDataBudgetBwRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public DataBudgetBwRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(DataBudgetBw entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                entity.RecalculateFinancialFields(increaseVersion: false);
                using var conn = _connectionFactory.CreateConnection();
                await conn.ExecuteAsync(_queryProvider.DataBudgetBwQueries.AddQuery(), entity);
            }
            catch (Exception ex) { throw new Exception("Error al insertar DataBudgetBw.", ex); }
        }

        public async Task<DataBudgetBw?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<DataBudgetBw>(_queryProvider.DataBudgetBwQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener DataBudgetBw con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataBudgetBw>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataBudgetBw>(_queryProvider.DataBudgetBwQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de DataBudgetBw.", ex); }
        }

        public async Task UpdateAsync(DataBudgetBw entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                entity.RecalculateFinancialFields(increaseVersion: false);
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataBudgetBwQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro DataBudgetBw con id {entity.Id} para actualizar.");
            }
            catch (Exception ex) { throw new Exception($"Error al actualizar DataBudgetBw con id {entity.Id}.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataBudgetBwQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro DataBudgetBw con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar DataBudgetBw con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataBudgetBw>> GetByPlantaEjercicioAsync(int planta, int ejercicio)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataBudgetBw>(_queryProvider.DataBudgetBwQueries.GetByPlantaEjercicioQuery(), new { planta, ejercicio }); }
            catch (Exception ex) { throw new Exception($"Error al obtener DataBudgetBw para planta {planta} ejercicio {ejercicio}.", ex); }
        }
    }
}
