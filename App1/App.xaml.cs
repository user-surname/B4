using App1.Features.Common.Extensions.DI;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using System;

namespace App1
{
    public partial class App : Application
    {
        private readonly IHost _host;
        private Window? _window;

        public static IServiceProvider Services =>
            (Current as App)?._host.Services ?? throw new InvalidOperationException("Host not initialized.");

        public App()
        {
            InitializeComponent();
            _host = CompositionRoot.CreateHostBuilder().Build();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
        }
    }
}
