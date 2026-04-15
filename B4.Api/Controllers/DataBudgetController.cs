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
public class DataBudgetController : ApiControllerBase
{
    private readonly IDataBudgetService _dataBudgetService;
    private readonly IMapper _mapper;
    private readonly ILogger<DataBudgetController> _logger;

    public DataBudgetController(IDataBudgetService dataBudgetService, IMapper mapper, ILogger<DataBudgetController> logger)
    {
        _dataBudgetService = dataBudgetService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET api/v1/dataBudget
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll dataBudget requested");
        var records = await _dataBudgetService.GetAllAsync();
        _logger.LogInformation("GetAll dataBudget returned {Count} records", records?.Count() ?? 0);
        return OkResponse(records);
    }

    // GET api/v1/dataBudget/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GetById dataBudget requested. Id: {Id}", id);
        var result = await ExecuteAsync(
            () => _dataBudgetService.GetByIdAsync(id)
                                .ContinueWith(t => t.Result is null ? null : (object)t.Result),
            $"No existe dataBudget para id={id}"
        );
        if (result is NotFoundObjectResult)
            _logger.LogWarning("GetById dataBudget not found. Id: {Id}", id);
        return result;
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DataBudget entity)
    {
        _logger.LogInformation("Create DataBudget requested");
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Create DataBudget rejected: invalid model state");
            return BadRequestResponse<object>("Los datos enviados no son válidos");
        }
        await _dataBudgetService.AddAsync(entity);
        _logger.LogInformation("Create DataBudget succeeded. Id: {Id}", entity.Id);
        return OkResponse(entity);
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DataBudget entity)
    {
        _logger.LogInformation("Update DataBudget requested. Id: {Id}", id);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Update DataBudget rejected: invalid model state. Id: {Id}", id);
            return BadRequestResponse<object>("Los datos enviados no son válidos");
        }
        if (entity.Id != 0 && entity.Id != id)
        {
            _logger.LogWarning("Update DataBudget rejected: route id {RouteId} does not match body id {BodyId}", id, entity.Id);
            return BadRequestResponse<object>("El id de la ruta no coincide con el id del body.");
        }
        entity.Id = id;
        await _dataBudgetService.UpdateAsync(entity);
        _logger.LogInformation("Update DataBudget succeeded. Id: {Id}", id);
        return OkResponse(entity);
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete DataBudget requested. Id: {Id}", id);
        try
        {
            await _dataBudgetService.DeleteAsync(id);
            _logger.LogInformation("Delete DataBudget succeeded. Id: {Id}", id);
            return OkResponse(new { deleted = true });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Delete DataBudget failed. Id: {Id}", id);
            return NotFoundResponse<object>(ex.Message);
        }
    }
}
