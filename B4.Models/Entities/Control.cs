using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities
{
    public class Control
    {
        public int IdControl { get; set; }
        public int Anyo { get; set; }
        public int IdCiclo { get; set; }
        public int IdFaseControl { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Final { get; set; }
        public bool? Activo { get; set; }
        public string? AddInfo { get; set; }
        public bool? SendAutEmail { get; set; }
        public bool? BWReportsMandatory { get; set; }


        public Control() { }


        public Control(int idControl, int anyo, int idCiclo, int idFaseControl, DateTime inicio, DateTime final)
        {
            IdControl = idControl;
            Anyo = anyo;
            IdCiclo = idCiclo;
            IdFaseControl = idFaseControl;
            Inicio = inicio;
            Final = final;
            Activo = true; // valor por defecto
            SendAutEmail = false;
            BWReportsMandatory = false;
        }
    }
}

