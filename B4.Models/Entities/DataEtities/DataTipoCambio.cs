using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Entities.DataEtities
{
    public class DataTipoCambio : DataBase
    {
        public int Id { get; set; }
        public int IdAPICarga { get; set; }
        public Guid GuidCarga { get; set; } = Guid.Empty;
        public DateTime FechaUltModif { get; set; }
        public int Ejercicio { get; set; }
        public int IdCurrency { get; set; }
        public DateTime CalendarDay { get; set; }
        public int Mes { get; set; }
        public decimal? P { get; set; }
        public decimal? FC { get; set; }
        public decimal? FB { get; set; }
        public int? IdCarga { get; set; }
        public int? IdCargaSTGBW { get; set; }
        public int? IdHoja { get; set; }

        // Constructor vacío
        public DataTipoCambio() { }

        // Constructor con parámetros
        public DataTipoCambio(
            int id,
            int idAPICarga,
            Guid guidCarga,
            DateTime fechaUltModif,
            int ejercicio,
            int idCurrency,
            DateTime calendarDay,
            int mes,
            decimal? p,
            decimal? fc,
            decimal? fb,
            int? idCarga,
            int? idCargaSTGBW,
            int? idHoja,
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
            Ejercicio = ejercicio;
            IdCurrency = idCurrency;
            CalendarDay = calendarDay;
            Mes = mes;
            P = p;
            FC = fc;
            FB = fb;
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
