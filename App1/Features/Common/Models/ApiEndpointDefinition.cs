namespace App1.Features.Common.Models;

public sealed class ApiEndpointDefinition
{
    public required string ControllerName { get; init; }
    public required string RouteSegment { get; init; }
    public bool SupportsGetAll { get; init; }
    public bool SupportsGetById { get; init; }
    public bool SupportsCreate { get; init; }
    public bool SupportsDelete { get; init; }
}
