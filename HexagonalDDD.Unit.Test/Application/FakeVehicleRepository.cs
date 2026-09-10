using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Vehicle;
using HexagonalDDD.Domain.Repositories;

namespace HexagonalDDD.Unit.Test.Application
{
    public class FakeVehicleRepository : IVehicleRepository
    {
        public VehicleAggregate Vehicle { get; set; }

        public Task SaveAsync(VehicleAggregate vehicle)
        {
            Vehicle = vehicle;
            return Task.CompletedTask;
        }

        public Task<VehicleAggregate> GetByIdAsync(Guid id)
        {
            return Task.FromResult(
                Vehicle != null && Vehicle.Id == id
                    ? Vehicle
                    : null);
        }

        public Task<IReadOnlyList<VehicleAggregate>> GetAvailableAsync()
        {
            IReadOnlyList<VehicleAggregate> vehicles =
                Vehicle != null &&
                Vehicle.Status == VehicleStatus.Available
                    ? new List<VehicleAggregate> { Vehicle }
                    : new List<VehicleAggregate>();

            return Task.FromResult(vehicles);
        }
        public Task<IReadOnlyList<VehicleAggregate>> GetRentedAsync()
        {
            IReadOnlyList<VehicleAggregate> vehicles =
                Vehicle != null && Vehicle.Status == VehicleStatus.Rented
                    ? new List<VehicleAggregate> { Vehicle }
                    : new List<VehicleAggregate>();

            return Task.FromResult(vehicles);
        }
    }
}