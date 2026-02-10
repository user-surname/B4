using B4.Models.Entities.LkEntities;

namespace B4.Models.ServiceInterfaces;

public interface IMemoryCacheService
{
    Task<IReadOnlyList<LkCiclos>> GetCiclosAsync(bool onlyActive = true);
    Task<IReadOnlyList<LkFases>> GetFasesAsync(bool onlyActive = true);
    Task<IReadOnlyList<LkEpigrafe>> GetEpigrafesAsync(bool onlyActive = true);

    Task<IReadOnlyList<LkPlantCountry>> GetPlantCountriesAsync(bool onlyActive = true);
    Task<IReadOnlyList<LkPlantCurrency>> GetPlantCurrenciesAsync(bool onlyActive = true);
    Task<IReadOnlyList<LkPlantDivision>> GetPlantDivisionsAsync(bool onlyActive = true);
    Task<IReadOnlyList<LkPlantDivisionCompany>> GetPlantDivisionCompaniesAsync(bool onlyActive = true);
    Task<IReadOnlyList<LkPlantSubdivision>> GetPlantSubdivisionsAsync(bool onlyActive = true);
    Task<IReadOnlyList<LkPlantTree>> GetPlantTreeAsync(bool onlyActive = true);

    Task<IReadOnlyList<LkPlantCompany>> GetPlantCompaniesAsync(bool onlyActive = true);
    Task<IReadOnlyList<LkPlantControllers>> GetPlantControllersAsync(bool onlyActive = true);

    Task<IReadOnlyList<LkPlantillasBotonesPasosTipos>> GetPlantillasBotonesPasosTiposAsync(bool onlyActive = true);

    void InvalidateCache();
    void InvalidateCache(string cacheKey);
}
