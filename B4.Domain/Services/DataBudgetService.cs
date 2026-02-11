using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces; // <-- o B4.Models.Interfaces.DataInterfaces (según tu proyecto)
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services;

public class DataBudgetService : IDataBudgetService
{
    private readonly IDataBudgetRepository _repo;

    public DataBudgetService(IDataBudgetRepository repo)
    {
        _repo = repo;
    }

    public Task<DataBudget?> GetByIdAsync(int id)
        => _repo.GetByIdAsync(id);

    public Task<IEnumerable<DataBudget>> GetAllAsync()
        => _repo.GetAllAsync();

    public async Task AddAsync(DataBudget entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        // Defaults técnicos típicos
        entity.GuidCarga = Guid.NewGuid();
        entity.FechaUltModif = DateTime.UtcNow;
        entity.IdAPICarga = 1;

        await _repo.AddAsync(entity);
    }

    public async Task UpdateAsync(DataBudget entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        entity.FechaUltModif = DateTime.UtcNow;
        await _repo.UpdateAsync(entity);
    }

    public Task DeleteAsync(int id)
        => _repo.DeleteAsync(id);
}
