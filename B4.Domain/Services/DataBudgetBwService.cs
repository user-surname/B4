using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces; // ajusta si tu namespace es otro
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services;

public class DataBudgetBwService : IDataBudgetBwService
{
    private readonly IDataBudgetBwRepository _repo;

    public DataBudgetBwService(IDataBudgetBwRepository repo)
    {
        _repo = repo;
    }

    public Task<DataBudgetBw?> GetByIdAsync(int id)
        => _repo.GetByIdAsync(id);

    public Task<IEnumerable<DataBudgetBw>> GetAllAsync()
        => _repo.GetAllAsync();
}
