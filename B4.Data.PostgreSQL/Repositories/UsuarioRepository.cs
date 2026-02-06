using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces;
using Npgsql;
using System;
using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace B4.Data.PostgreSQL.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        // La cadena de conexión se inyecta al crear el repositorio
        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Implementacion de IRepository
        // C - Create
        public async Task AddAsync(Usuario entity)
        {
            throw new NotImplementedException();
        }

        // R - Read
        public async Task<Usuario?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        // U - Update
        public async Task UpdateAsync(Usuario entity)
        {
            throw new NotImplementedException();
        }

        // D - Delete
        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


        // Implementacion de IUsuarioRepository

        // Métodos específicos que no son genéricos
        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Usuario>> GetUsersByRoleAsync(string role)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Usuario>> FindWithOrdersAsync(int userId)
        {
            throw new NotImplementedException();
        }

    }
}
