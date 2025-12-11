using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.LkEntities
{
    public class LkEpigrafe : LkBase
    {
        public int IdEpigrafe { get; set; }
        public int IdPlantilla { get; set; }
        public int IdHoja { get; set; }
        public string? PreEpigrafe { get; set; }
        public string Epigrafe { get; set; }
        public string EpigrafeFull { get; set; }

        public LkEpigrafe() { }

        public LkEpigrafe(int idEpigrafe, int idPlantilla, int idHoja, string? preEpigrafe, string epigrafe, string epigrafeFull,
            DateTime createdAt, DateTime updatedAt, long isActive) : base(createdAt, updatedAt, isActive)
        {
            IdEpigrafe = idEpigrafe;
            IdPlantilla = idPlantilla;
            IdHoja = idHoja;
            PreEpigrafe = preEpigrafe;
            Epigrafe = epigrafe;
            EpigrafeFull = epigrafeFull;
        }
    }
}
