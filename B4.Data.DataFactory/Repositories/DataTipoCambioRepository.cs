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
    public sealed class DataTipoCambioRepository : IDataTipoCambioRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public DataTipoCambioRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(DataTipoCambio entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try { using var conn = _connectionFactory.CreateConnection(); await conn.ExecuteAsync(_queryProvider.DataTipoCambioQueries.AddQuery(), entity); }
            catch (Exception ex) { throw new Exception("Error al insertar un TipoCambio.", ex); }
        }

        public async Task<DataTipoCambio?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<DataTipoCambio>(_queryProvider.DataTipoCambioQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener TipoCambio con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataTipoCambio>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataTipoCambio>(_queryProvider.DataTipoCambioQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de TipoCambio.", ex); }
        }

        public async Task UpdateAsync(DataTipoCambio entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataTipoCambioQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro TipoCambio con id {entity.Id} para actualizar.");
            }
            catch (Exception ex) { throw new Exception($"Error al actualizar TipoCambio con id {entity.Id}.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataTipoCambioQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro TipoCambio con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar TipoCambio con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataTipoCambio>> GetByEjercicioAsync(int ejercicio)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataTipoCambio>(_queryProvider.DataTipoCambioQueries.GetByEjercicioQuery(), new { ejercicio }); }
            catch (Exception ex) { throw new Exception($"Error al obtener TipoCambio para ejercicio {ejercicio}.", ex); }
        }

        public async Task<IEnumerable<DataTipoCambio>> GetByEjercicioCurrencyAsync(int ejercicio, int idCurrency)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataTipoCambio>(_queryProvider.DataTipoCambioQueries.GetByEjercicioCurrencyQuery(), new { ejercicio, idCurrency }); }
            catch (Exception ex) { throw new Exception($"Error al obtener TipoCambio para ejercicio {ejercicio} currency {idCurrency}.", ex); }
        }
    }
}
