using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantTreeRepository : IPlantTreeRepository
    {
        private readonly DapperContext _context;

        public PlantTreeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LkPlantTree entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plant_tree
                (idtree, iddivision, iddivisioncompany, idsubdivision, idcountry)
                VALUES
                (@IdTree, @IdDivision, @IdDivisionCompany,
                 @IdSubdivision, @IdCountry);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task<LkPlantTree?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM b4.lk_plant_tree WHERE idtree=@Id;";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<LkPlantTree>(sql, new { Id = id });
        }

        public async Task<IEnumerable<LkPlantTree>> GetAllAsync()
        {
            const string sql = "SELECT * FROM b4.lk_plant_tree;";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<LkPlantTree>(sql);
        }

        public async Task UpdateAsync(LkPlantTree entity)
        {
            const string sql = @"
                UPDATE b4.lk_plant_tree SET
                    iddivision=@IdDivision,
                    iddivisioncompany=@IdDivisionCompany,
                    idsubdivision=@IdSubdivision,
                    idcountry=@IdCountry
                WHERE idtree=@IdTree;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM b4.lk_plant_tree WHERE idtree=@Id;";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
