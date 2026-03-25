using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    /// <summary>
    /// Implementa la version MySQL de las queries de LkCiclos.
    /// </summary>
    public sealed class MySqlLkCiclosQueries : ILkCiclosQueries
    {
        public string AddQuery() => @"
            INSERT INTO LK_CICLOS (idCiclo, Ciclo, Descripcion)
            VALUES (@IdCiclo, @Ciclo, @Descripcion)";

        public string GetByIdQuery() => "SELECT * FROM LK_CICLOS WHERE id = @Id";

        public string GetAllQuery() => "SELECT * FROM LK_CICLOS";

        public string UpdateQuery() => @"
            UPDATE LK_CICLOS SET
                idCiclo = @IdCiclo,
                Ciclo = @Ciclo,
                Descripcion = @Descripcion
            WHERE id = @Id";

        public string DeleteQuery() => "DELETE FROM LK_CICLOS WHERE id = @Id";
    }
}
