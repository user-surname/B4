using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces; // ajusta si tu namespace es otro
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services;

public class DataTipoCambioService : IDataTipoCambioService
{
    private readonly IDataTipoCambioRepository _repo;

    public DataTipoCambioService(IDataTipoCambioRepository repo)
    {
        _repo = repo;
    }

    public Task<DataTipoCambio?> GetByIdAsync(int id)
        => _repo.GetByIdAsync(id);

    public Task<IEnumerable<DataTipoCambio>> GetAllAsync()
        => _repo.GetAllAsync();

    public async Task AddAsync(DataTipoCambio entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        entity.GuidCarga = Guid.NewGuid();
        entity.FechaUltModif = DateTime.UtcNow;
        entity.IdAPICarga = 1;

        await _repo.AddAsync(entity);
    }

    public async Task UpdateAsync(DataTipoCambio entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        entity.FechaUltModif = DateTime.UtcNow;
        await _repo.UpdateAsync(entity);
    }

    public Task DeleteAsync(int id)
        => _repo.DeleteAsync(id);

    // -------------------------
    // Consultas habituales
    // -------------------------
    public Task<IEnumerable<DataTipoCambio>> GetByEjercicioAsync(int ejercicio)
        => _repo.GetByEjercicioAsync(ejercicio);

    public Task<IEnumerable<DataTipoCambio>> GetByEjercicioCurrencyAsync(int ejercicio, int idCurrency)
        => _repo.GetByEjercicioCurrencyAsync(ejercicio, idCurrency);
}
