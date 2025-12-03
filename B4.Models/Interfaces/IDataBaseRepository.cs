using B4.Models.Entities;

namespace B4.Models.Interfaces
{
    public interface IDataBaseRepository<T> : IRepository<T> where T : DataBase

    {

        // Add any additional methods specific to DataActuals if needed

    }
}
