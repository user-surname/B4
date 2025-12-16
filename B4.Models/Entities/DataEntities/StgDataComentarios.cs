using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.DataEntities
{
    public class StgDataComentarios : DataBase
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
        public string Comentario { get; set; }

        // Constructor sin parámetros
        public StgDataComentarios()
        {
        }

        // Constructor con todos los parámetros
        public StgDataComentarios(
            int id, int idAPICarga, Guid guidCarga, DateTime? fechaUltModif,
            int idCompany, int ejercicio, int idCiclo, int idFase, int idEpigrafe,
            string etiqueta, string comentario,
            DateTime createdAt,
            DateTime updatedAt,
            int version,
            int checksum,
            int isZero
            ) : base(createdAt, updatedAt, version, checksum, isZero)
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
