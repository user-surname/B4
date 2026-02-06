using B4.Data.MySQL;
using B4.Models.Entities;
using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace B4.Data.MySQL.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private const string _tableName = "USUARIOS";

        private readonly MySQLDapperContext _context;

        public UsuarioRepository(MySQLDapperContext context)
        {
            _context = context;
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
        public async Task<Usuario?> GetByEmailAsync(String email)
        {
            // 1️ Crear la conexión
            using var connection = _context.CreateConnection();

            // 2️ Definir el query SQL
            string sql = $@"
            SELECT id, email, hashed_password AS HashedPassword, role
            FROM {_tableName}
            WHERE email = @Email;
        ";

            // 3️ Ejecutar el query con Dapper
            var usuario = await connection.QuerySingleOrDefaultAsync<Usuario>(
                sql,
                new { Email = email } // parámetro seguro contra SQL injection
            );

            // 4️ Retornar el usuario o null si no existe
            return usuario;
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
