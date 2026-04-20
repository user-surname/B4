using B4.Models.Entities.DataEntities;

namespace B4.Models.ServiceInterfaces;

public interface ILogActividadService
{
    Task<LogActividad?> GetByIdAsync(int id);
    Task<IEnumerable<LogActividad>> GetAllAsync();
    Task AddAsync(LogActividad entity);
    Task UpdateAsync(LogActividad entity);
    Task DeleteAsync(int id);
}
