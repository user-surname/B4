using App1.Features.Controllers.ViewModels;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using System.Collections.Specialized;
using Microsoft.UI.Xaml.Data;

namespace App1.Features.Controllers.Views;

public sealed partial class ControllersPage : Page
{
    public ControllersViewModel ViewModel { get; }

    public ControllersPage()
    {
        InitializeComponent();
        ViewModel = App.Services.GetRequiredService<ControllersViewModel>();
        DataContext = ViewModel;
        ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        ViewModel.TableColumns.CollectionChanged += TableColumnsOnCollectionChanged;
        RebuildGridColumns();
    }

    private async void GetAll_OnClick(object sender, RoutedEventArgs e) => await ViewModel.GetAllCommand.ExecuteAsync(null);

    private async void GetById_OnClick(object sender, RoutedEventArgs e) => await ViewModel.GetByIdCommand.ExecuteAsync(null);

    private async void Create_OnClick(object sender, RoutedEventArgs e) => await ViewModel.CreateCommand.ExecuteAsync(null);

    private async void Delete_OnClick(object sender, RoutedEventArgs e) => await ViewModel.DeleteCommand.ExecuteAsync(null);

    private void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ControllersViewModel.SelectedEndpoint))
        {
            RebuildGridColumns();
        }
        else if (e.PropertyName == nameof(ControllersViewModel.ResultJson))
        {
            RawResultBox.Visibility = string.IsNullOrWhiteSpace(ViewModel.ResultJson)
                ? Visibility.Collapsed
                : Visibility.Visible;
        }
    }

    private void RebuildGridColumns()
    {
        ResultsGrid.Columns.Clear();

        foreach (var columnName in ViewModel.TableColumns)
        {
            var column = new DataGridTextColumn
            {
                Header = columnName,
                Binding = new Binding
                {
                    Path = new PropertyPath($"[{columnName}]"),
                    Mode = BindingMode.OneWay
                }
            };
            ResultsGrid.Columns.Add(column);
        }
    }

    private void TableColumnsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        RebuildGridColumns();
    }
}
