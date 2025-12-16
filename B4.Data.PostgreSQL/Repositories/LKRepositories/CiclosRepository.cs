using Dapper;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class CiclosRepository 
        : BaseLkRepository<LkCiclos>, ICiclosRepository
    {
        public CiclosRepository(PostgreSQLDapperContext ctx)
            : base(ctx, "b4.lk_ciclos", "idciclo") { }

        public override async Task AddAsync(LkCiclos entity)
        {
            const string sql = @"
                INSERT INTO b4.lk_ciclos (idciclo, ciclo, descripcion)
                VALUES (@IdCiclo, @Ciclo, @Descripcion);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public override async Task UpdateAsync(LkCiclos entity)
        {
            const string sql = @"
                UPDATE b4.lk_ciclos SET 
                    ciclo=@Ciclo,
                    descripcion=@Descripcion
                WHERE idciclo=@IdCiclo;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }
    }
}
