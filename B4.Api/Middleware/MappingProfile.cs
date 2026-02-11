using AutoMapper;
using B4.Api.Controllers;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities;
using B4.Models.Entities.LkEntities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace B4.Api.Middleware;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeo de modelo a DTO
        CreateMap<LkCiclos, CiclosGetDto>();
        CreateMap<LkEpigrafe, EpigrafeGetDto>();
        CreateMap<LkFases, FasesGetDto>();
        CreateMap<LkPlantCompany, PlantCompanyGetDto>();
        CreateMap<LkPlantControllers, PlantControllersGetDto>();
        CreateMap<LkPlantCountry, PlantCountryGetDto>();
        CreateMap<LkPlantCurrency, PlantCurrencyGetDto>();
        CreateMap<LkPlantDivisionCompany, PlantDivisionCompanyGetDto>();
        CreateMap<LkPlantDivision, PlantDivisionGetDto>();
        CreateMap<LkPlantillasBotonesPasosTipos, PlantillasBotonesPasosTiposGetDto>();
        CreateMap<LkPlantSubdivision, PlantSubdivisionGetDto>();
        CreateMap<LkPlantTree, PlantTreeGetDto>();
        CreateMap<Control, ControlGetDto>();


        // Mapeo de DTO a modelo
        CreateMap<CiclosPostDto, LkCiclos>();
        CreateMap<EpigrafePostDto, LkEpigrafe>();
        CreateMap<FasesPostDto, LkFases>();
        CreateMap<PlantCompanyPostDto, LkPlantCompany>();
        CreateMap<PlantControllersPostDto, LkPlantControllers>();
        CreateMap<PlantCountryPostDto, LkPlantCountry>();
        CreateMap<PlantCurrencyPostDto, LkPlantCurrency>();
        CreateMap<PlantDivisionCompanyPostDto, LkPlantDivisionCompany>();
        CreateMap<PlantDivisionPostDto, LkPlantDivision>();
        CreateMap<PlantillasBotonesPasosTiposPostDto, LkPlantillasBotonesPasosTipos>();
        CreateMap<PlantSubdivisionPostDto, LkPlantSubdivision>();
        CreateMap<PlantTreePostDto, LkPlantTree>();
        CreateMap<ControlPostDto, Control>();


    }
}