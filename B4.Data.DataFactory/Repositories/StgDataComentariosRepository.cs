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
    public sealed class StgDataComentariosRepository : IStgDataComentariosRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public StgDataComentariosRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(StgDataComentarios entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try { using var conn = _connectionFactory.CreateConnection(); await conn.ExecuteAsync(_queryProvider.StgDataComentariosQueries.AddQuery(), entity); }
            catch (Exception ex) { throw new Exception("Error al insertar un comentario.", ex); }
        }

        public async Task<StgDataComentarios?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<StgDataComentarios>(_queryProvider.StgDataComentariosQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener el comentario con id {id}.", ex); }
        }

        public async Task<IEnumerable<StgDataComentarios>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<StgDataComentarios>(_queryProvider.StgDataComentariosQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de comentarios.", ex); }
        }

        public async Task UpdateAsync(StgDataComentarios entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.StgDataComentariosQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro el comentario con id {entity.Id} para actualizar.");
            }
            catch (Exception ex) { throw new Exception($"Error al actualizar el comentario con id {entity.Id}.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.StgDataComentariosQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro el comentario con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar el comentario con id {id}.", ex); }
        }
    }
}
