using B4.Models.Entities.DataEntities;

namespace B4.Models.ServiceInterfaces;

public interface IDataBridgesFyBwService
{
    Task<DataBridgesFyBw?> GetByIdAsync(int id);
    Task<IEnumerable<DataBridgesFyBw>> GetAllAsync();
}
