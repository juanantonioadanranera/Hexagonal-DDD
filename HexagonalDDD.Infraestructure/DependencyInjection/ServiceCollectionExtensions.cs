using HexagonalDDD.Domain.Repositories;
using HexagonalDDD.Infraestructure.Persistence.InMemory;
using HexagonalDDD.Infraestructure.Persistence.Oracle;
using Microsoft.Extensions.DependencyInjection;
using WPFHexagonalDDD.Infraestructure;

namespace HexagonalDDD.Infraestructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {

            services.AddTransient<Entities>();
            //services.AddSingleton<IVehicleRepository, VehicleRepository>();
            services.AddTransient<IVehicleRepository, OracleVehicleRepository>();
            //services.AddSingleton<IRentalRepository, RentalRepository>();
            services.AddTransient<IRentalRepository, OracleRentalRepository>();
            //services.AddSingleton<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ICustomerRepository, OracleCustomerRepository>();
            return services;
        }
    }
}