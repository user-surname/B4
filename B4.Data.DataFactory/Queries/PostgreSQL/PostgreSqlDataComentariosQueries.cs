using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlDataComentariosQueries : IDataComentariosQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.data_comentarios
            (idapicarga, guidcarga, fechaultmodif, idcompany, ejercicio, idciclo, idfase, idepigrafe, etiqueta, comentario)
            VALUES
            (@IdAPICarga, @GuidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase, @IdEpigrafe, @Etiqueta, @Comentario)
            RETURNING id";

        public string GetByIdQuery() => "SELECT * FROM b4.data_comentarios WHERE id = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.data_comentarios LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.data_comentarios SET
                idapicarga=@IdAPICarga, guidcarga=@GuidCarga, fechaultmodif=@FechaUltModif,
                idcompany=@IdCompany, ejercicio=@Ejercicio, idciclo=@IdCiclo, idfase=@IdFase,
                idepigrafe=@IdEpigrafe, etiqueta=@Etiqueta, comentario=@Comentario
            WHERE id=@Id";

        public string DeleteQuery() => "DELETE FROM b4.data_comentarios WHERE id = @Id";
    }
}
