using System;

namespace B4.Models.Entities.DataEtities
{
    public class DataBwBase 
    {
        public int? IdCarga { get; set; }
        public int? IdCargaSTGBW { get; set; }
        public int? IdHoja { get; set; }
        public int Id { get; set; }
        public int IdAPICarga { get; set; }
        public Guid GuidCarga { get; set; } = Guid.Empty;
        public DateTime FechaUltModif { get; set; }
        public int IdCompany { get; set; }
        public int Ejercicio { get; set; }
        public int IdCiclo { get; set; }
        public int IdFase { get; set; }
        public int IdCurrency { get; set; }
        public int IdEpigrafe { get; set; }
        public decimal Mes00 { get; set; }
        public decimal Mes01 { get; set; }
        public decimal Mes02 { get; set; }
        public decimal Mes03 { get; set; }
        public decimal Mes04 { get; set; }
        public decimal Mes05 { get; set; }
        public decimal Mes06 { get; set; }
        public decimal Mes07 { get; set; }
        public decimal Mes08 { get; set; }
        public decimal Mes09 { get; set; }
        public decimal Mes10 { get; set; }
        public decimal Mes11 { get; set; }
        public decimal Mes12 { get; set; }
        public decimal Mes13 { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Version { get; set; }
        public int? Checksum { get; set; }
        public int IsZero { get; set; }

        public DataBwBase() { }

        public DataBwBase(
            int id,
            int idAPICarga,
            Guid guidCarga,
            DateTime fechaUltModif,
            int idCompany,
            int ejercicio,
            int idCiclo,
            int idFase,
            int idCurrency,
            int idEpigrafe,
            decimal mes00, decimal mes01, decimal mes02, decimal mes03,
            decimal mes04, decimal mes05, decimal mes06, decimal mes07,
            decimal mes08, decimal mes09, decimal mes10, decimal mes11,
            decimal mes12, decimal mes13,
            int? idCarga,
            int? idCargaSTGBW,
            int? idHoja,
            DateTime createdAt,
            DateTime updatedAt,
            int version,
            int? checksum,
            int isZero)
        {
            Id = id;
            IdAPICarga = idAPICarga;
            GuidCarga = guidCarga;
            FechaUltModif = fechaUltModif;
            IdCompany = idCompany;
            Ejercicio = ejercicio;
            IdCiclo = idCiclo;
            IdFase = idFase;
            IdCurrency = idCurrency;
            IdEpigrafe = idEpigrafe;
            Mes00 = mes00;
            Mes01 = mes01;
            Mes02 = mes02;
            Mes03 = mes03;
            Mes04 = mes04;
            Mes05 = mes05;
            Mes06 = mes06;
            Mes07 = mes07;
            Mes08 = mes08;
            Mes09 = mes09;
            Mes10 = mes10;
            Mes11 = mes11;
            Mes12 = mes12;
            Mes13 = mes13;
            IdCarga = idCarga;
            IdCargaSTGBW = idCargaSTGBW;
            IdHoja = idHoja;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Version = version;
            Checksum = checksum;
            IsZero = isZero;
        }
    }

}
