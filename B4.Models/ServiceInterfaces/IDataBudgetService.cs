using B4.Models.Entities.DataEntities;

namespace B4.Models.ServiceInterfaces;

public interface IDataBudgetService
{
    Task<DataBudget?> GetByIdAsync(int id);
    Task<IEnumerable<DataBudget>> GetAllAsync();

    Task AddAsync(DataBudget entity);
    Task UpdateAsync(DataBudget entity);
    Task DeleteAsync(int id);
}
