using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlLkFasesQueries : ILkFasesQueries
    {
        public string AddQuery() => @"
            INSERT INTO LK_FASES (idFase, Fase, FaseAlias)
            VALUES (@IdFase, @Fase, @FaseAlias)";

        public string GetByIdQuery() => "SELECT idFase AS IdFase, Fase, FaseAlias FROM LK_FASES WHERE idFase = @Id";

        public string GetAllQuery() => "SELECT idFase AS IdFase, Fase, FaseAlias FROM LK_FASES";

        public string UpdateQuery() => @"
            UPDATE LK_FASES SET
                Fase = @Fase,
                FaseAlias = @FaseAlias
            WHERE idFase = @IdFase";

        public string DeleteQuery() => "DELETE FROM LK_FASES WHERE idFase = @Id";
    }
}
