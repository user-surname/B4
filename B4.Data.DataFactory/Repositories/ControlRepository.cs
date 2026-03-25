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
    public sealed class ControlRepository : IControlRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public ControlRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(Control entity)
        {
            try { using var conn = _connectionFactory.CreateConnection(); await conn.ExecuteAsync(_queryProvider.ControlQueries.AddQuery(), entity); }
            catch (Exception ex) { throw new Exception("Error al insertar un Control.", ex); }
        }

        public async Task<Control?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<Control>(_queryProvider.ControlQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener el Control con id {id}.", ex); }
        }

        public async Task<IEnumerable<Control>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<Control>(_queryProvider.ControlQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de Controles.", ex); }
        }

        public async Task UpdateAsync(Control entity)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.ControlQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro el Control con id {entity.IdControl} para actualizar.");
            }
            catch (Exception ex) { throw new Exception($"Error al actualizar el Control con id {entity.IdControl}.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.ControlQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro el Control con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar el Control con id {id}.", ex); }
        }
    }
}
