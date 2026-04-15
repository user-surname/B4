using AutoMapper;
using B4.Api.Dto.GetDto;
using B4.Api.Dto.PostDto;
using B4.Models.Entities.DataEntities;
using B4.Models.ServiceInterfaces;
using B4.Api.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace B4.Api.Controllers;

[CustomAuthorize(Policy = "AdminOnly")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DataComentariosController : ApiControllerBase
{
    private readonly IDataComentariosService _dataComentariosService;
    private readonly IMapper _mapper;
    private readonly ILogger<DataComentariosController> _logger;

    public DataComentariosController(IDataComentariosService dataComentariosService, IMapper mapper, ILogger<DataComentariosController> logger)
    {
        _dataComentariosService = dataComentariosService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/dataComentarios
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll dataComentarios requested");
        var records = await _dataComentariosService.GetAllAsync();
        _logger.LogInformation("GetAll dataComentarios returned {Count} records", records?.Count() ?? 0);
        return OkResponse(records);
    }

    // GET api/v1/dataComentarios/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById dataComentarios requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _dataComentariosService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : (object)t.Result),
            $"No existe dataComentarios para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById dataComentarios not found. Id: {Id}", id);
        return result;
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DataComentarios entity)
    {
        _logger.LogInformation("Create DataComentarios requested");
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create DataComentarios rejected: invalid model state");
            return BadRequestResponse<object>("Los datos enviados no son válidos");
        }
        await _dataComentariosService.AddAsync(entity);
        _logger.LogInformation("Create DataComentarios succeeded. Id: {Id}", entity.Id);
        return OkResponse(entity);
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DataComentarios entity)
    {
        _logger.LogInformation("Update DataComentarios requested. Id: {Id}", id);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Update DataComentarios rejected: invalid model state. Id: {Id}", id);
            return BadRequestResponse<object>("Los datos enviados no son válidos");
        }
        if (entity.Id != 0 && entity.Id != id)
        {
            _logger.LogWarning("Update DataComentarios rejected: route id {RouteId} does not match body id {BodyId}", id, entity.Id);
            return BadRequestResponse<object>("El id de la ruta no coincide con el id del body.");
        }
        entity.Id = id;
        await _dataComentariosService.UpdateAsync(entity);
        _logger.LogInformation("Update DataComentarios succeeded. Id: {Id}", id);
        return OkResponse(entity);
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete DataComentarios requested. Id: {Id}", id);
        try
        {
            await _dataComentariosService.DeleteAsync(id);
            _logger.LogInformation("Delete DataComentarios succeeded. Id: {Id}", id);
            return OkResponse(new { deleted = true });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Delete DataComentarios failed. Id: {Id}", id);
            return NotFoundResponse<object>(ex.Message);
        }
    }
}
