using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.PostgreSQL
{
    public sealed class PostgreSqlLkPlantCurrencyQueries : ILkPlantCurrencyQueries
    {
        public string AddQuery() => @"
            INSERT INTO b4.lk_plant_currency VALUES (@IdCurrency, @Currency, @CurrencyAlias, @CreatedAt, @UpdatedAt, @IsActive)";

        public string GetByIdQuery() => "SELECT * FROM b4.lk_plant_currency WHERE idcurrency = @Id";

        public string GetAllQuery() => "SELECT * FROM b4.lk_plant_currency LIMIT 2000";

        public string UpdateQuery() => @"
            UPDATE b4.lk_plant_currency SET
                currency=@Currency, currencyalias=@CurrencyAlias,
                updatedat=@UpdatedAt, isactive=@IsActive
            WHERE idcurrency=@IdCurrency";

        public string DeleteQuery() => "DELETE FROM b4.lk_plant_currency WHERE idcurrency = @Id";
    }
}
