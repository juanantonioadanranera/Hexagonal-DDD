using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HexagonalDDD.Application.UseCases.Return_Vehicle;
using HexagonalDDD.Domain.Aggregates.Rental;
using HexagonalDDD.Domain.Aggregates.Vehicle;

namespace HexagonalDDD.Unit.Test.Application
{
    [TestClass]
    public class ReturnVehicleHandlerTests
    {
        [TestMethod]
        public async Task ExecuteAsync_ShouldReturnVehicleAndCloseRental()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            var vehicle = VehicleAggregate.Create(
                "9012GHI",
                "Seat",
                "Leon",
                DateTime.Today.AddYears(-2));

            vehicle.Rent();

            var rental = RentalAggregate.Create(
                vehicle.Id,
                customerId);

            var vehicleRepository = new FakeVehicleRepository
            {
                Vehicle = vehicle
            };

            var rentalRepository = new FakeRentalRepository
            {
                Rental = rental
            };

            var handler = new ReturnVehicleHandler(
                vehicleRepository,
                rentalRepository);

            var command = new ReturnVehicleCommand
            {
                VehicleId = vehicle.Id
            };

            // Act
            await handler.ExecuteAsync(command);

            // Assert
            Assert.AreEqual(
                VehicleStatus.Available,
                vehicleRepository.Vehicle.Status);

            Assert.IsNotNull(
                rentalRepository.Rental.ReturnDate);

            Assert.IsFalse(
                rentalRepository.Rental.IsActive);
        }
    }
}