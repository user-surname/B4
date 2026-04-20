using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services;

public class LogActividadService : ILogActividadService
{
    private readonly ILogActividadRepository _repo;

    public LogActividadService(ILogActividadRepository repo)
    {
        _repo = repo;
    }

    public Task<LogActividad?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

    public Task<IEnumerable<LogActividad>> GetAllAsync() => _repo.GetAllAsync();

    public Task AddAsync(LogActividad entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return _repo.AddAsync(entity);
    }

    public Task UpdateAsync(LogActividad entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        return _repo.UpdateAsync(entity);
    }

    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
}
