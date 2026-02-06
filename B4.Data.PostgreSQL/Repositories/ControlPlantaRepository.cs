using Dapper;
using B4.Models.Entities;
using B4.Models.RepositoryInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class ControlPlantaRepository : IControlPlantaRepository
    {
        private readonly PostgreSQLDapperContext _context;

        public ControlPlantaRepository(PostgreSQLDapperContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ControlPlanta entity)
        {
            const string sql = @"
                INSERT INTO control_planta
                (idcontrol, idcompany)
                VALUES (@IdControl, @IdCompany)";
            
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task<ControlPlanta?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM control_planta WHERE idcontrolplanta=@Id";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<ControlPlanta>(sql, new { Id = id });
        }

        public async Task<IEnumerable<ControlPlanta>> GetAllAsync()
        {
            const string sql = "SELECT * FROM control_planta";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<ControlPlanta>(sql);
        }

        public async Task UpdateAsync(ControlPlanta entity)
        {
            const string sql = @"
                UPDATE control_planta SET
                    idcontrol=@IdControl,
                    idcompany=@IdCompany
                WHERE idcontrolplanta=@IdControlPlanta";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync("DELETE FROM control_planta WHERE idcontrolplanta=@Id", new { Id = id });
        }
    }
}
