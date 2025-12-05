using B4.Models.Entities.DataEtities;

namespace B4.Models.Interfaces.DataInterfaces
{
    public interface IDataBwBaseRepository<T> : IRepository<T> where T : DataFinancial

    {

        // Add any additional methods specific to DataActuals if needed

    }
}
