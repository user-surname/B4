using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.LkEntities
{
    public class LkPlantTree : LkBase
    {
        public int IdTree { get; set; }
        public int IdDivision { get; set; }
        public int IdDivisionCompany { get; set; }
        public int IdSubdivision { get; set; }
        public int IdCountry { get; set; }

        public LkPlantTree() { }

        public LkPlantTree(int idTree, int idDivision, int idDivisionCompany, int idSubdivision, int idCountry,
            DateTime createdAt, DateTime updatedAt, long isActive) : base(createdAt, updatedAt, isActive)
        {
            IdTree = idTree;
            IdDivision = idDivision;
            IdDivisionCompany = idDivisionCompany;
            IdSubdivision = idSubdivision;
            IdCountry = idCountry;
        }
    }
}
