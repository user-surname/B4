using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlStgDataComentariosQueries : IStgDataComentariosQueries
    {
        public string AddQuery() => @"
            INSERT INTO STG_DATA_Comentarios
            (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase,
             IdEpigrafe, Etiqueta, Comentario, checksum, iszero)
            VALUES
            (@IdAPICarga, @GuidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase,
             @IdEpigrafe, @Etiqueta, @Comentario, @Checksum, @IsZero)";

        public string GetByIdQuery() => "SELECT * FROM STG_DATA_Comentarios WHERE Id = @Id";

        public string GetAllQuery() => "SELECT * FROM STG_DATA_Comentarios";

        public string UpdateQuery() => @"
            UPDATE STG_DATA_Comentarios SET
                IdAPICarga = @IdAPICarga, guidCarga = @GuidCarga, FechaUltModif = @FechaUltModif,
                IdCompany = @IdCompany, Ejercicio = @Ejercicio, IdCiclo = @IdCiclo, IdFase = @IdFase,
                IdEpigrafe = @IdEpigrafe, Etiqueta = @Etiqueta, Comentario = @Comentario,
                checksum = @Checksum, iszero = @IsZero
            WHERE Id = @Id";

        public string DeleteQuery() => "DELETE FROM STG_DATA_Comentarios WHERE Id = @Id";
    }
}
