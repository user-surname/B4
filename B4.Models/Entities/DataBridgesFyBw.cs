using System;

namespace B4.Models.Entities
{
    public class DataBridgesFyBw
    {
        public int Id { get; set; }
        public int IdAPICarga { get; set; }
        public Guid GuidCarga { get; set; }
        public DateTime FechaUltModif { get; set; }
        public int IdCompany { get; set; }
        public int Ejercicio { get; set; }
        public int IdCiclo { get; set; }
        public int IdFase { get; set; }
        public int IdCurrency { get; set; }
        public int IdEpigrafe { get; set; }
        public decimal FiscalYear { get; set; }
        public decimal Percentage { get; set; }
        public decimal Zero { get; set; }
        public decimal ZeroPercentage { get; set; }
        public decimal Absolute { get; set; }
        public decimal AbsolutePercentage { get; set; }
        public decimal VMixNew { get; set; }
        public decimal RawMaterial { get; set; }
        public decimal Scrap { get; set; }
        public decimal Economics { get; set; }
        public decimal CurrencyMix { get; set; }
        public decimal Performance { get; set; }
        public decimal ProtoTool { get; set; }
        public decimal Others { get; set; }
        public string? Comments { get; set; }
        public int? IdCarga { get; set; }
        public int? IdCargaSTGBW { get; set; }
        public int? IdHoja { get; set; }

        public DataBridgesFyBw() { }

        public DataBridgesFyBw(
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
            decimal fiscalYear, decimal percentage, decimal zero, decimal zeroPercentage,
            decimal absolute, decimal absolutePercentage,
            decimal vMixNew, decimal rawMaterial, decimal scrap, decimal economics,
            decimal currencyMix, decimal performance,
            decimal protoTool, decimal others,
            string? comments,
            int? idCarga,
            int? idCargaSTGBW,
            int? idHoja)
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
            FiscalYear = fiscalYear;
            Percentage = percentage;
            Zero = zero;
            ZeroPercentage = zeroPercentage;
            Absolute = absolute;
            AbsolutePercentage = absolutePercentage;
            VMixNew = vMixNew;
            RawMaterial = rawMaterial;
            Scrap = scrap;
            Economics = economics;
            CurrencyMix = currencyMix;
            Performance = performance;
            ProtoTool = protoTool;
            Others = others;
            Comments = comments;
            IdCarga = idCarga;
            IdCargaSTGBW = idCargaSTGBW;
            IdHoja = idHoja;
        }
    }
}
