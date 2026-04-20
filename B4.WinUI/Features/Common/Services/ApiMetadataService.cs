using B4.WinUI.Features.Common.Interfaces;
using B4.WinUI.Features.Common.Models;

namespace B4.WinUI.Features.Common.Services;

public sealed class ApiMetadataService : IApiMetadataService
{
    private static readonly IReadOnlyList<ApiEndpointDefinition> Endpoints =
    [
        new() { ControllerName = "Ciclos", RouteSegment = "ciclos", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "Control", RouteSegment = "control", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "DataBridgesFy", RouteSegment = "databridgesfy", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "DataBridgesMonth", RouteSegment = "databridgesmonth", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "DataBudget", RouteSegment = "databudget", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "DataComentarios", RouteSegment = "datacomentarios", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "DataForecast", RouteSegment = "dataforecast", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "LogActividad", RouteSegment = "logactividad", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "DataTipoCambio", RouteSegment = "datatipocambio", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "Epigrafe", RouteSegment = "epigrafe", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "Fases", RouteSegment = "fases", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "PlantCompany", RouteSegment = "plantcompany", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "PlantControllers", RouteSegment = "plantcontrollers", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "PlantCountry", RouteSegment = "plantcountry", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "PlantCurrency", RouteSegment = "plantcurrency", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "PlantDivision", RouteSegment = "plantdivision", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "PlantDivisionCompany", RouteSegment = "plantdivisioncompany", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "PlantillasBotonesPasosTipos", RouteSegment = "plantillasbotonespasostipos", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "PlantSubdivision", RouteSegment = "plantsubdivision", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true },
        new() { ControllerName = "PlantTree", RouteSegment = "planttree", SupportsGetAll = true, SupportsGetById = true, SupportsCreate = true, SupportsDelete = true }
    ];

    public IReadOnlyList<ApiEndpointDefinition> GetCrudEndpoints() => Endpoints;
}
