using System.Collections.Generic;
using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Vehicle;
using HexagonalDDD.Domain.Repositories;

namespace HexagonalDDD.Application.UseCases.Get_Rented_Vehicles
{
    public class GetRentedVehiclesHandler
    {
        private readonly IVehicleRepository _repository;

        public GetRentedVehiclesHandler(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<VehicleAggregate>> ExecuteAsync()
        {
            return await _repository.GetRentedAsync();
        }
    }
}