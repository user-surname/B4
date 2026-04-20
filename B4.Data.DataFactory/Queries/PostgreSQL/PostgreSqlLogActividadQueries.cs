using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlLogActividadQueries : ILogActividadQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.log_actividad
            (timestamp, nivel, modulo, entrada, idficheroexcel, nombreficheroexcel, idcarga, dcr_id_job)
            VALUES
            (@Timestamp, @Nivel, @Modulo, @Entrada, @IdFicheroExcel, @NombreFicheroExcel, @IDCarga, @DCR_ID_JOB)
            RETURNING id";

        public string GetByIdQuery() => "SELECT * FROM b4.log_actividad WHERE id = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.log_actividad ORDER BY id DESC LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.log_actividad SET
                timestamp = @Timestamp,
                nivel = @Nivel,
                modulo = @Modulo,
                entrada = @Entrada,
                idficheroexcel = @IdFicheroExcel,
                nombreficheroexcel = @NombreFicheroExcel,
                idcarga = @IDCarga,
                dcr_id_job = @DCR_ID_JOB
            WHERE id = @Id";

        public string DeleteQuery() => "DELETE FROM b4.log_actividad WHERE id = @Id";
    }
}
