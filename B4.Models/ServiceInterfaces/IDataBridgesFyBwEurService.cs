using B4.Models.Entities.DataEntities;

namespace B4.Models.ServiceInterfaces;

public interface IDataBridgesFyBwEurService
{
    Task<DataBridgesFyBwEur?> GetByIdAsync(int id);
    Task<IEnumerable<DataBridgesFyBwEur>> GetAllAsync();
}
