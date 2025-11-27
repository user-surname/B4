using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantControllersRepository : IPlantControllersRepository
    {
        private readonly DapperContext _context;

        public PlantControllersRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LkPlantControllers entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plant_controllers
                (idcompanycontroller, idcompany, controller, email)
                VALUES
                (@IdCompanyController, @IdCompany, @Controller, @Email);";

            using var conn =
                _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task<LkPlantControllers?> GetByIdAsync(int id)
        {
            const string sql =
                "SELECT * FROM b4.lk_plant_controllers WHERE idcompanycontroller=@Id;";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<LkPlantControllers>(sql, new { Id = id });
        }

        public async Task<IEnumerable<LkPlantControllers>> GetAllAsync()
        {
            const string sql =
                "SELECT * FROM b4.lk_plant_controllers;";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<LkPlantControllers>(sql);
        }

        public async Task UpdateAsync(LkPlantControllers entity)
        {
            const string sql = @"
                UPDATE b4.lk_plant_controllers SET
                    idcompany=@IdCompany,
                    controller=@Controller,
                    email=@Email
                WHERE idcompanycontroller=@IdCompanyController;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            const string sql =
                "DELETE FROM b4.lk_plant_controllers WHERE idcompanycontroller=@Id;";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
