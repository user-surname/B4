using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Nodes;
using App1.Features.Common.Interfaces;
using App1.Features.Common.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App1.Features.Controllers.ViewModels;

public partial class ControllersViewModel(
    IApiMetadataService metadataService,
    IApiCrudService apiCrudService) : ObservableObject
{
    public ObservableCollection<ApiEndpointDefinition> Endpoints { get; } =
        new(metadataService.GetCrudEndpoints());

    [ObservableProperty]
    private ApiEndpointDefinition? selectedEndpoint;

    [ObservableProperty]
    private string getByIdText = "1";

    [ObservableProperty]
    private string createJson = "{ }";

    [ObservableProperty]
    private string resultJson = string.Empty;

    public ObservableCollection<Dictionary<string, string>> TableRows { get; } = [];
    public ObservableCollection<string> TableColumns { get; } = [];

    [ObservableProperty]
    private string statusMessage = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    partial void OnSelectedEndpointChanged(ApiEndpointDefinition? value)
    {
        if (value is null)
        {
            return;
        }

        ResultJson = string.Empty;
        TableRows.Clear();
        TableColumns.Clear();
        StatusMessage = $"Controller seleccionado: {value.ControllerName}";
    }

    [RelayCommand]
    private async Task GetAllAsync()
    {
        if (SelectedEndpoint is null)
        {
            StatusMessage = "Selecciona un controller.";
            return;
        }

        await ExecuteAsync(async () =>
        {
            var result = await apiCrudService.GetAllAsync(SelectedEndpoint.RouteSegment);
            BuildTableFromGetAllResponse(result);
            StatusMessage = TableRows.Count > 0 ? $"GetAll OK. {TableRows.Count} filas." : "GetAll OK.";
        });
    }

    [RelayCommand]
    private async Task GetByIdAsync()
    {
        if (SelectedEndpoint is null)
        {
            StatusMessage = "Selecciona un controller.";
            return;
        }

        if (!int.TryParse(GetByIdText, out var id))
        {
            StatusMessage = "El id debe ser numérico.";
            return;
        }

        await ExecuteAsync(async () =>
        {
            var result = await apiCrudService.GetByIdAsync(SelectedEndpoint.RouteSegment, id);
            BuildTableFromResponseData(result);
            StatusMessage = TableRows.Count > 0 ? "GetById OK." : "GetById OK (sin datos tabulares).";
        });
    }

    [RelayCommand]
    private async Task CreateAsync()
    {
        if (SelectedEndpoint is null)
        {
            StatusMessage = "Selecciona un controller.";
            return;
        }

        await ExecuteAsync(async () =>
        {
            var result = await apiCrudService.CreateAsync(SelectedEndpoint.RouteSegment, CreateJson);
            ResultJson = Format(result);
            StatusMessage = "Create OK.";
        });
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (SelectedEndpoint is null)
        {
            StatusMessage = "Selecciona un controller.";
            return;
        }

        if (!int.TryParse(GetByIdText, out var id))
        {
            StatusMessage = "El id debe ser numérico.";
            return;
        }

        await ExecuteAsync(async () =>
        {
            await apiCrudService.DeleteAsync(SelectedEndpoint.RouteSegment, id);
            ResultJson = "{ \"deleted\": true }";
            StatusMessage = "Delete OK.";
        });
    }

    private async Task ExecuteAsync(Func<Task> action)
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            await action();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void BuildTableFromGetAllResponse(JsonNode? response)
    {
        BuildTableFromResponseData(response);
    }

    private void BuildTableFromResponseData(JsonNode? response)
    {
        TableRows.Clear();
        TableColumns.Clear();

        var dataNode = response?["data"];
        if (dataNode is null)
        {
            ResultJson = Format(response);
            return;
        }

        List<Dictionary<string, string>> rows;

        if (dataNode is JsonArray dataArray)
        {
            rows = dataArray
                .OfType<JsonObject>()
                .Select(ToDictionary)
                .ToList();
        }
        else if (dataNode is JsonObject dataObject)
        {
            rows = [ToDictionary(dataObject)];
        }
        else
        {
            ResultJson = Format(response);
            return;
        }

        if (rows.Count == 0)
        {
            ResultJson = Format(response);
            return;
        }

        var columns = rows
            .SelectMany(r => r.Keys)
            .Distinct()
            .OrderBy(k => k)
            .ToList();

        foreach (var column in columns)
        {
            TableColumns.Add(column);
        }

        foreach (var row in rows)
        {
            var normalized = columns.ToDictionary(
                c => c,
                c => row.TryGetValue(c, out var value) ? value : string.Empty);
            TableRows.Add(normalized);
        }

        ResultJson = string.Empty;
    }

    private static Dictionary<string, string> ToDictionary(JsonObject obj) =>
        obj.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value?.ToJsonString() ?? string.Empty);

    private static string Format(JsonNode? node) => node?.ToJsonString(new() { WriteIndented = true }) ?? "null";
}
