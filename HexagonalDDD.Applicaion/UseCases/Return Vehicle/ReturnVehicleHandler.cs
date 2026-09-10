using System;
using System.Threading.Tasks;
using HexagonalDDD.Domain.Repositories;

namespace HexagonalDDD.Application.UseCases.Return_Vehicle
{
    public class ReturnVehicleHandler
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRentalRepository _rentalRepository;

        public ReturnVehicleHandler(
            IVehicleRepository vehicleRepository,
            IRentalRepository rentalRepository)
        {
            _vehicleRepository = vehicleRepository;
            _rentalRepository = rentalRepository;
        }

        public async Task ExecuteAsync(ReturnVehicleCommand command)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(
                command.VehicleId);

            if (vehicle == null)
            {
                throw new InvalidOperationException(
                    "The vehicle does not exist.");
            }

            var rental = await _rentalRepository.GetActiveByVehicleIdAsync(
                command.VehicleId);

            if (rental == null)
            {
                throw new InvalidOperationException(
                    "The vehicle does not have an active rental.");
            }

            vehicle.Return();
            rental.Return();

            await _vehicleRepository.SaveAsync(vehicle);
            await _rentalRepository.SaveAsync(rental);
        }
    }
}