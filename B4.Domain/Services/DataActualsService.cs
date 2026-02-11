using B4.Models.Entities.DataEntities;
using B4.Models.RepositoryInterfaces.DataInterfaces;
using B4.Models.ServiceInterfaces;

namespace B4.Domain.Services;

public class DataActualsService : IDataActualsService
{
    private readonly IDataActualsRepository _repo;

    public DataActualsService(IDataActualsRepository repo)
    {
        _repo = repo;
    }

    public Task<DataActuals?> GetByPlantaEjercicioEpigrafeAsync(int planta, int ejercicio, int epigrafe)
        => _repo.GetByPlantaEjercicioEpigrafeAsync(planta, ejercicio, epigrafe);

    public async Task<int> AddRangeAsync(int planta, int ejercicio, int mes, string tipo, IEnumerable<DataActuals> actuals)
    {
        int idCurrency = DeterminarMoneda(tipo);
        Guid guidCarga = Guid.NewGuid();
        DateTime fechaActual = DateTime.UtcNow;

        int inserted = 0;

        foreach (var entity in actuals)
        {
            entity.IdCompany = planta;
            entity.Ejercicio = ejercicio;
            entity.IdCiclo = 0;
            entity.IdFase = 0;
            entity.IdCurrency = idCurrency;
            entity.IdAPICarga = 1;
            entity.GuidCarga = guidCarga;
            entity.FechaUltModif = fechaActual;

            await _repo.AddAsync(entity);
            inserted++;
        }

        return inserted;
    }

    private static int DeterminarMoneda(string tipo)
        => tipo.Equals("EUR", StringComparison.OrdinalIgnoreCase) ? 2 : 1;
}
