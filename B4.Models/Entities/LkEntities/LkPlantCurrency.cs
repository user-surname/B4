using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.LkEntities
{
    public class LkPlantCurrency : LkBase
    {
        public int IdCurrency { get; set; }
        public string Currency { get; set; }
        public string CurrencyAlias { get; set; }

        public LkPlantCurrency() { }

        public LkPlantCurrency(int idCurrency, string currency, string currencyAlias,
            DateTime createdAt, DateTime updatedAt, long isActive) : base(createdAt, updatedAt, isActive)
        {
            IdCurrency = idCurrency;
            Currency = currency;
            CurrencyAlias = currencyAlias;
        }
    }
}
