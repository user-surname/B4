using AutoMapper;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities.LkEntities;
using B4.Models.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

/// <summary>
/// EJEMPLO MIGRADO: CiclosController usando ApiControllerBase y ApiResponse.
///
/// Cambios respecto a la versión anterior:
///   - Hereda de ApiControllerBase en lugar de ControllerBase
///   - Elimina los objetos anónimos { coderror, msg... } manuales
///   - No necesita try/catch en endpoints simples (el GlobalExceptionHandlerMiddleware lo cubre)
///   - El ResponseWrapperMiddleware puede retirarse o dejarse solo para rutas legacy
/// </summary>
[AllowAnonymous]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CiclosController : ApiControllerBase
{
    private readonly ICiclosService _ciclosService;
    private readonly IMapper _mapper;

    public CiclosController(ICiclosService ciclosService, IMapper mapper)
    {
        _ciclosService = ciclosService;
        _mapper = mapper;
    }

    // GET api/v1/ciclos
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await _ciclosService.GetAllAsync();
        var dtos = _mapper.Map<List<CiclosGetDto>>(records);
        return OkResponse(dtos);
    }

    // GET api/v1/ciclos/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        // ExecuteAsync mide el tiempo y gestiona el 404 automáticamente
        return await ExecuteAsync(
            () => _ciclosService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : _mapper.Map<CiclosGetDto>(t.Result)),
            $"No existe ciclo para id={id}"
        );
    }

    // POST api/v1/ciclos
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CiclosPostDto dto)
    {
        if (dto is null)
            return BadRequestResponse<CiclosGetDto>("El cuerpo de la petición no puede estar vacío");

        if (!ModelState.IsValid)
            return BadRequestResponse<CiclosGetDto>("Los datos enviados no son válidos");

        var ciclo = _mapper.Map<LkCiclos>(dto);
        await _ciclosService.AddAsync(ciclo);

        var resultDto = _mapper.Map<CiclosGetDto>(ciclo);
        return CreatedResponse(nameof(GetById), new { id = ciclo.IdCiclo }, resultDto);
    }

    // DELETE api/v1/ciclos/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        // KeyNotFoundException es capturada por GlobalExceptionHandlerMiddleware,
        // pero si quieres un 404 controlado en lugar de un 500, mantenlo aquí.
        var existing = await _ciclosService.GetByIdAsync(id);
        if (existing is null)
            return NotFoundResponse<CiclosGetDto>($"No existe ciclo con id={id}");

        await _ciclosService.DeleteAsync(id);
        return OkResponse(new { id, deleted = true });
    }
}