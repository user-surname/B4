using B4.Models.Entities.DataEntities;

namespace B4.Models.ServiceInterfaces;

public interface IDataBridgesMonthService
{
    Task<DataBridgesMonth?> GetByIdAsync(int id);
    Task<IEnumerable<DataBridgesMonth>> GetAllAsync();

    Task AddAsync(DataBridgesMonth entity);
    Task UpdateAsync(DataBridgesMonth entity);
    Task DeleteAsync(int id);
}