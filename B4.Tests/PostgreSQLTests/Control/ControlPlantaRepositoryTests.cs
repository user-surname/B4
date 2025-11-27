using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class ControlPlantaRepository : IControlPlantaRepository
    {
        private readonly DapperContext _context;

        public ControlPlantaRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ControlPlanta entity)
        {
            const string sql = @"
                INSERT INTO b4.control_planta
                (idcontrol, idcompany)
                VALUES (@IdControl, @IdCompany);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task<ControlPlanta?> GetByIdAsync(int id)
        {
            const string sql =
                "SELECT * FROM b4.control_planta WHERE idcontrolplanta=@Id;";

            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<ControlPlanta>(sql, new { Id = id });
        }

        public async Task<IEnumerable<ControlPlanta>> GetAllAsync()
        {
            const string sql = "SELECT * FROM b4.control_planta;";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<ControlPlanta>(sql);
        }

        public async Task UpdateAsync(ControlPlanta entity)
        {
            const string sql = @"
                UPDATE b4.control_planta SET
                    idcontrol=@IdControl,
                    idcompany=@IdCompany
                WHERE idcontrolplanta=@IdControlPlanta;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            const string sql =
                "DELETE FROM b4.control_planta WHERE idcontrolplanta=@Id;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
