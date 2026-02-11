using B4.Models.Entities.DataEntities;

namespace B4.Models.RepositoryInterfaces.DataInterfaces
{
    public interface IDataTipoCambioRepository : IRepository<DataTipoCambio>
    {

        // Funciones implementadas para el controller (DataTipoCambio)
        // NOTA: Estas funciones probablemente se reutilizarán en otros
        // controllers (Budget/Forecast/Bridges...)

        Task<IEnumerable<DataTipoCambio>> GetByEjercicioAsync(int ejercicio);
        Task<IEnumerable<DataTipoCambio>> GetByEjercicioCurrencyAsync(int ejercicio, int idCurrency);


    }
}
