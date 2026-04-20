using B4.WinUI.Pages;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using Windows.System;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media.Animation;

namespace B4.WinUI
{
    public sealed partial class MainWindow : Window
    {
        private const double SidebarExpandedWidth = 220;
        private const double SidebarCompactWidth = 58;
        private bool _isPaneOpen = true;

        public MainWindow()
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            ApplySidebarCompactState();
            NavigateToLogin();
        }

        private void NavigateToLogin()
        {
            LoginNavButton.Style = (Style)Application.Current.Resources["AccentButtonStyle"];
            ControllersNavButton.ClearValue(FrameworkElement.StyleProperty);
            SettingsNavButton.ClearValue(FrameworkElement.StyleProperty);
            var loginPage = new LoginPage();
            loginPage.LoginSucceeded += (_, _) =>
            {
                ControllersNavButton.IsEnabled = true;
                NavigateToControllers();
            };

            ContentFrame.Content = loginPage;
        }

        private void NavigateToControllers()
        {
            ControllersNavButton.Style = (Style)Application.Current.Resources["AccentButtonStyle"];
            LoginNavButton.ClearValue(FrameworkElement.StyleProperty);
            SettingsNavButton.ClearValue(FrameworkElement.StyleProperty);
            ContentFrame.Content = new ControllersPage();
        }

        private void NavigateToSettings()
        {
            SettingsNavButton.Style = (Style)Application.Current.Resources["AccentButtonStyle"];
            LoginNavButton.ClearValue(FrameworkElement.StyleProperty);
            ControllersNavButton.ClearValue(FrameworkElement.StyleProperty);
            ContentFrame.Content = new SettingsPage();
        }

        private async void DonateButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://ko-fi.com/memstechtips"));
        }

        private async void BugButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/memstechtips/Winhance/issues"));
        }

        private async void DocsButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://winhance.net/docs/index.html"));
        }

        private void PaneToggleButton_Click(object sender, RoutedEventArgs e)
        {
            _isPaneOpen = !_isPaneOpen;
            AnimateSidebarWidth(_isPaneOpen ? SidebarExpandedWidth : SidebarCompactWidth);
            ApplySidebarCompactState();
        }

        private void AnimateSidebarWidth(double targetWidth)
        {
            var animation = new DoubleAnimation
            {
                To = targetWidth,
                Duration = TimeSpan.FromMilliseconds(220),
                EnableDependentAnimation = true,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            Storyboard.SetTarget(animation, SidebarPanel);
            Storyboard.SetTargetProperty(animation, nameof(FrameworkElement.Width));

            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }

        private void ApplySidebarCompactState()
        {
            var navTextVisibility = _isPaneOpen ? Visibility.Visible : Visibility.Collapsed;
            LoginNavText.Visibility = navTextVisibility;
            ControllersNavText.Visibility = navTextVisibility;
            SettingsNavText.Visibility = navTextVisibility;
            MoreNavText.Visibility = navTextVisibility;

            var contentAlignment = _isPaneOpen ? HorizontalAlignment.Left : HorizontalAlignment.Center;
            LoginNavButton.HorizontalContentAlignment = contentAlignment;
            ControllersNavButton.HorizontalContentAlignment = contentAlignment;
            SettingsNavButton.HorizontalContentAlignment = contentAlignment;
            MoreButton.HorizontalContentAlignment = contentAlignment;
        }

        private void ControllersNavButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToControllers();
        }

        private void LoginNavButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToLogin();
        }

        private void SettingsNavButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToSettings();
        }

        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Flyout is FlyoutBase flyout)
            {
                flyout.ShowAt(button);
            }
        }

        private void LogsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                XamlRoot = RootGrid.XamlRoot,
                Title = "Open Logs",
                Content = "Visual placeholder. Connect this action to your API logs viewer when ready.",
                CloseButtonText = "OK"
            };
            _ = dialog.ShowAsync();
        }

        private void ScriptsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                XamlRoot = RootGrid.XamlRoot,
                Title = "Open Scripts",
                Content = "Visual placeholder. You can map this to script management later.",
                CloseButtonText = "OK"
            };
            _ = dialog.ShowAsync();
        }

        private void CloseAppMenuItem_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Exit();
        }
    }
}
