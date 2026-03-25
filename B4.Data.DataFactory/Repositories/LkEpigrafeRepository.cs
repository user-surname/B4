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
    public sealed class LkEpigrafeRepository : IEpigrafeRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public LkEpigrafeRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(LkEpigrafe entity)
        {
            try { using var conn = _connectionFactory.CreateConnection(); await conn.ExecuteAsync(_queryProvider.LkEpigrafeQueries.AddQuery(), entity); }
            catch (Exception ex) { throw new Exception("Error al insertar Epigrafe.", ex); }
        }

        public async Task<LkEpigrafe?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<LkEpigrafe>(_queryProvider.LkEpigrafeQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener Epigrafe con id {id}.", ex); }
        }

        public async Task<IEnumerable<LkEpigrafe>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<LkEpigrafe>(_queryProvider.LkEpigrafeQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de Epigrafe.", ex); }
        }

        public async Task UpdateAsync(LkEpigrafe entity)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkEpigrafeQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro Epigrafe para actualizar.");
            }
            catch (Exception ex) { throw new Exception("Error al actualizar Epigrafe.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.LkEpigrafeQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro Epigrafe con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar Epigrafe con id {id}.", ex); }
        }
    }
}
