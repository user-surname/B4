using B4.Models.Entities.DataEntities;

namespace B4.Models.ServiceInterfaces;

public interface IDataForecastService
{
    Task<DataForecast?> GetByIdAsync(int id);
    Task<IEnumerable<DataForecast>> GetAllAsync();

    Task AddAsync(DataForecast entity);
    Task UpdateAsync(DataForecast entity);
    Task DeleteAsync(int id);
}
