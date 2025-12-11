using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.LkEntities
{
    public class LkPlantDivisionCompany : LkBase
    {
        public int IdDivisionCompany { get; set; }
        public string? DivisionCompany { get; set; }

        public LkPlantDivisionCompany() { }

        public LkPlantDivisionCompany(int idDivisionCompany, string? divisionCompany,
            DateTime createdAt, DateTime updatedAt, long isActive) : base(createdAt, updatedAt, isActive)
        {
            IdDivisionCompany = idDivisionCompany;
            DivisionCompany = divisionCompany;
        }
    }
}
