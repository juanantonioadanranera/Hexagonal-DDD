using System;
using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Rental;
using HexagonalDDD.Domain.Repositories;

namespace HexagonalDDD.Application.UseCases.Rent_Vehicle
{
    public class RentVehicleHandler
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IRentalUnitOfWork _unitOfWork;

        public RentVehicleHandler(
            IVehicleRepository vehicleRepository,
            IRentalRepository rentalRepository,
            IRentalUnitOfWork unitOfWork)
        {
            _vehicleRepository = vehicleRepository;
            _rentalRepository = rentalRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(RentVehicleCommand command)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(
                command.VehicleId);

            if (vehicle == null)
            {
                throw new InvalidOperationException(
                    "The vehicle does not exist.");
            }

            var activeRental =
                await _rentalRepository.GetActiveByCustomerIdAsync(
                    command.CustomerId);

            if (activeRental != null)
            {
                throw new InvalidOperationException(
                    "The customer already has an active rental.");
            }

            vehicle.Rent();

            var rental = RentalAggregate.Create(
                command.VehicleId,
                command.CustomerId);

            await _unitOfWork.SaveRentalAsync(
                vehicle,
                rental);
        }
    }
}