using B4.Models.Entities.DataEntities;

namespace B4.Models.ServiceInterfaces;

public interface IDataTipoCambioService
{
    Task<DataTipoCambio?> GetByIdAsync(int id);
    Task<IEnumerable<DataTipoCambio>> GetAllAsync();

    // CRUD básico
    Task AddAsync(DataTipoCambio entity);
    Task UpdateAsync(DataTipoCambio entity);
    Task DeleteAsync(int id);

    // Consultas típicas (opcional pero muy útil)
    Task<IEnumerable<DataTipoCambio>> GetByEjercicioAsync(int ejercicio);
    Task<IEnumerable<DataTipoCambio>> GetByEjercicioCurrencyAsync(int ejercicio, int idCurrency);
}
