using B4.Models.Entities.DataEntities;

namespace B4.Models.ServiceInterfaces;

public interface IDataForecastBwService
{
    Task<DataForecastBw?> GetByIdAsync(int id);
    Task<IEnumerable<DataForecastBw>> GetAllAsync();
}