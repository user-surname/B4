using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlLkPlantillasBotonesPasosTiposQueries : ILkPlantillasBotonesPasosTiposQueries
    {
        public string AddQuery() => @"
            INSERT INTO LK_PLANTILLAS_BOTONES_PASOS_TIPOS (idPasoTipo, Pasotipo, descripcion)
            VALUES (@IdPasoTipo, @Pasotipo, @Descripcion)";

        public string GetByIdQuery() => "SELECT * FROM LK_PLANTILLAS_BOTONES_PASOS_TIPOS WHERE idPasoTipo = @Id";

        public string GetAllQuery() => "SELECT * FROM LK_PLANTILLAS_BOTONES_PASOS_TIPOS";

        public string UpdateQuery() => @"
            UPDATE LK_PLANTILLAS_BOTONES_PASOS_TIPOS SET
                Pasotipo = @Pasotipo,
                descripcion = @Descripcion
            WHERE idPasoTipo = @IdPasoTipo";

        public string DeleteQuery() => "DELETE FROM LK_PLANTILLAS_BOTONES_PASOS_TIPOS WHERE idPasoTipo = @Id";
    }
}
