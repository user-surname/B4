using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities
{
    public class LkCiclos
    {
        public int Id { get; set; }
        public int IdCiclo { get; set; }
        public string Ciclo { get; set; }
        public string Descripcion { get; set; }

        public LkCiclos() { }

        public LkCiclos(int id, int idCiclo, string ciclo, string descripcion)
        {
            Id = id;
            IdCiclo = idCiclo;
            Ciclo = ciclo;
            Descripcion = descripcion;
        }
    }
}
