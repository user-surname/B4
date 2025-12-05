using B4.Models.Entities.DataEtities;

namespace B4.Models.Interfaces.DataInterfaces
{
    public interface IDataBaseRepository<T> : IRepository<T> where T : DataFinancial

    {

        // Add any additional methods specific to DataActuals if needed

    }
}
