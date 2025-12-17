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
            PrepareForInsert(entity);

            const string sql = @"
                INSERT INTO b4.lk_plant_controllers
                    (idcontroller, controller, createdat, updatedat, isactive)
                VALUES
                    (@IdController, @Controller, @CreatedAt, @UpdatedAt, @IsActive);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkPlantControllers entity)
        {
            PrepareForUpdate(entity);

            const string sql = @"
                UPDATE b4.lk_plant_controllers 
                SET controller=@Controller,
                    updatedat=@UpdatedAt,
                    isactive=@IsActive
                WHERE idcontroller=@IdController;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
