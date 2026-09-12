using System.Collections.Generic;
using HexagonalDDD.Domain.Repositories;
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

            var unitOfWork = new FakeRentalUnitOfWork();

            var handler = new RentVehicleHandler(
                vehicleRepository,
                rentalRepository,
                unitOfWork);

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
            var unitOfWork = new FakeRentalUnitOfWork();

            var handler = new RentVehicleHandler(
                vehicleRepository,
                rentalRepository,
                unitOfWork);

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

            Assert.IsNotNull(unitOfWork.SavedRental);

            Assert.AreEqual(
                customerId,
                unitOfWork.SavedRental.CustomerId);

            Assert.AreEqual(
                vehicle.Id,
                unitOfWork.SavedRental.VehicleId);

            Assert.IsTrue(
                unitOfWork.SavedRental.IsActive);
        }
        [TestMethod]
        public async Task ExecuteAsync_WhenUnitOfWorkFails_ShouldNotSaveVehicleSeparately()
        {
            // Arrange
            var vehicle = VehicleAggregate.Create(
                "9999ZZZ",
                "Test",
                "Test",
                DateTime.Today.AddYears(-1));

            var vehicleRepository = new RecordingVehicleRepository
            {
                Vehicle = vehicle
            };

            var rentalRepository = new FakeRentalRepository();

            var unitOfWork = new FailingRentalUnitOfWork();

            var handler = new RentVehicleHandler(
                vehicleRepository,
                rentalRepository,
                unitOfWork);

            var command = new RentVehicleCommand
            {
                VehicleId = vehicle.Id,
                CustomerId = Guid.NewGuid()
            };

            // Act
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => handler.ExecuteAsync(command));

            // Assert
            Assert.IsFalse(vehicleRepository.SaveCalled);
        }

        private class RecordingVehicleRepository : IVehicleRepository
        {
            public VehicleAggregate Vehicle { get; set; }

            public bool SaveCalled { get; private set; }

            public Task<VehicleAggregate> GetByIdAsync(Guid id)
            {
                return Task.FromResult(Vehicle);
            }

            public Task SaveAsync(VehicleAggregate vehicle)
            {
                SaveCalled = true;
                return Task.CompletedTask;
            }

            public Task<IReadOnlyList<VehicleAggregate>> GetAvailableAsync()
            {
                throw new NotImplementedException();
            }

            public Task<IReadOnlyList<VehicleAggregate>> GetRentedAsync()
            {
                throw new NotImplementedException();
            }
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

            var firstUnitOfWork = new FakeRentalUnitOfWork();

            var firstHandler = new RentVehicleHandler(
                firstVehicleRepository,
                rentalRepository,
                firstUnitOfWork);

            await firstHandler.ExecuteAsync(
                new RentVehicleCommand
                {
                    VehicleId = firstVehicle.Id,
                    CustomerId = customerId
                });

            rentalRepository.Rental =
                firstUnitOfWork.SavedRental;

            var secondVehicleRepository = new FakeVehicleRepository
            {
                Vehicle = secondVehicle
            };

            var secondUnitOfWork = new FakeRentalUnitOfWork();

            var secondHandler = new RentVehicleHandler(
                secondVehicleRepository,
                rentalRepository,
                secondUnitOfWork);

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
        private class FakeRentalUnitOfWork : IRentalUnitOfWork
        {
            public VehicleAggregate SavedVehicle { get; private set; }
            public RentalAggregate SavedRental { get; private set; }

            public Task SaveRentalAsync(
                VehicleAggregate vehicle,
                RentalAggregate rental)
            {
                SavedVehicle = vehicle;
                SavedRental = rental;

                return Task.CompletedTask;
            }
        }
        private class FailingRentalUnitOfWork : IRentalUnitOfWork
        {
            public Task SaveRentalAsync(
                VehicleAggregate vehicle,
                RentalAggregate rental)
            {
                throw new InvalidOperationException(
                    "Simulated transactional persistence failure.");
            }
        }
    }

}