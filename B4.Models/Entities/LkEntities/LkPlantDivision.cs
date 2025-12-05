using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.LkEntities
{
    public class LkPlantDivision
    {
        public int IdDivision { get; set; }
        public string Division { get; set; }

        public LkPlantDivision() { }

        public LkPlantDivision(int idDivision, string division)
        {
            IdDivision = idDivision;
            Division = division;
        }
    }
}
