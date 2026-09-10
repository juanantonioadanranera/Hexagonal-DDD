using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HexagonalDDD.Application.UseCases.Rent_Vehicle;
using HexagonalDDD.Domain.Aggregates.Rental;
using HexagonalDDD.Domain.Aggregates.Vehicle;

namespace HexagonalDDD.Unit.Test.Application
{
    [TestClass]
    public class RentVehicleHandlerTests
    {
        [TestMethod]
        public async Task ExecuteAsync_ShouldRejectCustomerWithActiveRental()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            var vehicle = VehicleAggregate.Create(
                "1234ABC",
                "Toyota",
                "Corolla",
                DateTime.Today.AddYears(-2));

            var existingRental = RentalAggregate.Create(
                Guid.NewGuid(),
                customerId);

            var vehicleRepository = new FakeVehicleRepository
            {
                Vehicle = vehicle
            };

            var rentalRepository = new FakeRentalRepository
            {
                Rental = existingRental
            };

            var handler = new RentVehicleHandler(
                vehicleRepository,
                rentalRepository);

            var command = new RentVehicleCommand
            {
                VehicleId = vehicle.Id,
                CustomerId = customerId
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => handler.ExecuteAsync(command));
        }
        [TestMethod]
        public async Task ExecuteAsync_ShouldCreateRentalWhenCustomerHasNoActiveRental()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            var vehicle = VehicleAggregate.Create(
                "5678DEF",
                "Ford",
                "Focus",
                DateTime.Today.AddYears(-1));

            var vehicleRepository = new FakeVehicleRepository
            {
                Vehicle = vehicle
            };

            var rentalRepository = new FakeRentalRepository();

            var handler = new RentVehicleHandler(
                vehicleRepository,
                rentalRepository);

            var command = new RentVehicleCommand
            {
                VehicleId = vehicle.Id,
                CustomerId = customerId
            };

            // Act
            await handler.ExecuteAsync(command);

            // Assert
            Assert.AreEqual(
                VehicleStatus.Rented,
                vehicleRepository.Vehicle.Status);

            Assert.IsNotNull(rentalRepository.Rental);

            Assert.AreEqual(
                customerId,
                rentalRepository.Rental.CustomerId);

            Assert.AreEqual(
                vehicle.Id,
                rentalRepository.Rental.VehicleId);

            Assert.IsTrue(
                rentalRepository.Rental.IsActive);
        }
        [TestMethod]
        public async Task ExecuteAsync_ShouldRejectSecondVehicleForSameCustomer()
        {
            // Arrange
            var firstVehicle = VehicleAggregate.Create(
                "1111AAA",
                "Toyota",
                "Corolla",
                DateTime.Today.AddYears(-2));

            var secondVehicle = VehicleAggregate.Create(
                "2222BBB",
                "Ford",
                "Focus",
                DateTime.Today.AddYears(-1));

            var customerId = Guid.NewGuid();

            var rentalRepository = new FakeRentalRepository();

            var firstVehicleRepository = new FakeVehicleRepository
            {
                Vehicle = firstVehicle
            };

            var firstHandler = new RentVehicleHandler(
                firstVehicleRepository,
                rentalRepository);

            await firstHandler.ExecuteAsync(
                new RentVehicleCommand
                {
                    VehicleId = firstVehicle.Id,
                    CustomerId = customerId
                });

            var secondVehicleRepository = new FakeVehicleRepository
            {
                Vehicle = secondVehicle
            };

            var secondHandler = new RentVehicleHandler(
                secondVehicleRepository,
                rentalRepository);

            // Act + Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () =>
                {
                    await secondHandler.ExecuteAsync(
                        new RentVehicleCommand
                        {
                            VehicleId = secondVehicle.Id,
                            CustomerId = customerId
                        });
                });

            Assert.AreEqual(
                VehicleStatus.Available,
                secondVehicle.Status);
        }
    }

}