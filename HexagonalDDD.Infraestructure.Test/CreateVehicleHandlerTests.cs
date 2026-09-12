using HexagonalDDD.Application.UseCases.Create_Vehicle;
using HexagonalDDD.Domain.Aggregates.Vehicle;
using HexagonalDDD.Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HexagonalDDD.Infraestructure.Test
{
    [TestClass]
    public class CreateVehicleHandlerTests
    {
        [TestMethod]
        public async Task ExecuteAsync_ShouldRejectVehicleOlderThanFiveYears()
        {
            // Arrange
            var repository = new FakeVehicleRepository();
            var handler = new CreateVehicleHandler(repository);

            var command = new CreateVehicleCommand
            {
                RegistrationNumber = "TEST-OLD",
                Brand = "Test",
                Model = "Old vehicle",
                ManufactureDate = DateTime.Today.AddYears(-6)
            };

            // Act + Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => handler.ExecuteAsync(command));
            Assert.IsFalse(repository.SaveCalled);
        }

        private class FakeVehicleRepository : IVehicleRepository
        {
            public bool SaveCalled { get; private set; }

            public Task SaveAsync(VehicleAggregate vehicle)
            {
                SaveCalled = true;
                return Task.CompletedTask;
            }

            public Task<VehicleAggregate> GetByIdAsync(Guid id)
            {
                throw new NotImplementedException();
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
    }
}