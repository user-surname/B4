using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces; // ajusta si usáis otro namespace
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services;

public class DataForecastBwService : IDataForecastBwService
{
    private readonly IDataForecastBwRepository _repo;

    public DataForecastBwService(IDataForecastBwRepository repo)
    {
        _repo = repo;
    }

    public Task<DataForecastBw?> GetByIdAsync(int id)
        => _repo.GetByIdAsync(id);

    public Task<IEnumerable<DataForecastBw>> GetAllAsync()
        => _repo.GetAllAsync();
}
