using B4.Models.Entities.DataEntities;

namespace B4.Models.RepositoryInterfaces.DataInterfaces
{
    public interface IDataActualsRepository : IRepository<DataActuals>
    {

        // Funciones implementadas para el controller (DataActuals)
        // NOTA: Estas funciones probablemente se reutilizarán en otros
        // controllers (Budget/Forecast/Bridges...)

        Task<IEnumerable<DataActuals>> GetByPlantaEjercicioAsync(int planta, int ejercicio);

        Task<DataActuals?> GetByPlantaEjercicioEpigrafeAsync(int planta, int ejercicio, int epigrafe);
    }

}
