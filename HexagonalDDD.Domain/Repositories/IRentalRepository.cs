using System;
using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Rental;

namespace HexagonalDDD.Domain.Repositories
{
    public interface IRentalRepository
    {
        Task SaveAsync(RentalAggregate rental);

        Task<RentalAggregate> GetActiveByCustomerIdAsync(Guid customerId);

        Task<RentalAggregate> GetActiveByVehicleIdAsync(Guid vehicleId);

        Task<RentalAggregate> GetByIdAsync(Guid id);
    }
}