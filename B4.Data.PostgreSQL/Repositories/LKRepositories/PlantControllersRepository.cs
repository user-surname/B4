using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantControllersRepository
        : BaseLkRepository<LkPlantControllers>, IPlantControllersRepository
    {
        // ✅ CAMBIO: key real en Postgres = idcompanycontroller (no idcontroller)
        public PlantControllersRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_plant_controllers", "idcompanycontroller") { }

        public override async Task AddAsync(LkPlantControllers entity)
        {
            PrepareForInsert(entity);

            // ✅ CAMBIO: columnas reales + NOT NULL (idcompany, email)
            const string sql = @"
                INSERT INTO b4.lk_plant_controllers
                    (idcompanycontroller, idcompany, controller, email, createdat, updatedat, isactive)
                VALUES
                    (@IdCompanyController, @IdCompany, @Controller, @Email, @CreatedAt, @UpdatedAt, @IsActive);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkPlantControllers entity)
        {
            PrepareForUpdate(entity);

            const string sql = @"
                UPDATE b4.lk_plant_controllers 
                SET
                    idcompany=@IdCompany,
                    controller=@Controller,
                    email=@Email,
                    updatedat=@UpdatedAt,
                    isactive=@IsActive
                WHERE idcompanycontroller=@IdCompanyController;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}