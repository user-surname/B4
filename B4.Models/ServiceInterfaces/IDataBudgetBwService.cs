using B4.Models.Entities.DataEntities;

namespace B4.Models.ServiceInterfaces;

public interface IDataBudgetBwService
{
    Task<DataBudgetBw?> GetByIdAsync(int id);
    Task<IEnumerable<DataBudgetBw>> GetAllAsync();
}