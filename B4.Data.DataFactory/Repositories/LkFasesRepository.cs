using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Data.DataFactory.Connections;
using B4.Data.DataFactory.Providers;
using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;
using Dapper;

namespace B4.Data.DataFactory.Repositories
{
    public sealed class LkFasesRepository : IFasesRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public LkFasesRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(LkFases entity)
        {
            try { using var conn = _connectionFactory.CreateConnection(); await conn.ExecuteAsync(_queryProvider.LkFasesQueries.AddQuery(), entity); }
            catch (Exception ex) { throw new Exception("Error al insertar Fase.", ex); }
        }

        public async Task<LkFases?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<LkFases>(_queryProvider.LkFasesQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener Fase con id {id}.", ex); }
        }

        public async Task<IEnumerable<LkFases>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<LkFases>(_queryProvider.LkFasesQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de Fase.", ex); }
        }

        public async Task UpdateAsync(LkFases entity)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkFasesQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro Fase para actualizar.");
            }
            catch (Exception ex) { throw new Exception("Error al actualizar Fase.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkFasesQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro Fase con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar Fase con id {id}.", ex); }
        }
    }
}
