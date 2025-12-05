using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.LkEntities
{
    public class LkPlantCurrency
    {
        public int IdCurrency { get; set; }
        public string Currency { get; set; }
        public string CurrencyAlias { get; set; }

        public LkPlantCurrency() { }

        public LkPlantCurrency(int idCurrency, string currency, string currencyAlias)
        {
            IdCurrency = idCurrency;
            Currency = currency;
            CurrencyAlias = currencyAlias;
        }
    }
}
