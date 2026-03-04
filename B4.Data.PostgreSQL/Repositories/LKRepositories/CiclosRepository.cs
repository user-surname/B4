using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.RepositoryInterfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class CiclosRepository
        : BaseLkRepository<LkCiclos>, ICiclosRepository
    {
        // ✅ CAMBIO: la key del repositorio pasa a ser "id" (PK real)
        public CiclosRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_ciclos", "id") { }

        public override async Task AddAsync(LkCiclos entity)
        {
            PrepareForInsert(entity);

            const string sql = @"
                INSERT INTO b4.lk_ciclos
                    (idciclo, ciclo, descripcion, createdat, updatedat, isactive)
                VALUES
                    (@IdCiclo, @Ciclo, @Descripcion, @CreatedAt, @UpdatedAt, @IsActive)
                RETURNING id;";

            using var conn = _context.CreateConnection();

            // ✅ CAMBIO: recuperamos el id generado y lo ponemos en entity.Id
            entity.Id = await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        public override async Task UpdateAsync(LkCiclos entity)
        {
            PrepareForUpdate(entity);

            const string sql = @"
                UPDATE b4.lk_ciclos SET 
                    idciclo=@IdCiclo,
                    ciclo=@Ciclo,
                    descripcion=@Descripcion,
                    updatedat=@UpdatedAt,
                    isactive=@IsActive
                WHERE id=@Id;";

            using var conn = _context.CreateConnection();

            // ✅ CAMBIO: ahora actualizamos por PK real (id)
            await conn.ExecuteAsync(sql, entity);
        }
    }
}