using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlControlQueries : IControlQueries
    {
        public string AddQuery() => @"
            INSERT INTO CONTROL
            (idControl, Anyo, idCiclo, idFaseControl, Inicio, Final, Activo, AddInfo, SendAutEmail, BWReportsMandatory)
            VALUES
            (@IdControl, @Anyo, @IdCiclo, @IdFaseControl, @Inicio, @Final, @Activo, @AddInfo, @SendAutEmail, @BWReportsMandatory)";

        public string GetByIdQuery() => "SELECT * FROM CONTROL WHERE idControl = @Id";

        public string GetAllQuery() => "SELECT * FROM CONTROL";

        public string UpdateQuery() => @"
            UPDATE CONTROL SET
                Anyo = @Anyo,
                idCiclo = @IdCiclo,
                idFaseControl = @IdFaseControl,
                Inicio = @Inicio,
                Final = @Final,
                Activo = @Activo,
                AddInfo = @AddInfo,
                SendAutEmail = @SendAutEmail,
                BWReportsMandatory = @BWReportsMandatory
            WHERE idControl = @IdControl";

        public string DeleteQuery() => "DELETE FROM CONTROL WHERE idControl = @Id";
    }
}
