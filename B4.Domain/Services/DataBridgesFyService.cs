using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces; // ajusta si tu namespace es otro
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services;

public class DataBridgesFyService : IDataBridgesFyService
{
    private readonly IDataBridgesFyRepository _repo;

    public DataBridgesFyService(IDataBridgesFyRepository repo)
    {
        _repo = repo;
    }

    public Task<DataBridgesFy?> GetByIdAsync(int id)
        => _repo.GetByIdAsync(id);

    public Task<IEnumerable<DataBridgesFy>> GetAllAsync()
        => _repo.GetAllAsync();

    public async Task AddAsync(DataBridgesFy entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        entity.GuidCarga ??= Guid.NewGuid();
        entity.FechaUltModif = DateTime.UtcNow;
        entity.IdAPICarga = 1;

        await _repo.AddAsync(entity);
    }

    public async Task UpdateAsync(DataBridgesFy entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        entity.FechaUltModif = DateTime.UtcNow;
        await _repo.UpdateAsync(entity);
    }

    public Task DeleteAsync(int id)
        => _repo.DeleteAsync(id);
}
