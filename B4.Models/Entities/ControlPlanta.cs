using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities
{
    public class ControlPlanta
    {
        public int IdControlPlanta { get; set; }
        public int IdControl { get; set; }
        public int IdCompany { get; set; }

        public ControlPlanta() { }

        public ControlPlanta(int idControlPlanta, int idControl, int idCompany)
        {
            IdControlPlanta = idControlPlanta;
            IdControl = idControl;
            IdCompany = idCompany;
        }
    }
}
