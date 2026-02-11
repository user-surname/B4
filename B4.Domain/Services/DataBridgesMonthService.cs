using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces; // ajusta si tu namespace es otro
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services;

public class DataBridgesMonthService : IDataBridgesMonthService
{
    private readonly IDataBridgesMonthRepository _repo;

    public DataBridgesMonthService(IDataBridgesMonthRepository repo)
    {
        _repo = repo;
    }

    public Task<DataBridgesMonth?> GetByIdAsync(int id)
        => _repo.GetByIdAsync(id);

    public Task<IEnumerable<DataBridgesMonth>> GetAllAsync()
        => _repo.GetAllAsync();

    public async Task AddAsync(DataBridgesMonth entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        entity.GuidCarga ??= Guid.NewGuid();
        entity.FechaUltModif = DateTime.UtcNow;
        entity.IdAPICarga = 1;

        await _repo.AddAsync(entity);
    }

    public async Task UpdateAsync(DataBridgesMonth entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        entity.FechaUltModif = DateTime.UtcNow;
        await _repo.UpdateAsync(entity);
    }

    public Task DeleteAsync(int id)
        => _repo.DeleteAsync(id);
}
