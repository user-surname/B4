using System;

namespace B4.Models.Entities
{
    public class DataActualsBw : DataForecastBw
    {
        public int? IdCarga { get; set; }
        public int? IdCargaSTGBW { get; set; }
        public int? IdHoja { get; set; }

        public DataActualsBw() { }

        public DataActualsBw(
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
            int? idHoja)
            : base(id, idAPICarga, guidCarga, fechaUltModif, idCompany, ejercicio,
                  idCiclo, idFase, idCurrency, idEpigrafe,
                  mes00, mes01, mes02, mes03, mes04, mes05, mes06, mes07,
                  mes08, mes09, mes10, mes11, mes12, mes13)
        {
            IdCarga = idCarga;
            IdCargaSTGBW = idCargaSTGBW;
            IdHoja = idHoja;
        }
    }
}
