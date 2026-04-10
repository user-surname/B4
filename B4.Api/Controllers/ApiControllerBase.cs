using B4.Models.Common;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace B4.Api.Controllers;

/// <summary>
/// Base para todos los controllers de B4.
/// Expone helpers tipados que devuelven ApiResponse y miden el tiempo de ejecución.
/// </summary>
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    // ── Helpers tipados ──────────────────────────────────────────────────────

    protected IActionResult OkResponse<T>(T data, Stopwatch? sw = null)
        => Ok(ApiResponse<T>.Ok(data, sw?.ElapsedMilliseconds ?? 0));

    protected IActionResult CreatedResponse<T>(string actionName, object routeValues, T data)
        => CreatedAtAction(actionName, routeValues, ApiResponse<T>.Created(data));

    protected IActionResult NotFoundResponse<T>(string msg)
        => NotFound(ApiResponse<T>.NotFound(msg));

    protected IActionResult BadRequestResponse<T>(string msg)
        => BadRequest(ApiResponse<T>.BadRequest(msg));

    protected IActionResult UnauthorizedResponse<T>(string msg = "No estás autorizado")
        => Unauthorized(ApiResponse<T>.Unauthorized(msg));

    protected IActionResult ServerErrorResponse<T>(string msg = "Error interno del servidor")
        => StatusCode(500, ApiResponse<T>.ServerError(msg));

    // ── Helper para ejecutar y medir en un solo paso ─────────────────────────

    protected async Task<IActionResult> ExecuteAsync<T>(Func<Task<T?>> operation, string notFoundMsg)
    {
        var sw = Stopwatch.StartNew();
        var result = await operation();
        sw.Stop();

        return result is null
            ? NotFoundResponse<T>(notFoundMsg)
            : OkResponse(result, sw);
    }
}