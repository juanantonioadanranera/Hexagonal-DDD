using Microsoft.Extensions.DependencyInjection;
using HexagonalDDD.Domain.Repositories;
using HexagonalDDD.Infraestructure.Persistence.InMemory;

namespace HexagonalDDD.Infraestructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddSingleton<IVehicleRepository, VehicleRepository>();
            services.AddSingleton<IRentalRepository, RentalRepository>();
            services.AddSingleton<ICustomerRepository, CustomerRepository>();

            return services;
        }
    }
}