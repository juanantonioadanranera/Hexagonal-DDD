using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Vehicle;
using HexagonalDDD.Domain.Repositories;

namespace HexagonalDDD.Infraestructure.Persistence.InMemory
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly List<VehicleAggregate> _vehicles =
            new List<VehicleAggregate>();

        public Task SaveAsync(VehicleAggregate vehicle)
        {
            var existing = _vehicles.FirstOrDefault(
                x => x.Id == vehicle.Id);

            if (existing == null)
            {
                _vehicles.Add(vehicle);
            }

            return Task.CompletedTask;
        }

        public Task<VehicleAggregate> GetByIdAsync(Guid id)
        {
            var vehicle = _vehicles.FirstOrDefault(
                x => x.Id == id);

            return Task.FromResult(vehicle);
        }

        public Task<IReadOnlyList<VehicleAggregate>> GetAvailableAsync()
        {
            IReadOnlyList<VehicleAggregate> vehicles =
                _vehicles
                    .Where(x => x.Status == VehicleStatus.Available)
                    .ToList();

            return Task.FromResult(vehicles);
        }
        public Task<IReadOnlyList<VehicleAggregate>> GetRentedAsync()
        {
            IReadOnlyList<VehicleAggregate> vehicles =
                _vehicles
                    .Where(x => x.Status == VehicleStatus.Rented)
                    .ToList();

            return Task.FromResult(vehicles);
        }
    }
}