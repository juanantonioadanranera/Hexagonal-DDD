using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HexagonalDDD.Application.DependencyInjection;
using HexagonalDDD.Infraestructure.DependencyInjection;

namespace WPF_Hexagonal_DDD
{
    public static class Bootstrapper
    {
        public static IHost BuildHost()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddApplication();
                    services.AddInfrastructure();
                    services.AddTransient<MainWindow>();
                })
                .Build();
        }
    }
}