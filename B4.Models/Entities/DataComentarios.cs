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
        public Guid GuidCarga { get; set; }
        public DateTime? FechaUltModif { get; set; }
        public int IdCompany { get; set; }
        public int Ejercicio { get; set; }
        public int IdCiclo { get; set; }
        public int IdFase { get; set; }
        public int IdEpigrafe { get; set; }
        public string Etiqueta { get; set; }
        public string? Comentario { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Version { get; set; }
        public int? Checksum { get; set; }
        public int IsZero { get; set; }

        public DataComentarios() { }

        public DataComentarios(int id, int idAPICarga, Guid guidCarga, DateTime? fechaUltModif, int idCompany,
            int ejercicio, int idCiclo, int idFase, int idEpigrafe, string etiqueta, string? comentario,
            DateTime createdAt,
            DateTime updatedAt,
            int version,
            int? checksum,
            int isZero
            )
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
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Version = version;
            Checksum = checksum;
            IsZero = isZero;
        }
    }
}
