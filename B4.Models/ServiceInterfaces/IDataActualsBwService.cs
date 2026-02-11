using B4.Models.Entities.DataEntities;

namespace B4.Models.ServiceInterfaces;

public interface IDataActualsBwService
{
    Task<DataActualsBw?> GetByIdAsync(int id);
    Task<IEnumerable<DataActualsBw>> GetAllAsync();
}