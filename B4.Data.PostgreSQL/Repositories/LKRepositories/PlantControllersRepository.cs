using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantControllersRepository
        : BaseLkRepository<LkPlantControllers>, IPlantControllersRepository
    {
        public PlantControllersRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_plant_controllers", "idcontroller") { }

        public override async Task AddAsync(LkPlantControllers entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_plant_controllers (idcontroller, controller)
                VALUES (@IdController, @Controller);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkPlantControllers entity)
        {
            const string sql = @"
                UPDATE b4.lk_plant_controllers 
                SET controller=@Controller
                WHERE idcontroller=@IdController;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
