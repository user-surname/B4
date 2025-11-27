using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantillasBotonesPasosTiposRepository : IPlantillasBotonesPasosTiposRepository
    {
        private readonly DapperContext _context;

        public PlantillasBotonesPasosTiposRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LkPlantillasBotonesPasosTipos entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plantillas_botones_pasos_tipos
                (idpasotipo, pasotipo, descripcion)
                VALUES (@IdPasoTipo, @Pasotipo, @Descripcion);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task<LkPlantillasBotonesPasosTipos?> GetByIdAsync(int id)
        {
            const string sql =
                "SELECT * FROM b4.lk_plantillas_botones_pasos_tipos WHERE idpasotipo=@Id;";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<LkPlantillasBotonesPasosTipos>(sql, new { Id = id });
        }

        public async Task<IEnumerable<LkPlantillasBotonesPasosTipos>> GetAllAsync()
        {
            const string sql =
                "SELECT * FROM b4.lk_plantillas_botones_pasos_tipos;";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<LkPlantillasBotonesPasosTipos>(sql);
        }

        public async Task UpdateAsync(LkPlantillasBotonesPasosTipos entity)
        {
            const string sql = @"
                UPDATE b4.lk_plantillas_botones_pasos_tipos SET 
                    pasotipo=@Pasotipo,
                    descripcion=@Descripcion
                WHERE idpasotipo=@IdPasoTipo;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            const string sql =
                "DELETE FROM b4.lk_plantillas_botones_pasos_tipos WHERE idpasotipo=@Id;";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
