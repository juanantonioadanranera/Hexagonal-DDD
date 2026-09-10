using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WPF_Hexagonal_DDD
{
    public static class Bootstrapper
    {
        public static IHost BuildHost()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {

                })
                .Build();
        }
    }
}
