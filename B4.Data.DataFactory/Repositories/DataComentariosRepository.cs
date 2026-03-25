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
    public sealed class DataComentariosRepository : IDataComentariosRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;

        public DataComentariosRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }

        public async Task AddAsync(DataComentarios entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try { using var conn = _connectionFactory.CreateConnection(); await conn.ExecuteAsync(_queryProvider.DataComentariosQueries.AddQuery(), entity); }
            catch (Exception ex) { throw new Exception("Error al insertar un comentario.", ex); }
        }

        public async Task<DataComentarios?> GetByIdAsync(int id)
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QuerySingleOrDefaultAsync<DataComentarios>(_queryProvider.DataComentariosQueries.GetByIdQuery(), new { Id = id }); }
            catch (Exception ex) { throw new Exception($"Error al obtener el comentario con id {id}.", ex); }
        }

        public async Task<IEnumerable<DataComentarios>> GetAllAsync()
        {
            try { using var conn = _connectionFactory.CreateConnection(); return await conn.QueryAsync<DataComentarios>(_queryProvider.DataComentariosQueries.GetAllQuery()); }
            catch (Exception ex) { throw new Exception("Error al obtener la lista de comentarios.", ex); }
        }

        public async Task UpdateAsync(DataComentarios entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataComentariosQueries.UpdateQuery(), entity);
                if (rows == 0) throw new Exception($"No se encontro el comentario con id {entity.Id} para actualizar.");
            }
            catch (Exception ex) { throw new Exception($"Error al actualizar el comentario con id {entity.Id}.", ex); }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                var rows = await conn.ExecuteAsync(_queryProvider.DataComentariosQueries.DeleteQuery(), new { Id = id });
                if (rows == 0) throw new Exception($"No se encontro el comentario con id {id} para eliminar.");
            }
            catch (Exception ex) { throw new Exception($"Error al eliminar el comentario con id {id}.", ex); }
        }
    }
}
