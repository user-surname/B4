using App1.Features.Common.Models;

namespace App1.Features.Common.Interfaces;

public interface IApiMetadataService
{
    IReadOnlyList<ApiEndpointDefinition> GetCrudEndpoints();
}
