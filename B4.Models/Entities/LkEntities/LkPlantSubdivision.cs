using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.LkEntities
{
    public class LkPlantSubdivision
    {
        public int IdSubdivision { get; set; }
        public string Subdivision { get; set; }

        public LkPlantSubdivision() { }

        public LkPlantSubdivision(int idSubdivision, string subdivision)
        {
            IdSubdivision = idSubdivision;
            Subdivision = subdivision;
        }
    }
}
