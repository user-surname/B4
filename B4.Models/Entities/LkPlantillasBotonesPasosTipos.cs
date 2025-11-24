using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities
{
    public class LkPlantillasBotonesPasosTipos
    {
        public int IdPasoTipo { get; set; }
        public string Pasotipo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        // Constructor vacío
        public LkPlantillasBotonesPasosTipos() { }

        // Constructor con parámetros
        public LkPlantillasBotonesPasosTipos(int idPasoTipo, string pasotipo, string? descripcion = null)
        {
            IdPasoTipo = idPasoTipo;
            Pasotipo = pasotipo;
            Descripcion = descripcion;
        }
    }
}
