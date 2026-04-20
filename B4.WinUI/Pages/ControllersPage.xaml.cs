using B4.WinUI.Features.Controllers.ViewModels;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Collections.Specialized;
using System.ComponentModel;
using Microsoft.UI.Xaml.Data;

namespace B4.WinUI.Pages;

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

    private void TableViewToggle_Checked(object sender, RoutedEventArgs e)
    {
        if (CardViewToggle != null)
        {
            CardViewToggle.IsChecked = false;
        }
    }

    private void TableViewToggle_Unchecked(object sender, RoutedEventArgs e)
    {
        if (CardViewToggle != null && CardViewToggle.IsChecked != true)
        {
            TableViewToggle.IsChecked = true;
        }
    }

    private void CardViewToggle_Checked(object sender, RoutedEventArgs e)
    {
        if (TableViewToggle != null)
        {
            TableViewToggle.IsChecked = false;
        }
    }

    private void CardViewToggle_Unchecked(object sender, RoutedEventArgs e)
    {
        if (TableViewToggle != null && TableViewToggle.IsChecked != true)
        {
            CardViewToggle.IsChecked = true;
        }
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
