using B4.WinUI.Features.Auth.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace B4.WinUI.Pages;

public sealed partial class LoginPage : Page
{
    public LoginViewModel ViewModel { get; }

    public event EventHandler? LoginSucceeded;

    public LoginPage()
    {
        InitializeComponent();
        ViewModel = App.Services.GetRequiredService<LoginViewModel>();
        ViewModel.LoginSucceeded += (_, _) => LoginSucceeded?.Invoke(this, EventArgs.Empty);
        DataContext = ViewModel;
    }

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox pb)
        {
            ViewModel.Password = pb.Password;
        }
    }

    private async void LoginButton_OnClick(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoginCommand.ExecuteAsync(null);
    }
}
