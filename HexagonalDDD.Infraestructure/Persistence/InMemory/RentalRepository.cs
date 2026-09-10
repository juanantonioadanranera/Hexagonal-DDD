using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Rental;
using HexagonalDDD.Domain.Repositories;

namespace HexagonalDDD.Infraestructure.Persistence.InMemory
{
    public class RentalRepository : IRentalRepository
    {
        private readonly List<RentalAggregate> _rentals =
            new List<RentalAggregate>();

        public Task SaveAsync(RentalAggregate rental)
        {
            var existing = _rentals.FirstOrDefault(
                x => x.Id == rental.Id);

            if (existing == null)
            {
                _rentals.Add(rental);
            }

            return Task.CompletedTask;
        }

        public Task<RentalAggregate> GetActiveByCustomerIdAsync(
            Guid customerId)
        {
            var rental = _rentals.FirstOrDefault(
                x => x.CustomerId == customerId &&
                     x.IsActive);

            return Task.FromResult(rental);
        }

        public Task<RentalAggregate> GetActiveByVehicleIdAsync(
            Guid vehicleId)
        {
            var rental = _rentals.FirstOrDefault(
                x => x.VehicleId == vehicleId &&
                     x.IsActive);

            return Task.FromResult(rental);
        }

        public Task<RentalAggregate> GetByIdAsync(Guid id)
        {
            var rental = _rentals.FirstOrDefault(
                x => x.Id == id);

            return Task.FromResult(rental);
        }
    }
}