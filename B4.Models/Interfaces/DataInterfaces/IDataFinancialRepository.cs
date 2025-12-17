using B4.Models.Entities.DataEntities;

namespace B4.Models.Interfaces.DataInterfaces
{
    public interface IDataFinancialRepository<T> : IRepository<T> where T : DataBaseFinanciero

    {

        // Add any additional methods specific to DataActuals if needed

    }
}
