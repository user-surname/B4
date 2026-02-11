using B4.Models.Entities.DataEntities;

namespace B4.Models.ServiceInterfaces;

public interface IDataBridgesFyService
{
    Task<DataBridgesFy?> GetByIdAsync(int id);
    Task<IEnumerable<DataBridgesFy>> GetAllAsync();

    Task AddAsync(DataBridgesFy entity);
    Task UpdateAsync(DataBridgesFy entity);
    Task DeleteAsync(int id);
}
