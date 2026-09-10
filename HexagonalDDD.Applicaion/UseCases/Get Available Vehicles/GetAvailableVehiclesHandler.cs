using System.Collections.Generic;
using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Vehicle;
using HexagonalDDD.Domain.Repositories;

namespace HexagonalDDD.Application.UseCases.Get_Available_Vehicles
{
    public class GetAvailableVehiclesHandler
    {
        private readonly IVehicleRepository _repository;

        public GetAvailableVehiclesHandler(
            IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<VehicleAggregate>> ExecuteAsync()
        {
            return await _repository.GetAvailableAsync();
        }
    }
}