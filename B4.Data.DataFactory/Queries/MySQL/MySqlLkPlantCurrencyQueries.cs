using B4.Data.DataFactory.Queries.Common;

namespace B4.Data.DataFactory.Queries.MySQL
{
    public sealed class MySqlLkPlantCurrencyQueries : ILkPlantCurrencyQueries
    {
        public string AddQuery() => @"
            INSERT INTO LK_PLANT_CURRENCY (idCurrency, Currency, CurrencyAlias)
            VALUES (@IdCurrency, @Currency, @CurrencyAlias)";

        public string GetByIdQuery() => "SELECT * FROM LK_PLANT_CURRENCY WHERE idCurrency = @Id";

        public string GetAllQuery() => "SELECT * FROM LK_PLANT_CURRENCY";

        public string UpdateQuery() => @"
            UPDATE LK_PLANT_CURRENCY SET
                Currency = @Currency,
                CurrencyAlias = @CurrencyAlias
            WHERE idCurrency = @IdCurrency";

        public string DeleteQuery() => "DELETE FROM LK_PLANT_CURRENCY WHERE idCurrency = @Id";
    }
}
