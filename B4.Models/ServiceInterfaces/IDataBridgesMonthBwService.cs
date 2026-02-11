using B4.Models.Entities.DataEntities;

namespace B4.Models.ServiceInterfaces;

public interface IDataBridgesMonthBwService
{
    Task<DataBridgesMonthBw?> GetByIdAsync(int id);
    Task<IEnumerable<DataBridgesMonthBw>> GetAllAsync();
}
