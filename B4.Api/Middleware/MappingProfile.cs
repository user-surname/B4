using AutoMapper;
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

        // Mapeo de DTO a modelo
        CreateMap<CiclosPostDto, LkCiclos>();
    }
}