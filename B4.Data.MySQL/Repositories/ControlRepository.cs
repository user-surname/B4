using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;
using B4.Data.MySQL;
using System;

namespace B4.Data.MySQL.Repositories
{
    public class ControlRepository : IControlRepository
    {
        // Nombre de la tabla asociado al repositorio
        private const string _tableName = "CONTROL";

        // Contexto para gestionar la conexión
        private readonly DapperContext _context;

        public ControlRepository(DapperContext context)
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
                (idControl, Anyo, idCiclo, idFaseControl, Inicio, Final, Activo, AddInfo, SendAutEmail, BWReportsMandatory)
                VALUES (@IdControl, @Anyo, @IdCiclo, @IdFaseControl, @Inicio, @Final, @Activo, @AddInfo, @SendAutEmail, @BWReportsMandatory)";

            try
            {
                using var conn = _context.CreateConnection();
                await conn.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar un Control en la base de datos.", ex);
            }
        }

        // -------------------------------------------------
        // R - READ (por ID)
        // -------------------------------------------------
        public async Task<Control?> GetByIdAsync(int id)
        {
            const string sql = $@"SELECT * FROM {_tableName} WHERE idControl = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                return await conn.QuerySingleOrDefaultAsync<Control>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el Control con id {id}.", ex);
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
                throw new Exception("Error al obtener la lista de controles.", ex);
            }
        }

        // -------------------------------------------------
        // U - UPDATE
        // -------------------------------------------------
        public async Task UpdateAsync(Control entity)
        {
            const string sql = $@"
                UPDATE {_tableName} SET
                    Anyo = @Anyo,
                    idCiclo = @IdCiclo,
                    idFaseControl = @IdFaseControl,
                    Inicio = @Inicio,
                    Final = @Final,
                    Activo = @Activo,
                    AddInfo = @AddInfo,
                    SendAutEmail = @SendAutEmail,
                    BWReportsMandatory = @BWReportsMandatory
                WHERE idControl = @IdControl";

            try
            {
                using var conn = _context.CreateConnection();

                int rows = await conn.ExecuteAsync(sql, entity);

                if (rows == 0)
                    throw new Exception($"No se encontró el Control con id {entity.IdControl} para actualizar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el Control con id {entity.IdControl}.", ex);
            }
        }

        // -------------------------------------------------
        // D - DELETE
        // -------------------------------------------------
        public async Task DeleteAsync(int id)
        {
            const string sql = $@"DELETE FROM {_tableName} WHERE idControl = @Id";

            try
            {
                using var conn = _context.CreateConnection();
                int rows = await conn.ExecuteAsync(sql, new { Id = id });

                if (rows == 0)
                    throw new Exception($"No se encontró el Control con id {id} para eliminar.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el Control con id {id}.", ex);
            }
        }
    }
}

