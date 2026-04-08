using App1.Features.Auth.Views;
using App1.Features.Controllers.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace App1
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigateToLogin();
        }

        private void NavigateToLogin()
        {
            var loginPage = new LoginPage();
            loginPage.LoginSucceeded += (_, _) =>
            {
                ControllersNavItem.IsEnabled = true;
                AppNavView.SelectedItem = ControllersNavItem;
                ContentFrame.Content = new ControllersPage();
            };

            AppNavView.SelectedItem = LoginNavItem;
            ContentFrame.Content = loginPage;
        }

        private void AppNavView_OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is not NavigationViewItem selected)
            {
                return;
            }

            switch (selected.Tag?.ToString())
            {
                case "Login":
                    NavigateToLogin();
                    break;
                case "Controllers":
                    ContentFrame.Content = new ControllersPage();
                    break;
            }
        }
    }
}
