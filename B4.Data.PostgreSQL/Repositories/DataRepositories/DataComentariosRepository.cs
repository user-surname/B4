using Dapper;
using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;

namespace B4.Data.PostgreSQL.Repositories.DataRepositories
{
    public class DataComentariosRepository 
        : BaseRepository<DataComentarios>, IDataComentariosRepository
    {
        public DataComentariosRepository(PostgreSQLDapperContext context)
            : base(context, "b4.data_comentarios") { }

        // CREATE
        public async Task AddAsync(DataComentarios entity)
        {
            var sql = @"
                INSERT INTO b4.data_comentarios
                (idapicarga, guidcarga, fechaultmodif,
                 idcompany, ejercicio, idciclo, idfase,
                 idepigrafe, etiqueta, comentario)
                VALUES
                (@IdAPICarga, @GuidCarga, @FechaUltModif,
                 @IdCompany, @Ejercicio, @IdCiclo, @IdFase,
                 @IdEpigrafe, @Etiqueta, @Comentario)
                RETURNING id;";

            using var conn = GetConnection();
            entity.Id = await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        // UPDATE
        public async Task UpdateAsync(DataComentarios entity)
        {
            var sql = @"
                UPDATE b4.data_comentarios SET
                    idapicarga    = @IdAPICarga,
                    guidcarga     = @GuidCarga,
                    fechaultmodif = @FechaUltModif,
                    idcompany     = @IdCompany,
                    ejercicio     = @Ejercicio,
                    idciclo       = @IdCiclo,
                    idfase        = @IdFase,
                    idepigrafe    = @IdEpigrafe,
                    etiqueta      = @Etiqueta,
                    comentario    = @Comentario
                WHERE id = @Id";

            using var conn = GetConnection();
            int rows = await conn.ExecuteAsync(sql, entity);

            if (rows == 0)
                throw new Exception($"No existe comentario con ID {entity.Id}");
        }
    }
}
