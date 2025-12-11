using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.LkEntities
{
    public class LkPlantCountry : LkBase
    {
        public int IdCountry { get; set; }
        public string Country { get; set; }

        public LkPlantCountry() { }

        public LkPlantCountry(int idCountry, string country,
            DateTime createdAt, DateTime updatedAt, long isActive) : base(createdAt, updatedAt, isActive)
        {
            IdCountry = idCountry;
            Country = country; 
        }
    }
}
