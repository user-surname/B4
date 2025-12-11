using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.LkEntities
{
    public class LkPlantillasBotonesPasosTipos : LkBase
    {
        public int IdPasoTipo { get; set; }
        public string Pasotipo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        // Constructor vacío
        public LkPlantillasBotonesPasosTipos() { }

        // Constructor con parámetros
        public LkPlantillasBotonesPasosTipos(int idPasoTipo, string pasotipo,
            DateTime createdAt, DateTime updatedAt, long isActive, string? descripcion = null) : base(createdAt, updatedAt, isActive)
        {
            IdPasoTipo = idPasoTipo;
            Pasotipo = pasotipo;
            Descripcion = descripcion;
        }
    }
}
