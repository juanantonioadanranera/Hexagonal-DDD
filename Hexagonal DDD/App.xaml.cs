using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WPF_Hexagonal_DDD
{
    public partial class App : Application
    {
        private IHost _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _host = Bootstrapper.BuildHost();
            _host.Start();

            var mainWindow =
                _host.Services.GetRequiredService<MainWindow>();

            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host?.StopAsync().GetAwaiter().GetResult();
            _host?.Dispose();

            base.OnExit(e);
        }
    }
}