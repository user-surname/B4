using B4.Models.Entities;

namespace B4.Models.Interfaces
{
    public interface IDataBwBaseRepository<T> : IRepository<T> where T : DataBwBase

    {

        // Add any additional methods specific to DataActuals if needed

    }
}
