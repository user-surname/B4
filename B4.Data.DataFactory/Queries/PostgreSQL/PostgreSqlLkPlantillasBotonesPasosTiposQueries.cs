using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlLkPlantillasBotonesPasosTiposQueries : ILkPlantillasBotonesPasosTiposQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.lk_plantillas_botones_pasos_tipos
                (idpasotipo, pasotipo, descripcion, createdat, updatedat, isactive)
            VALUES
                (@IdPasoTipo, @Pasotipo, @Descripcion, @CreatedAt, @UpdatedAt, @IsActive)";

        public string GetByIdQuery() => "SELECT * FROM b4.lk_plantillas_botones_pasos_tipos WHERE idpasotipo = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.lk_plantillas_botones_pasos_tipos LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.lk_plantillas_botones_pasos_tipos SET
                pasotipo=@Pasotipo, descripcion=@Descripcion,
                updatedat=@UpdatedAt, isactive=@IsActive
            WHERE idpasotipo=@IdPasoTipo";

        public string DeleteQuery() => "DELETE FROM b4.lk_plantillas_botones_pasos_tipos WHERE idpasotipo = @Id";
    }
}
