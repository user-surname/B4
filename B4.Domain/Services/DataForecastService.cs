using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces; // ajusta si usáis otro namespace
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services;

public class DataForecastService : IDataForecastService
{
    private readonly IDataForecastRepository _repo;

    public DataForecastService(IDataForecastRepository repo)
    {
        _repo = repo;
    }

    public Task<DataForecast?> GetByIdAsync(int id)
        => _repo.GetByIdAsync(id);

    public Task<IEnumerable<DataForecast>> GetAllAsync()
        => _repo.GetAllAsync();

    public async Task AddAsync(DataForecast entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        // Valores técnicos comunes
        entity.GuidCarga = Guid.NewGuid();
        entity.FechaUltModif = DateTime.UtcNow;
        entity.IdAPICarga = 1;

        await _repo.AddAsync(entity);
    }

    public async Task UpdateAsync(DataForecast entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        entity.FechaUltModif = DateTime.UtcNow;
        await _repo.UpdateAsync(entity);
    }

    public Task DeleteAsync(int id)
        => _repo.DeleteAsync(id);
}
