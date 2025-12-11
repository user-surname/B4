using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.LkEntities
{
    public class LkFases : LkBase
    {
        public int IdFase { get; set; }
        public string Fase { get; set; }
        public string? FaseAlias { get; set; }

        public LkFases() { }

        public LkFases(int idFase, string fase, string? faseAlias,
            DateTime createdAt, DateTime updatedAt, long isActive) : base(createdAt, updatedAt, isActive)
        {
            IdFase = idFase;
            Fase = fase;
            FaseAlias = faseAlias;
        }
    }
}
