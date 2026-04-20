using B4.WinUI.Features.Auth.Models;
using B4.WinUI.Features.Common.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace B4.WinUI.Features.Auth.ViewModels;

public partial class LoginViewModel(IAuthService authService) : ObservableObject
{
    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? statusMessage;

    public event EventHandler? LoginSucceeded;

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            StatusMessage = null;
            await authService.LoginAsync(new LoginRequest { Email = Email, Password = Password });
            StatusMessage = "Login correcto.";
            LoginSucceeded?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error de login: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
