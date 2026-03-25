using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlLkFasesQueries : ILkFasesQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.lk_fases (idfase, fase, fasealias, createdat, updatedat, isactive)
            VALUES (@IdFase, @Fase, @FaseAlias, @CreatedAt, @UpdatedAt, @IsActive)";

        public string GetByIdQuery() => "SELECT * FROM b4.lk_fases WHERE idfase = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.lk_fases LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.lk_fases SET
                fase=@Fase, fasealias=@FaseAlias,
                updatedat=@UpdatedAt, isactive=@IsActive
            WHERE idfase=@IdFase";

        public string DeleteQuery() => "DELETE FROM b4.lk_fases WHERE idfase = @Id";
    }
}
