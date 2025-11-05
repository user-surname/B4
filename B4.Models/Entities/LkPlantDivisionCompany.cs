using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities
{
    public class LkPlantDivisionCompany
    {
        public int IdDivisionCompany { get; set; }
        public string? DivisionCompany { get; set; }

        public LkPlantDivisionCompany() { }

        public LkPlantDivisionCompany(int idDivisionCompany, string? divisionCompany)
        {
            IdDivisionCompany = idDivisionCompany;
            DivisionCompany = divisionCompany;
        }
    }
}
