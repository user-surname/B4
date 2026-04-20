using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlLogActividadQueries : ILogActividadQueries
    {
        public string AddQuery() => @"
            INSERT INTO log_actividad
            (timestamp, nivel, modulo, entrada, IdFicheroExcel, NombreFicheroExcel, iDCarga, DCR_ID_JOB)
            VALUES
            (@Timestamp, @Nivel, @Modulo, @Entrada, @IdFicheroExcel, @NombreFicheroExcel, @IDCarga, @DCR_ID_JOB)";

        public string GetByIdQuery() => "SELECT * FROM log_actividad WHERE id = @Id";

        public string GetAllQuery() => "SELECT * FROM log_actividad ORDER BY id DESC";

        public string UpdateQuery() => @"
            UPDATE log_actividad SET
                timestamp = @Timestamp,
                nivel = @Nivel,
                modulo = @Modulo,
                entrada = @Entrada,
                IdFicheroExcel = @IdFicheroExcel,
                NombreFicheroExcel = @NombreFicheroExcel,
                iDCarga = @IDCarga,
                DCR_ID_JOB = @DCR_ID_JOB
            WHERE id = @Id";

        public string DeleteQuery() => "DELETE FROM log_actividad WHERE id = @Id";
    }
}
