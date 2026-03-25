using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlLkEpigrafeQueries : ILkEpigrafeQueries
    {
        public string AddQuery() => @"
            INSERT INTO LK_EPIGRAFES (idEpigrafe, idPlantilla, idHoja, PreEpigrafe, Epigrafe, EpigrafeFull)
            VALUES (@IdEpigrafe, @IdPlantilla, @IdHoja, @PreEpigrafe, @Epigrafe, @EpigrafeFull)";

        public string GetByIdQuery() => "SELECT * FROM LK_EPIGRAFES WHERE idEpigrafe = @Id";

        public string GetAllQuery() => "SELECT * FROM LK_EPIGRAFES";

        public string UpdateQuery() => @"
            UPDATE LK_EPIGRAFES SET
                idPlantilla = @IdPlantilla,
                idHoja = @IdHoja,
                PreEpigrafe = @PreEpigrafe,
                Epigrafe = @Epigrafe,
                EpigrafeFull = @EpigrafeFull
            WHERE idEpigrafe = @IdEpigrafe";

        public string DeleteQuery() => "DELETE FROM LK_EPIGRAFES WHERE idEpigrafe = @Id";
    }
}
