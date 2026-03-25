using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlDataComentariosQueries : IDataComentariosQueries
    {
        public string AddQuery() => @"
            INSERT INTO DATA_Comentarios
            (IdAPICarga, guidCarga, FechaUltModif, IdCompany, Ejercicio, IdCiclo, IdFase,
             IdEpigrafe, Etiqueta, Comentario, checksum, iszero)
            VALUES
            (@IdAPICarga, @GuidCarga, @FechaUltModif, @IdCompany, @Ejercicio, @IdCiclo, @IdFase,
             @IdEpigrafe, @Etiqueta, @Comentario, @Checksum, @IsZero)";

        public string GetByIdQuery() => "SELECT * FROM DATA_Comentarios WHERE Id = @Id";

        public string GetAllQuery() => "SELECT * FROM DATA_Comentarios";

        public string UpdateQuery() => @"
            UPDATE DATA_Comentarios SET
                IdAPICarga = @IdAPICarga, guidCarga = @GuidCarga, FechaUltModif = @FechaUltModif,
                IdCompany = @IdCompany, Ejercicio = @Ejercicio, IdCiclo = @IdCiclo, IdFase = @IdFase,
                IdEpigrafe = @IdEpigrafe, Etiqueta = @Etiqueta, Comentario = @Comentario,
                checksum = @Checksum, iszero = @IsZero
            WHERE Id = @Id";

        public string DeleteQuery() => "DELETE FROM DATA_Comentarios WHERE Id = @Id";
    }
}
