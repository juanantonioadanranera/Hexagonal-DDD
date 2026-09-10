using System;
using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Rental;
using HexagonalDDD.Domain.Repositories;

namespace HexagonalDDD.Unit.Test.Application
{
    public class FakeRentalRepository : IRentalRepository
    {
        public RentalAggregate Rental { get; set; }

        public Task SaveAsync(RentalAggregate rental)
        {
            Rental = rental;
            return Task.CompletedTask;
        }

        public Task<RentalAggregate> GetActiveByCustomerIdAsync(
            Guid customerId)
        {
            if (Rental != null &&
                Rental.CustomerId == customerId &&
                Rental.IsActive)
            {
                return Task.FromResult(Rental);
            }

            return Task.FromResult<RentalAggregate>(null);
        }

        public Task<RentalAggregate> GetActiveByVehicleIdAsync(
            Guid vehicleId)
        {
            if (Rental != null &&
                Rental.VehicleId == vehicleId &&
                Rental.IsActive)
            {
                return Task.FromResult(Rental);
            }

            return Task.FromResult<RentalAggregate>(null);
        }

        public Task<RentalAggregate> GetByIdAsync(Guid id)
        {
            if (Rental != null && Rental.Id == id)
            {
                return Task.FromResult(Rental);
            }

            return Task.FromResult<RentalAggregate>(null);
        }
    }
}