using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using B4.Models.Entities.LkEntities;
using B4.Models.Interfaces.LkInterfaces;

namespace B4.Data.PostgreSQL.Services;

public class MemoryCacheService : IMemoryCacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<MemoryCacheService> _logger;

    private readonly ICiclosRepository _ciclosRepo;
    private readonly IFasesRepository _fasesRepo;
    private readonly IEpigrafeRepository _epigrafeRepo;

    private readonly IPlantCountryRepository _plantCountryRepo;
    private readonly IPlantCurrencyRepository _plantCurrencyRepo;
    private readonly IPlantDivisionRepository _plantDivisionRepo;
    private readonly IPlantDivisionCompanyRepository _plantDivisionCompanyRepo;
    private readonly IPlantSubdivisionRepository _plantSubdivisionRepo;
    private readonly IPlantTreeRepository _plantTreeRepo;

    private readonly IPlantCompanyRepository _plantCompanyRepo;
    private readonly IPlantControllersRepository _plantControllersRepo;

    private readonly IPlantillasBotonesPasosTiposRepository _plantillasBotonesPasosTiposRepo;

    // Expiraciones recomendadas para LK

    //TODO: Parametrizar los tiempos en app.conf
    private static readonly TimeSpan Sliding = TimeSpan.FromHours(1);
    private static readonly TimeSpan Absolute = TimeSpan.FromHours(24);

    public MemoryCacheService(
        IMemoryCache cache,
        ILogger<MemoryCacheService> logger,
        ICiclosRepository ciclosRepo,
        IFasesRepository fasesRepo,
        IEpigrafeRepository epigrafeRepo,
        IPlantCountryRepository plantCountryRepo,
        IPlantCurrencyRepository plantCurrencyRepo,
        IPlantDivisionRepository plantDivisionRepo,
        IPlantDivisionCompanyRepository plantDivisionCompanyRepo,
        IPlantSubdivisionRepository plantSubdivisionRepo,
        IPlantTreeRepository plantTreeRepo,
        IPlantCompanyRepository plantCompanyRepo,
        IPlantControllersRepository plantControllersRepo,
        IPlantillasBotonesPasosTiposRepository plantillasBotonesPasosTiposRepo)
    {
        _cache = cache;
        _logger = logger;

        _ciclosRepo = ciclosRepo;
        _fasesRepo = fasesRepo;
        _epigrafeRepo = epigrafeRepo;

        _plantCountryRepo = plantCountryRepo;
        _plantCurrencyRepo = plantCurrencyRepo;
        _plantDivisionRepo = plantDivisionRepo;
        _plantDivisionCompanyRepo = plantDivisionCompanyRepo;
        _plantSubdivisionRepo = plantSubdivisionRepo;
        _plantTreeRepo = plantTreeRepo;

        _plantCompanyRepo = plantCompanyRepo;
        _plantControllersRepo = plantControllersRepo;

        _plantillasBotonesPasosTiposRepo = plantillasBotonesPasosTiposRepo;
    }

    // -----------------------
    // Helper genérico
    // -----------------------
    private Task<IReadOnlyList<T>> GetOrCreateAsync<T>(
    string key,
    Func<Task<IEnumerable<T>>> loader)
{
    return _cache.GetOrCreateAsync<IReadOnlyList<T>>(key, async entry =>
    {
        _logger.LogDebug("CACHE MISS -> {Key}", key);

        entry.SlidingExpiration = Sliding;
        entry.AbsoluteExpirationRelativeToNow = Absolute;

        var data = await loader();

        // OJO: devolvemos List<T> como IReadOnlyList<T>
        return data.ToList();
    })!;
}

    private static IEnumerable<T> OnlyActive<T>(IEnumerable<T> list) where T : LkBase
        => list.Where(x => x.IsActive == 1);

    // -----------------------
    // Getters LK_*
    // -----------------------

    //TODO: Revisar el enfoque de la cache de activos
    public Task<IReadOnlyList<LkCiclos>> GetCiclosAsync(bool onlyActive = true) =>
        GetOrCreateAsync(
            onlyActive ? CacheKeys.LkCiclosActive : CacheKeys.LkCiclosAll,
            async () =>
            {
                var data = await _ciclosRepo.GetAllAsync();
                return onlyActive ? OnlyActive(data) : data;
            });

    public Task<IReadOnlyList<LkFases>> GetFasesAsync(bool onlyActive = true) =>
        GetOrCreateAsync(
            onlyActive ? CacheKeys.LkFasesActive : CacheKeys.LkFasesAll,
            async () =>
            {
                var data = await _fasesRepo.GetAllAsync();
                return onlyActive ? OnlyActive(data) : data;
            });

    public Task<IReadOnlyList<LkEpigrafe>> GetEpigrafesAsync(bool onlyActive = true) =>
        GetOrCreateAsync(
            onlyActive ? CacheKeys.LkEpigrafesActive : CacheKeys.LkEpigrafesAll,
            async () =>
            {
                var data = await _epigrafeRepo.GetAllAsync();
                return onlyActive ? OnlyActive(data) : data;
            });

    public Task<IReadOnlyList<LkPlantCountry>> GetPlantCountriesAsync(bool onlyActive = true) =>
        GetOrCreateAsync(
            onlyActive ? CacheKeys.LkPlantCountryActive : CacheKeys.LkPlantCountryAll,
            async () =>
            {
                var data = await _plantCountryRepo.GetAllAsync();
                return onlyActive ? OnlyActive(data) : data;
            });

    public Task<IReadOnlyList<LkPlantCurrency>> GetPlantCurrenciesAsync(bool onlyActive = true) =>
        GetOrCreateAsync(
            onlyActive ? CacheKeys.LkPlantCurrencyActive : CacheKeys.LkPlantCurrencyAll,
            async () =>
            {
                var data = await _plantCurrencyRepo.GetAllAsync();
                return onlyActive ? OnlyActive(data) : data;
            });

    public Task<IReadOnlyList<LkPlantDivision>> GetPlantDivisionsAsync(bool onlyActive = true) =>
        GetOrCreateAsync(
            onlyActive ? CacheKeys.LkPlantDivisionActive : CacheKeys.LkPlantDivisionAll,
            async () =>
            {
                var data = await _plantDivisionRepo.GetAllAsync();
                return onlyActive ? OnlyActive(data) : data;
            });

    public Task<IReadOnlyList<LkPlantDivisionCompany>> GetPlantDivisionCompaniesAsync(bool onlyActive = true) =>
        GetOrCreateAsync(
            onlyActive ? CacheKeys.LkPlantDivisionCompanyActive : CacheKeys.LkPlantDivisionCompanyAll,
            async () =>
            {
                var data = await _plantDivisionCompanyRepo.GetAllAsync();
                return onlyActive ? OnlyActive(data) : data;
            });

    public Task<IReadOnlyList<LkPlantSubdivision>> GetPlantSubdivisionsAsync(bool onlyActive = true) =>
        GetOrCreateAsync(
            onlyActive ? CacheKeys.LkPlantSubdivisionActive : CacheKeys.LkPlantSubdivisionAll,
            async () =>
            {
                var data = await _plantSubdivisionRepo.GetAllAsync();
                return onlyActive ? OnlyActive(data) : data;
            });

    public Task<IReadOnlyList<LkPlantTree>> GetPlantTreeAsync(bool onlyActive = true) =>
        GetOrCreateAsync(
            onlyActive ? CacheKeys.LkPlantTreeActive : CacheKeys.LkPlantTreeAll,
            async () =>
            {
                var data = await _plantTreeRepo.GetAllAsync();
                return onlyActive ? OnlyActive(data) : data;
            });

    public Task<IReadOnlyList<LkPlantCompany>> GetPlantCompaniesAsync(bool onlyActive = true) =>
        GetOrCreateAsync(
            onlyActive ? CacheKeys.LkPlantCompanyActive : CacheKeys.LkPlantCompanyAll,
            async () =>
            {
                var data = await _plantCompanyRepo.GetAllAsync();
                return onlyActive ? OnlyActive(data) : data;
            });

    public Task<IReadOnlyList<LkPlantControllers>> GetPlantControllersAsync(bool onlyActive = true) =>
        GetOrCreateAsync(
            onlyActive ? CacheKeys.LkPlantControllersActive : CacheKeys.LkPlantControllersAll,
            async () =>
            {
                var data = await _plantControllersRepo.GetAllAsync();
                return onlyActive ? OnlyActive(data) : data;
            });

    public Task<IReadOnlyList<LkPlantillasBotonesPasosTipos>> GetPlantillasBotonesPasosTiposAsync(bool onlyActive = true) =>
        GetOrCreateAsync(
            onlyActive ? CacheKeys.LkPlantillasBotonesPasosTiposActive : CacheKeys.LkPlantillasBotonesPasosTiposAll,
            async () =>
            {
                var data = await _plantillasBotonesPasosTiposRepo.GetAllAsync();
                return onlyActive ? OnlyActive(data) : data;
            });

    // -----------------------
    // Invalidación
    // -----------------------
    public void InvalidateCache()
    {
        foreach (var key in CacheKeys.AllKeys)
            _cache.Remove(key);

        _logger.LogDebug("InvalidateCache -> ALL LK keys removed");
    }

    public void InvalidateCache(string cacheKey)
    {
        _cache.Remove(cacheKey);
        _logger.LogDebug("InvalidateCache -> {Key}", cacheKey);
    }
}
