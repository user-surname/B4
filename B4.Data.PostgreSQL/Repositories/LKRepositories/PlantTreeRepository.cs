using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class PlantTreeRepository
        : BaseLkRepository<LkPlantTree>, IPlantTreeRepository
    {
        public PlantTreeRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_plant_tree", "idtree") { }

        public override async Task AddAsync(LkPlantTree entity)
        {
            PrepareForInsert(entity);

            const string sql = @"
                INSERT INTO b4.lk_plant_tree
                    (idtree, iddivision, iddivisioncompany, idsubdivision, idcountry,
                     createdat, updatedat, isactive)
                VALUES
                    (@IdTree, @IdDivision, @IdDivisionCompany, @IdSubdivision, @IdCountry,
                     @CreatedAt, @UpdatedAt, @IsActive);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkPlantTree entity)
        {
            PrepareForUpdate(entity);

            const string sql = @"
                UPDATE b4.lk_plant_tree SET
                    iddivision=@IdDivision,
                    iddivisioncompany=@IdDivisionCompany,
                    idsubdivision=@IdSubdivision,
                    idcountry=@IdCountry,
                    updatedat=@UpdatedAt,
                    isactive=@IsActive
                WHERE idtree=@IdTree;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
