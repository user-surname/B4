using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlLkEpigrafeQueries : ILkEpigrafeQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.lk_epigrafe (idepigrafe, epigrafe, createdat, updatedat, isactive)
            VALUES (@IdEpigrafe, @Epigrafe, @CreatedAt, @UpdatedAt, @IsActive)";

        public string GetByIdQuery() => "SELECT * FROM b4.lk_epigrafe WHERE idepigrafe = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.lk_epigrafe LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.lk_epigrafe SET
                epigrafe=@Epigrafe, updatedat=@UpdatedAt, isactive=@IsActive
            WHERE idepigrafe=@IdEpigrafe";

        public string DeleteQuery() => "DELETE FROM b4.lk_epigrafe WHERE idepigrafe = @Id";
    }
}
