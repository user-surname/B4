using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.DataEntities
{
    public class DataBridgesMonth : DataBase
    {
        public int Id { get; set; }
        public int IdAPICarga { get; set; }
        public Guid? GuidCarga { get; set; }
        public DateTime FechaUltModif { get; set; }
        public int IdCompany { get; set; }
        public int Ejercicio { get; set; }
        public int IdCiclo { get; set; }
        public int IdFase { get; set; }
        public int IdCurrency { get; set; }
        public int IdEpigrafe { get; set; }
        public decimal Actuals { get; set; }
        public decimal PctActuals { get; set; }
        public decimal Budget { get; set; }
        public decimal PctBudget { get; set; }
        public decimal Variance { get; set; }
        public decimal Volume { get; set; }
        public decimal InventoryChange { get; set; }
        public decimal Mix { get; set; }
        public decimal New { get; set; }
        public decimal EconomicsInflation { get; set; }
        public decimal EconomicsInflationClaim { get; set; }
        public decimal EconomicsPricingIssues { get; set; }
        public decimal EconomicsLTAS { get; set; }
        public decimal EconomicsBPs { get; set; }
        public decimal EconomicsComponentEffect { get; set; }
        public decimal StockValuation { get; set; }
        public decimal QuickSavings { get; set; }
        public decimal CurrencyMix { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal RawMaterial { get; set; }
        public decimal Scrap { get; set; }
        public decimal PerformanceGSD { get; set; }
        public decimal PerformanceQuality { get; set; }
        public decimal PerformanceOther { get; set; }
        public decimal PerformanceLaunchingCost { get; set; }
        public decimal ProtoTooling { get; set; }
        public decimal AccruralOthers { get; set; }
        public decimal OneTimeEffectOthers { get; set; }
        public decimal Other { get; set; }
        public string? Comments { get; set; }

        // Constructor vacío
        public DataBridgesMonth() { }

        // Constructor con parámetros
        public DataBridgesMonth(
            int id,
            int idAPICarga,
            Guid? guidCarga,
            DateTime fechaUltModif,
            int idCompany,
            int ejercicio,
            int idCiclo,
            int idFase,
            int idCurrency,
            int idEpigrafe,
            decimal actuals,
            decimal pctActuals,
            decimal budget,
            decimal pctBudget,
            decimal variance,
            decimal volume,
            decimal inventoryChange,
            decimal mix,
            decimal @new,
            decimal economicsInflation,
            decimal economicsInflationClaim,
            decimal economicsPricingIssues,
            decimal economicsLTAS,
            decimal economicsBPs,
            decimal economicsComponentEffect,
            decimal stockValuation,
            decimal quickSavings,
            decimal currencyMix,
            decimal exchangeRate,
            decimal rawMaterial,
            decimal scrap,
            decimal performanceGSD,
            decimal performanceQuality,
            decimal performanceOther,
            decimal performanceLaunchingCost,
            decimal protoTooling,
            decimal accruralOthers,
            decimal oneTimeEffectOthers,
            decimal other,
            string? comments,
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
            IdCurrency = idCurrency;
            IdEpigrafe = idEpigrafe;
            Actuals = actuals;
            PctActuals = pctActuals;
            Budget = budget;
            PctBudget = pctBudget;
            Variance = variance;
            Volume = volume;
            InventoryChange = inventoryChange;
            Mix = mix;
            New = @new;
            EconomicsInflation = economicsInflation;
            EconomicsInflationClaim = economicsInflationClaim;
            EconomicsPricingIssues = economicsPricingIssues;
            EconomicsLTAS = economicsLTAS;
            EconomicsBPs = economicsBPs;
            EconomicsComponentEffect = economicsComponentEffect;
            StockValuation = stockValuation;
            QuickSavings = quickSavings;
            CurrencyMix = currencyMix;
            ExchangeRate = exchangeRate;
            RawMaterial = rawMaterial;
            Scrap = scrap;
            PerformanceGSD = performanceGSD;
            PerformanceQuality = performanceQuality;
            PerformanceOther = performanceOther;
            PerformanceLaunchingCost = performanceLaunchingCost;
            ProtoTooling = protoTooling;
            AccruralOthers = accruralOthers;
            OneTimeEffectOthers = oneTimeEffectOthers;
            Other = other;
            Comments = comments;
        }
    }
}
