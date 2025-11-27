using Dapper;
using B4.Models.Entities;
using B4.Models.Interfaces;

namespace B4.Data.PostgreSQL.Repositories
{
    public class ControlRepository : IControlRepository
    {
        private readonly DapperContext _context;

        public ControlRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Control entity)
        {
            const string sql = @"
                INSERT INTO b4.control
                (anyo, idciclo, idfasecontrol, inicio, final, activo,
                 addinfo, sendautemail, bwreportsmandatory)
                VALUES
                (@Anyo, @IdCiclo, @IdFaseControl, @Inicio, @Final, @Activo,
                 @AddInfo, @SendAutEmail, @BWReportsMandatory);";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task<Control?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM b4.control WHERE idcontrol=@Id;";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<Control>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Control>> GetAllAsync()
        {
            const string sql = "SELECT * FROM b4.control;";
            using var conn = _context.CreateConnection();
            return await conn.QueryAsync<Control>(sql);
        }

        public async Task UpdateAsync(Control entity)
        {
            const string sql = @"
                UPDATE b4.control SET
                    anyo=@Anyo,
                    idciclo=@IdCiclo,
                    idfasecontrol=@IdFaseControl,
                    inicio=@Inicio,
                    final=@Final,
                    activo=@Activo,
                    addinfo=@AddInfo,
                    sendautemail=@SendAutEmail,
                    bwreportsmandatory=@BWReportsMandatory
                WHERE idcontrol=@IdControl;";

            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(int id)
        {
            const string sql = "DELETE FROM b4.control WHERE idcontrol=@Id;";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(sql, new { Id = id });
        }
    }
}
