using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services;

public class DataBridgesMonthBwService : IDataBridgesMonthBwService
{
    private readonly IDataBridgesMonthBwRepository _repo;

    public DataBridgesMonthBwService(IDataBridgesMonthBwRepository repo)
    {
        _repo = repo;
    }

    public Task<DataBridgesMonthBw?> GetByIdAsync(int id)
        => _repo.GetByIdAsync(id);

    public Task<IEnumerable<DataBridgesMonthBw>> GetAllAsync()
        => _repo.GetAllAsync();
}
