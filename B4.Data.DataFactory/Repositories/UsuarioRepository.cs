using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using B4.Data.DataFactory.Connections;
using B4.Data.DataFactory.Providers;
using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces;
using Dapper;


namespace B4.Data.DataFactory.Repositories
{
    public sealed class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IDataQueryProvider _queryProvider;


        public UsuarioRepository(IDbConnectionFactory connectionFactory, IDataQueryProvider queryProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _queryProvider = queryProvider ?? throw new ArgumentNullException(nameof(queryProvider));
        }


        public Task AddAsync(Usuario entity)
        {
            throw new NotImplementedException();
        }


        public async Task<Usuario?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();

        }


        public Task<IEnumerable<Usuario>> GetAllAsync()
        {
            throw new NotImplementedException();
        }


        public Task UpdateAsync(Usuario entity)
        {
            throw new NotImplementedException();
        }


        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            try
            {
                using var conn = _connectionFactory.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<Usuario>(
                    _queryProvider.UsuarioQueries.GetByEmailQuery(),
                    new { Email = email });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Usuario por email '{email}'.", ex);
            }
        }


        public Task<IEnumerable<Usuario>> GetUsersByRoleAsync(string role)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<Usuario>> FindWithOrdersAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}





