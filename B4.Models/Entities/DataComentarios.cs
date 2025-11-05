using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities
{
    public class DataComentarios
    {
        public int Id { get; set; }
        public int IdAPICarga { get; set; }
        public string GuidCarga { get; set; }
        public DateTime? FechaUltModif { get; set; }
        public int IdCompany { get; set; }
        public int Ejercicio { get; set; }
        public int IdCiclo { get; set; }
        public int IdFase { get; set; }
        public int IdEpigrafe { get; set; }
        public string Etiqueta { get; set; }
        public string? Comentario { get; set; }

        public DataComentarios() { }

        public DataComentarios(int id, int idAPICarga, string guidCarga, DateTime? fechaUltModif, int idCompany,
            int ejercicio, int idCiclo, int idFase, int idEpigrafe, string etiqueta, string? comentario)
        {
            Id = id;
            IdAPICarga = idAPICarga;
            GuidCarga = guidCarga;
            FechaUltModif = fechaUltModif;
            IdCompany = idCompany;
            Ejercicio = ejercicio;
            IdCiclo = idCiclo;
            IdFase = idFase;
            IdEpigrafe = idEpigrafe;
            Etiqueta = etiqueta;
            Comentario = comentario;
        }
    }
}
