using B4.Models.Entities.DataEntities;

namespace B4.Models.ServiceInterfaces;

public interface IDataActualsService
{
    Task<DataActuals?> GetByPlantaEjercicioEpigrafeAsync(int planta, int ejercicio, int epigrafe);
    Task<int> AddRangeAsync(int planta, int ejercicio, int mes, string tipo, IEnumerable<DataActuals> actuals);
}
