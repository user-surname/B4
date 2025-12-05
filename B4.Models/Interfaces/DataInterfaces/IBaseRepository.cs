using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B4.Models.Interfaces.DataInterfaces
{
    public interface IBaseRepository<T> where T : class
    {

        Task AddAsync();
        Task UpdateAsync();

        Task<T?> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteAsync(int id);

    }
}
