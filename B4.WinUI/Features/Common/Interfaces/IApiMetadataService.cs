using B4.WinUI.Features.Common.Models;

namespace B4.WinUI.Features.Common.Interfaces;

public interface IApiMetadataService
{
    IReadOnlyList<ApiEndpointDefinition> GetCrudEndpoints();
}
