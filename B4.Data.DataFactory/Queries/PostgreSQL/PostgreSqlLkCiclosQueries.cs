using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlLkCiclosQueries : ILkCiclosQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.lk_ciclos (idciclo, ciclo, descripcion, createdat, updatedat, isactive)
            VALUES (@IdCiclo, @Ciclo, @Descripcion, @CreatedAt, @UpdatedAt, @IsActive)
            RETURNING id";

        public string GetByIdQuery() => "SELECT * FROM b4.lk_ciclos WHERE id = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.lk_ciclos LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.lk_ciclos SET
                idciclo=@IdCiclo, ciclo=@Ciclo, descripcion=@Descripcion,
                updatedat=@UpdatedAt, isactive=@IsActive
            WHERE id=@Id";

        public string DeleteQuery() => "DELETE FROM b4.lk_ciclos WHERE id = @Id";
    }
}
