using System;

namespace B4.Models.Entities
{
    public class DataBridgesMonthBw
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
        public decimal Volume { get; set; }
        public decimal InventoryChange { get; set; }
        public decimal Mix { get; set; }
        public decimal New { get; set; }
        public decimal Economics { get; set; }
        public decimal QuickSavings { get; set; }
        public decimal CurrencyMix { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal RawMaterial { get; set; }
        public decimal Scrap { get; set; }
        public decimal IndustrialPerformance { get; set; }
        public decimal ProtoTooling { get; set; }
        public decimal Other { get; set; }
        public decimal Check { get; set; }
        public string? Comments { get; set; }
        public int? IdCarga { get; set; }
        public int? IdCargaSTGBW { get; set; }
        public int? IdHoja { get; set; }

        public DataBridgesMonthBw() { }

        public DataBridgesMonthBw(
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
            decimal volume,
            decimal inventoryChange,
            decimal mix,
            decimal newValue,
            decimal economics,
            decimal quickSavings,
            decimal currencyMix,
            decimal exchangeRate,
            decimal rawMaterial,
            decimal scrap,
            decimal industrialPerformance,
            decimal protoTooling,
            decimal other,
            decimal check,
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

            Volume = volume;
            InventoryChange = inventoryChange;
            Mix = mix;
            New = newValue;
            Economics = economics;
            QuickSavings = quickSavings; // ← CORREGIDO AQUÍ
            CurrencyMix = currencyMix;
            ExchangeRate = exchangeRate;
            RawMaterial = rawMaterial;
            Scrap = scrap;
            IndustrialPerformance = industrialPerformance;
            ProtoTooling = protoTooling;
            Other = other;
            Check = check;

            Comments = comments;
            IdCarga = idCarga;
            IdCargaSTGBW = idCargaSTGBW;
            IdHoja = idHoja;
        }
    }
}
