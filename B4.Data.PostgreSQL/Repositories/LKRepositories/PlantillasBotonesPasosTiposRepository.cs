using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantillasBotonesPasosTiposRepository 
        : BaseLkRepository<LkPlantillasBotonesPasosTipos>, IPlantillasBotonesPasosTiposRepository
    {
        public PlantillasBotonesPasosTiposRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_plantillas_botones_pasos_tipos", "idpasotipo") 
        { 
        }

        public override async Task AddAsync(LkPlantillasBotonesPasosTipos entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plantillas_botones_pasos_tipos
                (idpasotipo, pasotipo, descripcion)
                VALUES (@IdPasoTipo, @Pasotipo, @Descripcion);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkPlantillasBotonesPasosTipos entity)
        {
            const string sql = @"
                UPDATE b4.lk_plantillas_botones_pasos_tipos SET 
                    pasotipo=@Pasotipo,
                    descripcion=@Descripcion
                WHERE idpasotipo=@IdPasoTipo;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
