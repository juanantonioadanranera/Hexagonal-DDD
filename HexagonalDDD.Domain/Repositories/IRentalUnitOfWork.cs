using System.Threading.Tasks;
using HexagonalDDD.Domain.Aggregates.Rental;
using HexagonalDDD.Domain.Aggregates.Vehicle;

namespace HexagonalDDD.Domain.Repositories
{
    public interface IRentalUnitOfWork
    {
        Task SaveRentalAsync(
            VehicleAggregate vehicle,
            RentalAggregate rental);
    }
}