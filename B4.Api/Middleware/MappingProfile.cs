using AutoMapper;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities;
using B4.Models.Entities.DataEntities;
using B4.Models.Entities.LkEntities;
using System;
using System.Collections.Generic;

namespace B4.Api.Middleware;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ==========================
        // LK / CONTROL - Modelo -> DTO
        // ==========================
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

        // ==========================
        // LK / CONTROL - DTO -> Modelo
        // ==========================
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

        // ======================================================
        // DATA ACTUALS - Lo necesario para DataActualsController
        // ======================================================

        // POST: DTO -> Entity
        CreateMap<DataActualsPostDto, DataActuals>()
            .ForMember(d => d.Mes00, o => o.MapFrom(_ => 0m));

        // GET: Entity -> DTO (Head + Detail)
        CreateMap<DataActuals, DataActualsGetDto>()
            .ConvertUsing((src, _, ctx) =>
            {
                var planta = ctx.Items.TryGetValue("planta", out var p) ? p?.ToString() ?? "" : "";
                var ejercicio = ctx.Items.TryGetValue("ejercicio", out var e) ? Convert.ToInt32(e) : 0;

                return new DataActualsGetDto
                {
                    Head = new DataActualsGetDto.HeadDto
                    {
                        Timestamp = DateTime.UtcNow,
                        Planta = planta,
                        Ejercicio = ejercicio,
                        IdCiclo = src.IdCiclo,
                        IdFase = src.IdFase.ToString(),
                        Moneda = src.IdCurrency == 0 ? "PLN" : "EUR"
                    },
                    Detail = new List<DataActualsGetDto.DetailDto>
                    {
                        new()
                        {
                            IdEpigrafe = src.IdEpigrafe,
                            Valores = new decimal[]
                            {
                                src.Mes00, src.Mes01, src.Mes02, src.Mes03,
                                src.Mes04, src.Mes05, src.Mes06, src.Mes07,
                                src.Mes08, src.Mes09, src.Mes10, src.Mes11,
                                src.Mes12, src.Mes13
                            }
                        }
                    }
                };
            });

        // ==========================
        // DATA - entity->entity (placeholder)
        // ==========================
        CreateMap<DataActualsBw, DataActualsBw>();

        CreateMap<DataBudget, DataBudget>();
        CreateMap<DataBudgetBw, DataBudgetBw>();

        CreateMap<DataBridgesFy, DataBridgesFy>();
        CreateMap<DataBridgesFyBw, DataBridgesFyBw>();
        CreateMap<DataBridgesFyBwEur, DataBridgesFyBwEur>();

        CreateMap<DataBridgesMonth, DataBridgesMonth>();
        CreateMap<DataBridgesMonthBw, DataBridgesMonthBw>();

        CreateMap<DataForecast, DataForecast>();
        CreateMap<DataForecastBw, DataForecastBw>();

        CreateMap<DataComentarios, DataComentarios>();
        CreateMap<DataTipoCambio, DataTipoCambio>();
    }
}
