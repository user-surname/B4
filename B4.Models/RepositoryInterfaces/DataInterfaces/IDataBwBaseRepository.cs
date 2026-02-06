using B4.Models.Entities.DataEntities;

namespace B4.Models.RepositoryInterfaces.DataInterfaces
{
    public interface IDataBwBaseRepository<T> : IRepository<T> where T : DataBaseFinanciero

    {

        // Add any additional methods specific to DataActuals if needed

    }
}
