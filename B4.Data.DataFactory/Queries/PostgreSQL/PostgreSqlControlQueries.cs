using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlControlQueries : IControlQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.control
            (idcontrol, anyo, idciclo, idfasecontrol, inicio, final, activo, addinfo, sendautemail, bwreportsmandatory)
            VALUES
            (@IdControl, @Anyo, @IdCiclo, @IdFaseControl, @Inicio, @Final, @Activo, @AddInfo, @SendAutEmail, @BWReportsMandatory)";

        public string GetByIdQuery() => "SELECT * FROM b4.control WHERE idcontrol = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.control";

        public string UpdateQuery() => @"
            UPDATE b4.control SET
                anyo=@Anyo, idciclo=@IdCiclo, idfasecontrol=@IdFaseControl,
                inicio=@Inicio, final=@Final, activo=@Activo, addinfo=@AddInfo,
                sendautemail=@SendAutEmail, bwreportsmandatory=@BWReportsMandatory
            WHERE idcontrol=@IdControl";

        public string DeleteQuery() => "DELETE FROM b4.control WHERE idcontrol = @Id";
    }
}
