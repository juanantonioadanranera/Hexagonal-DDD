using HexagonalDDD.Application.UseCases.Get_Rented_Vehicles;
using HexagonalDDD.Application.UseCases.Get_Customers;
using HexagonalDDD.Application.UseCases.Create_Customer;
using HexagonalDDD.Application.UseCases.Create_Sample;
using HexagonalDDD.Application.UseCases.Create_Vehicle;
using HexagonalDDD.Application.UseCases.Get_Available_Vehicles;
using HexagonalDDD.Application.UseCases.Rent_Vehicle;
using HexagonalDDD.Application.UseCases.Return_Vehicle;
using HexagonalDDD.Domain.Aggregates.Rental;
using HexagonalDDD.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace HexagonalDDD.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddTransient<CreateSampleHandler>();
            services.AddTransient<CreateVehicleHandler>();
            services.AddTransient<GetAvailableVehiclesHandler>();
            services.AddTransient<RentVehicleHandler>();
            services.AddTransient<ReturnVehicleHandler>();
            services.AddTransient<CreateCustomerHandler>();
            services.AddTransient<GetCustomersHandler>();
            services.AddTransient<GetRentedVehiclesHandler>();
            return services;
        }
    }
}
