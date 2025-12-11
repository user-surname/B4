using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.LkEntities
{
    public class LkPlantSubdivision : LkBase
    {
        public int IdSubdivision { get; set; }
        public string Subdivision { get; set; }

        public LkPlantSubdivision() { }

        public LkPlantSubdivision(int idSubdivision, string subdivision,
            DateTime createdAt, DateTime updatedAt, long isActive) : base(createdAt, updatedAt, isActive)
        {
            IdSubdivision = idSubdivision;
            Subdivision = subdivision;
        }
    }
}
