using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.RepositoryInterfaces;
using B4.Data.PostgreSQL;

namespace B4.Data.PostgreSQL.Repositories
{
    public class ControlRepository : IControlRepository
    {
        // Ajusta el schema si tu tabla está en public en vez de b4
        private const string _tableName = "b4.control";

        private readonly PostgreSQLDapperContext _context;

        public ControlRepository(PostgreSQLDapperContext context)
        {
            _context = context;
        }

        // -------------------------------------------------
        // C - CREATE
        // -------------------------------------------------
        public async Task AddAsync(Control entity)
        {
            const string sql = $@"
                INSERT INTO {_tableName}
                (idcontrol, anyo, idciclo, idfasecontrol, inicio, final, activo, addinfo, sendautemail, bwreportsmandatory)
                VALUES
                (@IdControl, @Anyo, @IdCiclo, @IdFaseControl, @Inicio, @Final, @Activo, @AddInfo, @SendAutEmail, @BWReportsMandatory)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un Control en la base de datos (PostgreSQL).", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<Control?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE idcontrol = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<Control>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el Control con id {id} (PostgreSQL).", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (todos)
        // -------------------------------------------------
        public async Task<IEnumerable<Control>> GetAllAsync()
        {
            const string sql = $@"SELECT * FROM {_tableName}";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QueryAsync<Control>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de controles (PostgreSQL).", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(Control entity)
        {
            const string sql = $@"
                UPDATE {_tableName} SET
                    anyo = @Anyo,
                    idciclo = @IdCiclo,
                    idfasecontrol = @IdFaseControl,
                    inicio = @Inicio,
                    final = @Final,
                    activo = @Activo,
                    addinfo = @AddInfo,
                    sendautemail = @SendAutEmail,
                    bwreportsmandatory = @BWReportsMandatory
                WHERE idcontrol = @IdControl";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, entity);

                if (rows == 0)
                    throw new Exception($"No se encontró el Control con id {entity.IdControl} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el Control con id {entity.IdControl} (PostgreSQL).", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = $@"DELETE FROM {_tableName} WHERE idcontrol = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, new { Id = id });

                if (rows == 0)
                    throw new Exception($"No se encontró el Control con id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el Control con id {id} (PostgreSQL).", ex);
            }
        }
    }
}
