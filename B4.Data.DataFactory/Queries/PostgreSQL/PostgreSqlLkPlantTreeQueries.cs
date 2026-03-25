using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlLkPlantTreeQueries : ILkPlantTreeQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.lk_plant_tree
                (idtree, iddivision, iddivisioncompany, idsubdivision, idcountry, createdat, updatedat, isactive)
            VALUES
                (@IdTree, @IdDivision, @IdDivisionCompany, @IdSubdivision, @IdCountry, @CreatedAt, @UpdatedAt, @IsActive)";

        public string GetByIdQuery() => "SELECT * FROM b4.lk_plant_tree WHERE idtree = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.lk_plant_tree LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.lk_plant_tree SET
                iddivision=@IdDivision, iddivisioncompany=@IdDivisionCompany,
                idsubdivision=@IdSubdivision, idcountry=@IdCountry,
                updatedat=@UpdatedAt, isactive=@IsActive
            WHERE idtree=@IdTree";

        public string DeleteQuery() => "DELETE FROM b4.lk_plant_tree WHERE idtree = @Id";
    }
}
