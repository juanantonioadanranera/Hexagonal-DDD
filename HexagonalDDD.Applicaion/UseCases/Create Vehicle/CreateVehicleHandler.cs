using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Vehicle;
using HexagonalDDD.Domain.Repositories;

namespace HexagonalDDD.Application.UseCases.Create_Vehicle
{
    public class CreateVehicleHandler
    {
        private readonly IVehicleRepository _repository;

        public CreateVehicleHandler(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(CreateVehicleCommand command)
        {
            var vehicle = VehicleAggregate.Create(
                command.RegistrationNumber,
                command.Brand,
                command.Model,
                command.ManufactureDate);

            await _repository.SaveAsync(vehicle);
        }
    }
}